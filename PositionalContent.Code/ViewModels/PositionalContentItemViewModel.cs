using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Umbraco.Core.Models.PublishedContent;

namespace Hifi.PositionalContent
{
    public class PositionalContentItemViewModel
    {
        public IList<IPublishedElement> Content { get; set; }
        public IList<IPublishedElement> Settings { get; set; }

        public bool IsPreview { get; set; }
        public string PreviewBreakpoint { get; set; }
        public string ModifierClass { get; set; }
    }
}

