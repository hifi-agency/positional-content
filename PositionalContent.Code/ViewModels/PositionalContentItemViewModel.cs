using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Umbraco.Core.Models;

namespace Hifi.PositionalContent
{
    public class PositionalContentItemViewModel
    {
        public IList<IPublishedContent> Content { get; set; }
        public IList<IPublishedContent> Settings { get; set; }

        public bool IsPreview { get; set; }
        public string PreviewBreakpoint { get; set; }
        public string ModifierClass { get; set; }
    }
}

