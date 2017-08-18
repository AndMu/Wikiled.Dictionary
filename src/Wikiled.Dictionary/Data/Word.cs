using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Wikiled.Dictionary.Data
{
    public class Word
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public Language Language { get; set; }

        public string Text { get; set; }
    }
}
