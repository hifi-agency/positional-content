using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;

namespace Hifi.PositionalContent
{
    public class PositionalContentContentService
    {
        private readonly IUmbracoContextAccessor _umbracoContextAccessor;
        private readonly PropertyValueConverterCollection propertyValueConverters;
        private readonly IPublishedModelFactory publishedModelFactory;
        private readonly PositionalContentChildDataTypeService childDataTypeService;
        private readonly PropertyEditorCollection propertyEditors;
        private readonly IPublishedContentTypeFactory _publishedContentTypeFactory;

        public PositionalContentContentService(IUmbracoContextAccessor umbracoContextAccessor, PropertyValueConverterCollection propertyValueConverters, 
            IPublishedModelFactory publishedModelFactory, PositionalContentChildDataTypeService childDataTypeService,
            PropertyEditorCollection propertyEditors, IPublishedContentTypeFactory publishedContentTypeFactory )
        {
            _umbracoContextAccessor = umbracoContextAccessor;
            this.propertyValueConverters = propertyValueConverters;
            this.publishedModelFactory = publishedModelFactory;
            this.childDataTypeService = childDataTypeService;
            this.propertyEditors = propertyEditors;
            _publishedContentTypeFactory = publishedContentTypeFactory;
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

                var inPreviewMode = false;
                if(_umbracoContextAccessor.TryGetUmbracoContext(out var umbracoContext))
                    inPreviewMode = umbracoContext.InPreviewMode;

                var source = properyType.ConvertSourceToInter(null, value, inPreviewMode);

                // Try convert data to source
                // We try this first as the value is stored as JSON not
                // as XML as would occur in the XML cache as in the act
                // of concerting to XML this would ordinarily get called
                // but with JSON it doesn't, so we try this first
                var converted1 = properyType.ConvertInterToObject(null, PropertyCacheLevel.Element, source, inPreviewMode);
                if (converted1 is T) return (T)converted1;

                // If List IPublishedElements, try to convert to that
                // as List of ModelsBuilder types won't automatically convert to List of IPublishedElements
                if (converted1 is IEnumerable && typeof(T) == typeof(List<IPublishedElement>))
                {
                    var collection = converted1 as IEnumerable;
                    object listOfElements = collection.Cast<IPublishedElement>().ToList();
                    if (listOfElements is T) return (T)listOfElements;
                }

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
            return new PublishedPropertyType(editorAlias, dataTypeId, false, ContentVariation.Nothing, propertyValueConverters, publishedModelFactory, _publishedContentTypeFactory);
        }

    }
    
}
