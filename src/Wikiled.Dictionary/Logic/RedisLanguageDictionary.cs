using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Wikiled.Common.Arguments;
using Wikiled.Dictionary.Data;
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
            var leftSide = candidates.SelectMany(item => item.Result.Translations).Distinct();

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
            var rightSide = candidates.SelectMany(item => item.Result.Translations).Distinct();
            Dictionary<string, DicVectorCell> englishCells = new Dictionary<string, DicVectorCell>(StringComparer.OrdinalIgnoreCase);
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
