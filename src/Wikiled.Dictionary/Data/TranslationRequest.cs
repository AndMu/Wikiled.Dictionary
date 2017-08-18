using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Wikiled.Dictionary.Data
{
    public class TranslationRequest
    {
        public Word Source { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Language TargetLanguage { get; }
    }
}
