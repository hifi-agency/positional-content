using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Hifi.PositionalContent
{
    public class PositionalContentModel : PositionalContentModelBase
    {
        public PositionalContentModel()
        {
            Items = new List<PositionalContentItem>();
            Breakpoints = new Dictionary<string, PositionalContentBreakpoint>();
        }

        [JsonProperty("primaryImage")]
        public PositionalContentPrimaryImage PrimaryImage { get; set; }

        [JsonProperty("items")]
        public List<PositionalContentItem> Items { get; set; }

        [JsonProperty("breakpoints")]
        public Dictionary<string, PositionalContentBreakpoint> Breakpoints { get; set; }

        [JsonProperty("dtdGuid")]
        public Guid DtdGuid { get; set; }

        [JsonProperty("cssNamespace")]
        public string CssNamespace { get; set; }

        [JsonProperty("editorZoom")]
        public int EditorZoom { get; set; }

        public T Content<T>(Guid dtdGuid)
        {
            return Content<T>(dtdGuid, PositionalContentDataTypes.ImageContent);
        }

        public T Settings<T>(Guid dtdGuid)
        {
            return Settings<T>(dtdGuid, PositionalContentDataTypes.ImageSettings);
        }

    }

}

