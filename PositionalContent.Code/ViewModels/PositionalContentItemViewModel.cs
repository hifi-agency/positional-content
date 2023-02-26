using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;

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

