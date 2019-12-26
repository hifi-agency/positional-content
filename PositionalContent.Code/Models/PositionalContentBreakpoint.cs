using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Umbraco.Web;

using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;

namespace Hifi.PositionalContent
{
    public class PositionalContentBreakpoint : PositionalContentModelBase
    {

        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("imageId")]
        public string ImageId { get; set; }

        [JsonProperty("breakpointUpper")]
        public int? BreakpointUpper { get; set; }
        [JsonProperty("breakpointLower")]
        public int? BreakpointLower { get; set; }

        [JsonProperty("cropWidth")]
        public int CropWidth { get; set; }
        [JsonProperty("cropHeight")]
        public int CropHeight { get; set; }

        [JsonProperty("height")]
        public float Height { get; set; }
        [JsonProperty("width")]
        public float Width { get; set; }

        /*Crop Percentages*/
        [JsonProperty("left")]
        public float Left { get; set; }
        [JsonProperty("right")]
        public float Right { get; set; }
        [JsonProperty("top")]
        public float Top { get; set; }
        [JsonProperty("bottom")]
        public float Bottom { get; set; }

        [JsonProperty("scale")]
        public float Scale { get; set; }

        [JsonProperty("overrideContent")]
        public bool OverrideContent { get; set; }

        [JsonProperty("overrideSettings")]
        public bool OverrideSettings { get; set; }

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

