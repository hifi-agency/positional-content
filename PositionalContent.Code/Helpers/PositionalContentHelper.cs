using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Models;
using Newtonsoft.Json;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web;

namespace Hifi.PositionalContent
{
    public static class PositionalContentHelper
    {
        internal static ChildDataType GetDataTypeToDb(Guid dtdGuid, string property, bool from = false)
        {
            var dtd = GetTargetDataTypeDefinition(dtdGuid, property);
            if(dtd != null)
            {
                var propEditor = PropertyEditorResolver.Current.GetByAlias(dtd.PropertyEditorAlias);
                var propType = new PropertyType(dtd);

                return new ChildDataType() { DataTypeDefinition = dtd, PropertyEditor = propEditor, PropertyType = propType };
            }
            return null;
        }

        internal static ChildDataType GetDataTypeFromDb(Guid dtdGuid, string property, bool from = false)
        {
            var dtd = PositionalContentHelper.GetTargetDataTypeDefinition(dtdGuid, property);
            var preValues = ApplicationContext.Current.Services.DataTypeService.GetPreValuesCollectionByDataTypeId(dtd.Id);
            var propEditor = PropertyEditorResolver.Current.GetByAlias(dtd.PropertyEditorAlias);

            return new ChildDataType() { DataTypeDefinition = dtd, PropertyEditor = propEditor, PreValues = preValues };
        }

        internal static T ContentValue<T>(Guid dtdGuid, object value, PositionalContentDataTypes type)
        {
            if(value != null)
            {
                // Get target datatype
                IDataTypeDefinition targetDataType = null;

                if (type == PositionalContentDataTypes.ImageContent)
                    targetDataType = GetTargetDataTypeDefinition(dtdGuid, Constants.ImageContentDataType);
                else if (type == PositionalContentDataTypes.ImageSettings)
                    targetDataType = GetTargetDataTypeDefinition(dtdGuid, Constants.ImageSettingsDataType);
                else if (type == PositionalContentDataTypes.ItemContent)
                    targetDataType = GetTargetDataTypeDefinition(dtdGuid, Constants.ItemContentDataType);
                else if (type == PositionalContentDataTypes.ItemSettings)
                    targetDataType = GetTargetDataTypeDefinition(dtdGuid, Constants.ItemSettingsDataType);

                // Umbraco has the concept of a IPropertyEditorValueConverter which it 
                // also queries for property resolvers. However I'm not sure what these
                // are for, nor can I find any implementations in core, so am currently
                // just ignoring these when looking up converters.
                // NB: IPropertyEditorValueConverter not to be confused with
                // IPropertyValueConverter which are the ones most people are creating
                var properyType = PositionalContentHelper.CreateDummyPropertyType(
                    targetDataType.Id,
                    targetDataType.PropertyEditorAlias);

                var inPreviewMode = UmbracoContext.Current.InPreviewMode;

                // Try convert data to source
                // We try this first as the value is stored as JSON not
                // as XML as would occur in the XML cache as in the act
                // of concerting to XML this would ordinarily get called
                // but with JSON it doesn't, so we try this first
                var converted1 = properyType.ConvertDataToSource(value, inPreviewMode);
                if (converted1 is T) return (T)converted1;

                var convertAttempt = converted1.TryConvertTo<T>();
                if (convertAttempt.Success) return convertAttempt.Result;

                // Try convert source to object
                // If the source value isn't right, try converting to object
                var converted2 = properyType.ConvertSourceToObject(converted1, inPreviewMode);
                if (converted2 is T) return (T)converted2;

                convertAttempt = converted2.TryConvertTo<T>();
                if (convertAttempt.Success) return convertAttempt.Result;

                // Try just converting
                convertAttempt = value.TryConvertTo<T>();
                if (convertAttempt.Success) return convertAttempt.Result;
            }


            // Still not right type so return default value
            return default(T);
        }

        internal static IDataTypeDefinition GetTargetDataTypeDefinition(Guid myId, string property)
        {
            return (IDataTypeDefinition)ApplicationContext.Current.ApplicationCache.RuntimeCache.GetCacheItem(
                Constants.CacheKey_GetTargetDataTypeDefinition + myId + property,
                () =>
                {
                    // Get instance of our own datatype so we can lookup the actual datatype from prevalue
                    var services = ApplicationContext.Current.Services;
                    var dtd = services.DataTypeService.GetDataTypeDefinitionById(myId);
                    var preValues = services.DataTypeService.GetPreValuesCollectionByDataTypeId(dtd.Id).PreValuesAsDictionary;
                    DataTypeInfo dataType = null;
                    if (preValues[property].Value == null || preValues[property].Value == "None")
                        return null;
                    else
                        dataType = JsonConvert.DeserializeObject<DataTypeInfo>(preValues[property].Value);

                    // Grab an instance of the target datatype
                    return services.DataTypeService.GetDataTypeDefinitionById(dataType.Guid);
                });
        }

        internal static PublishedPropertyType CreateDummyPropertyType(int dataTypeId, string propertyEditorAlias)
        {
            return new PublishedPropertyType(null,
                new PropertyType(new DataTypeDefinition(-1, propertyEditorAlias)
                {
                    Id = dataTypeId
                }));
        }
    }

    internal static class Constants
    {
        public const string CacheKey_GetTargetDataTypeDefinition = "PositionalContent_GetTargetDataTypeDefinition_";
        public const string ImageContentDataType = "imageContentDataType";
        public const string ImageSettingsDataType = "imageSettingsDataType";
        public const string ItemContentDataType = "itemContentDataType";
        public const string ItemSettingsDataType = "itemSettingsDataType";
    }

    public class ChildDataType
    {
        public IDataTypeDefinition DataTypeDefinition { get; set; }
        public PropertyEditor PropertyEditor { get; set; }
        public PropertyType PropertyType { get; set; }
        public PreValueCollection PreValues { get; set; }
    }

    public enum PositionalContentDataTypes
    {
        ImageContent,
        ImageSettings, 
        ItemContent,
        ItemSettings
    }

    internal class DataTypeInfo
    {
        [JsonProperty("guid")]
        public Guid Guid { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("propertyEditorAlias")]
        public string PropertyEditorAlias { get; set; }
    }
}
