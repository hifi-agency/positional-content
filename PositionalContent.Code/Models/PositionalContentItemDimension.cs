using System;
using Newtonsoft.Json;

namespace Hifi.PositionalContent
{
    public class PositionalContentItemDimension : PositionalContentModelBase
    {

        [JsonProperty("top")]
        public float Top { get; set; }

        [JsonProperty("bottom")]
        public float Bottom { get; set; }

        [JsonProperty("left")]
        public float Left { get; set; }

        [JsonProperty("right")]
        public float Right { get; set; }

        [JsonProperty("width")]
        public float Width { get; set; }

        [JsonProperty("widthAuto")]
        public bool WidthAuto { get; set; }

        [JsonProperty("height")]
        public float Height { get; set; }

        [JsonProperty("heightAuto")]
        public bool HeightAuto { get; set; }

        [JsonProperty("pinToRight")]
        public bool PinToRight { get; set; }

        [JsonProperty("pinToBottom")]
        public bool PinToBottom { get; set; }

        [JsonProperty("hide")]
        public bool Hide { get; set; }

        [JsonProperty("overrideContent")]
        public bool OverrideContent { get; set; }

        [JsonProperty("overrideSettings")]
        public bool OverrideSettings { get; set; }

        public T Content<T>(Guid dtdGuid)
        {
            return Content<T>(dtdGuid, PositionalContentDataTypes.ItemContent);
        }

        public T Settings<T>(Guid dtdGuid)
        {
            return Settings<T>(dtdGuid, PositionalContentDataTypes.ItemSettings);
        }
    }
}

