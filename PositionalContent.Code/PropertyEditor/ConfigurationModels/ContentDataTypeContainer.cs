using System;
using Newtonsoft.Json;

namespace Hifi.PositionalContent
{
    public class ContentDataTypeContainer
    {
        [JsonProperty("guid")]
        public Guid Guid { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("propertyEditorAlias")]
        public string PropertyEditorAlias { get; set; }
    }
}
