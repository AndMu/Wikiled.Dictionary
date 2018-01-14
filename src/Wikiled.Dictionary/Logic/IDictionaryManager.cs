using System.Threading;
using System.Threading.Tasks;
using Wikiled.Core.Standard.Api.Client;
using Wikiled.Dictionary.Data;

namespace Wikiled.Dictionary.Logic
{
    public interface IDictionaryManager
    {
        Task<ServiceResponse<ServiceResult<TranslationResult>>> Translate(TranslationRequest request, CancellationToken token);
    }
}