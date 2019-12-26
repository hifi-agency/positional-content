using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Core.Models.PublishedContent;
using Newtonsoft.Json;

namespace Hifi.PositionalContent
{

    public class PositionalContentItem : PositionalContentModelBase
    {
        public PositionalContentItem()
        {
            Dimensions = new Dictionary<string, PositionalContentItemDimension>();
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("dimensions")]
        public Dictionary<string, PositionalContentItemDimension> Dimensions { get; set; }

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

