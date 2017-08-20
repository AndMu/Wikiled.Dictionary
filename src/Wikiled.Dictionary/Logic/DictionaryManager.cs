using System;
using System.Threading;
using System.Threading.Tasks;
using Wikiled.Core.Standard.Api;
using Wikiled.Core.Utility.Arguments;
using Wikiled.Dictionary.Data;

namespace Wikiled.Dictionary.Logic
{
    public class DictionaryManager : IDictionaryManager
    {
        private readonly IApiClientFactory factory;

        public DictionaryManager(IApiClientFactory factory)
        {
            this.factory = factory ?? throw new ArgumentNullException();
        }

        public async Task<ServiceResponse<TranslationResult>> Translate(TranslationRequest request, CancellationToken token)
        {
            Guard.NotNull(() => request, request);
            Guard.IsValid(() => request, request, translationRequest => !string.IsNullOrWhiteSpace(translationRequest.Word), "Word is not specified");
            Guard.IsValid(() => request, request, translationRequest => request.From == Language.English || request.To == Language.English, "Only from/to English is supported");
            Guard.IsValid(() => request, request, translationRequest => request.From != request.To , "From and To can't match");
            var client = factory.GetPostClient("Api/Dictionary/Translate");
            var result = await client.Request<TranslationRequest, TranslationResult>(request, token).ConfigureAwait(false);
            return result;
        }
    }
}
