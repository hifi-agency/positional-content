using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;
using Newtonsoft.Json;

namespace Hifi.PositionalContent
{
    public class PositionalContentPropertyConvertor : PropertyValueConverterBase
    {
        public override bool IsConverter(IPublishedPropertyType propertyType)
            => propertyType.EditorAlias == "HiFi.PositionalContent";

        public override Type GetPropertyValueType(IPublishedPropertyType propertyType)
            => typeof(string);

        public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType)
            => PropertyCacheLevel.Snapshot;

        public override object ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object source, bool preview)
        {
            if (source != null)
            {
                try
                {
                    return JsonConvert.DeserializeObject<PositionalContentModel>(source.ToString());
                }
                catch { }
            }
            return source;
        }
    }
}
