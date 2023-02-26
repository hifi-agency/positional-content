using System;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

namespace Hifi.PositionalContent
{
    public class PositionalContentChildDataTypeService
    {
        private readonly IDataTypeService dataTypeService;
        private readonly PropertyEditorCollection _propertyEditorCollection;
        private readonly AppCaches _appCaches;

        public PositionalContentChildDataTypeService(IDataTypeService dataTypeService, PropertyEditorCollection propertyEditorCollection, AppCaches appCaches)
        {
            this.dataTypeService = dataTypeService;
            _propertyEditorCollection = propertyEditorCollection;
            _appCaches = appCaches;
        }

        public IDataType Get(Guid myId, string property)
        {
            return (IDataType)_appCaches.RuntimeCache.Get(
                Constants.CacheKey_GetTargetDataTypeDefinition + myId + property,
                () =>
                {
                    // Get instance of our own datatype so we can lookup the actual datatype from prevalue
                    var dtd = dataTypeService.GetDataType(myId);

                    var propEditor = _propertyEditorCollection[dtd.EditorAlias];
                    var config = propEditor.GetConfigurationEditor().ToValueEditor(dtd.Configuration);

                    var childDataType = ((ContentDataTypeContainer)config[property]);

                    if (childDataType == null || childDataType.Guid == Guid.Empty)
                        return null;

                    // Grab an instance of the target datatype
                    return dataTypeService.GetDataType(childDataType.Guid);

                });
        }
    }
    
}
