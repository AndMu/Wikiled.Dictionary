using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Wikiled.Common.Arguments;
using Wikiled.Dictionary.Data;
using Wikiled.MachineLearning.Mathematics.Vectors;
using Wikiled.Redis.Keys;
using Wikiled.Redis.Logic;
using Wikiled.Redis.Logic.Pool;

namespace Wikiled.Dictionary.Logic
{
    public class RedisLanguageDictionary : ILanguageDictionary
    {
        private readonly IRedisLink redis;

        public RedisLanguageDictionary(IRedisLinksPool pool)
        {
            Guard.NotNull(() => pool, pool);
            redis = pool.GetKey("D");
            Guard.NotNull(() => redis, redis);
        }

        public string Name => "Dic";

        public async Task<TranslationResult> Translate(TranslationRequest request)
        {
            Guard.NotNull(() => request, request);
            if (request.To == Language.English ||
                request.From == Language.English)
            {
                return await TranslateRaw(request);
            }

            var originalTo = request.To;
            var originalFrom = request.From;
            request.To = Language.English;
            var intermediate = await TranslateRaw(request).ConfigureAwait(false);
            List<Task<TranslationResult>> candidates = new List<Task<TranslationResult>>();
            foreach (var intermediateTranslation in intermediate.Translations)
            {
                var subRequest = new TranslationRequest();
                subRequest.From = Language.English;
                subRequest.To = originalFrom;
                subRequest.Word = intermediateTranslation;
                candidates.Add(TranslateRaw(subRequest));
            }

            await Task.WhenAll(candidates);
            var leftSide = candidates.SelectMany(item => item.Result.Translations).Distinct().ToArray();

            candidates = new List<Task<TranslationResult>>();
            foreach (var intermediateTranslation in intermediate.Translations)
            {
                var subRequest = new TranslationRequest();
                subRequest.From = Language.English;
                subRequest.To = originalTo;
                subRequest.Word = intermediateTranslation;
                candidates.Add(TranslateRaw(subRequest));
            }

            await Task.WhenAll(candidates);
            var candidatesTranslations = candidates.Select(item => item.Result);
            var rightSide = candidates.SelectMany(item => item.Result.Translations).Distinct().ToArray();

            candidates = new List<Task<TranslationResult>>();
            foreach (var result in leftSide)
            {
                var subRequest = new TranslationRequest();
                subRequest.From = originalFrom;
                subRequest.To = Language.English;
                subRequest.Word = result;
                candidates.Add(TranslateRaw(subRequest));
            }

            foreach (var result in rightSide)
            {
                var subRequest = new TranslationRequest();
                subRequest.From = originalTo;
                subRequest.To = Language.English;
                subRequest.Word = result;
                candidates.Add(TranslateRaw(subRequest));
            }

            await Task.WhenAll(candidates);
            DictionaryVectorHelper dictionaryVector = new DictionaryVectorHelper();
            var english = candidates.SelectMany(item => item.Result.Translations).Distinct();
            foreach (var word in english)
            {
                dictionaryVector.AddToDictionary(word);
            }

            Dictionary<string, VectorData> vectors = new Dictionary<string, VectorData>();
            var main = dictionaryVector.GetFullVector(intermediate.Translations);
            foreach (var translationTranslation in candidates)
            {
                var vector = dictionaryVector.GetFullVector(translationTranslation.Result.Translations);
                vectors.Add(translationTranslation.Result.Request.Word, vector);
            }

            var distance = new CosineSimilarityDistance();
            Dictionary<string, double> grabber = new Dictionary<string, double>();
            foreach (var right in candidates.Where(item => item.Result.Request.From == originalTo))
            {
                List<(string, string, double)> vectorsSim = new List<(string, string, double)>();
                foreach (var left in candidates.Where(item => item.Result.Request.From == originalFrom))
                {
                    var distanceLe = distance.Measure(vectors[left.Result.Request.Word], vectors[right.Result.Request.Word]);
                    vectorsSim.Add((left.Result.Request.Word, right.Result.Request.Word, distanceLe));
                }

                var firstSel = vectorsSim.OrderByDescending(item => item.Item3).Take(3);
                foreach (var first in firstSel)
                {
                    if (first.Item1 == request.Word)
                    {
                        grabber[first.Item2] = first.Item3;
                    }
                }
            }

            //var super = vectors.Select(item => new { item.Item2, Distance = distance.Measure(main, item.Item1) }).OrderByDescending(item => item.Distance).ToArray();
            //foreach (var candidate in candidates.Where(item => item.f))
            //{

            //}

            return null;
        }

        public async Task<TranslationResult> TranslateRaw(TranslationRequest request)
        {
            Guard.NotNull(() => request, request);
            var key = new RepositoryKey(this, new ObjectKey(request.From.ToString(), request.To.ToString(), request.Word.ToLower()));
            var results = await redis.Client.GetRecords<string>(key).ToArray();
            return new TranslationResult(request, results);
        }
    }
}
