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
    [PropertyValueType(typeof(PositionalContentModel))]
    [PropertyValueCache(PropertyCacheValue.All, PropertyCacheLevel.Content)]
    public class PositionalContentPropertyConvertor : PropertyValueConverterBase
    {
        public override object ConvertDataToSource(PublishedPropertyType propertyType, object source, bool preview)
        {
            if(source != null)
            {
                try
                {
                    return JsonConvert.DeserializeObject<PositionalContentModel>(source.ToString());
                }
                catch { }  
            }
            return source;
        }

        public override bool IsConverter(PublishedPropertyType propertyType)
        {
            return propertyType.PropertyEditorAlias.Equals("HiFi.PositionalContent");
        }
    }
}
