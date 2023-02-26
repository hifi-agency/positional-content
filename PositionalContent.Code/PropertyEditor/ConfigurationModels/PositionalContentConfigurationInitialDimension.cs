using Newtonsoft.Json;

namespace Hifi.PositionalContent
{
    public class PositionalContentConfigurationInitialDimension
    {
        [JsonProperty("top")]
        public int Top { get; set; }

        [JsonProperty("left")]
        public int Left { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("heightAuto")]
        public bool HeightAuto { get; set; }
    }
}
