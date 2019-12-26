using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Umbraco.Web;

using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;

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

