using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.PropertyEditors;
using Newtonsoft.Json;

namespace Hifi.PositionalContent
{
    public class PositionalContentConfigurationBreakpoint
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("cropWidth")]
        public int CropWidth { get; set; }

        [JsonProperty("cropHeight")]
        public int CropHeight { get; set; }

        [JsonProperty("breakpointUpper")]
        public int breakpointUpper { get; set; }

        [JsonProperty("breakpointLower")]
        public int BreakpointLower { get; set; }
    }
}
