using Newtonsoft.Json;

namespace Hifi.PositionalContent
{
    public class PositionalContentPrimaryImage
    {

        [JsonProperty("imageId")]
        public string ImageId { get; set; }
        [JsonProperty("height")]
        public float Height { get; set; }
        [JsonProperty("width")]
        public float Width { get; set; }

    }
    
}

