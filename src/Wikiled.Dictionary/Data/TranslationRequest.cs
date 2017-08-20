using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Wikiled.Dictionary.Data
{
    public class TranslationRequest
    {
        public string Word { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Language From { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Language To { get; set; }
    }
}
