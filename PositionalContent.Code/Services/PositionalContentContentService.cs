using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.Composing;

namespace Hifi.PositionalContent
{
    public class PositionalContentContentService
    {
        private readonly PropertyValueConverterCollection propertyValueConverters;
        private readonly IPublishedModelFactory publishedModelFactory;
        private readonly PositionalContentChildDataTypeService childDataTypeService;
        private readonly PropertyEditorCollection propertyEditors;

        public PositionalContentContentService(PropertyValueConverterCollection propertyValueConverters, 
            IPublishedModelFactory publishedModelFactory, PositionalContentChildDataTypeService childDataTypeService,
            PropertyEditorCollection propertyEditors)
        {
            this.propertyValueConverters = propertyValueConverters;
            this.publishedModelFactory = publishedModelFactory;
            this.childDataTypeService = childDataTypeService;
            this.propertyEditors = propertyEditors;
        }

        public T Value<T>(Guid dtdGuid, object value, PositionalContentDataTypes type)
        {
            if(value != null)
            {
                // Get target datatype
                IDataType targetDataType = null;

                if (type == PositionalContentDataTypes.ImageContent)
                    targetDataType = childDataTypeService.Get(dtdGuid, Constants.ImageContentDataType);
                else if (type == PositionalContentDataTypes.ImageSettings)
                    targetDataType = childDataTypeService.Get(dtdGuid, Constants.ImageSettingsDataType);
                else if (type == PositionalContentDataTypes.ItemContent)
                    targetDataType = childDataTypeService.Get(dtdGuid, Constants.ItemContentDataType);
                else if (type == PositionalContentDataTypes.ItemSettings)
                    targetDataType = childDataTypeService.Get(dtdGuid, Constants.ItemSettingsDataType);


                // Umbraco has the concept of a IPropertyEditorValueConverter which it 
                // also queries for property resolvers. However I'm not sure what these
                // are for, nor can I find any implementations in core, so am currently
                // just ignoring these when looking up converters.
                // NB: IPropertyEditorValueConverter not to be confused with
                // IPropertyValueConverter which are the ones most people are creating
                var properyType = CreateDummyPropertyType(
                    targetDataType.Id,
                    targetDataType.EditorAlias);

                var inPreviewMode = Current.UmbracoContext.InPreviewMode;

                var source = properyType.ConvertSourceToInter(null, value, inPreviewMode);

                // Try convert data to source
                // We try this first as the value is stored as JSON not
                // as XML as would occur in the XML cache as in the act
                // of concerting to XML this would ordinarily get called
                // but with JSON it doesn't, so we try this first
                var converted1 = properyType.ConvertInterToObject(null, PropertyCacheLevel.Element, source, inPreviewMode);
                if (converted1 is T) return (T)converted1;

                var convertAttempt = converted1.TryConvertTo<T>();
                if (convertAttempt.Success) return convertAttempt.Result;

                // Try convert source to object
                // If the source value isn't right, try converting to object
                var converted2 = properyType.ConvertSourceToInter(null, converted1, inPreviewMode);
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

        public PublishedPropertyType CreateDummyPropertyType(int dataTypeId, string editorAlias)
        {
            return new PublishedPropertyType(editorAlias, dataTypeId, false, ContentVariation.Nothing, propertyValueConverters, publishedModelFactory, Current.PublishedContentTypeFactory);
        }

    }
    
}
