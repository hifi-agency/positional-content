using System;
using Newtonsoft.Json;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace Hifi.PositionalContent
{
    public class PositionalContentPropertyConvertor : PropertyValueConverterBase, IPropertyValueConverter
    {
        public override bool IsConverter(IPublishedPropertyType propertyType)
            => propertyType.EditorAlias == "HiFi.PositionalContent";

        public override Type GetPropertyValueType(IPublishedPropertyType propertyType)
            => typeof(PositionalContentModel);

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
