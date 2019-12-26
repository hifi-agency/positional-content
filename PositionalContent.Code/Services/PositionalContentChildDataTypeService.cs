using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.Composing;
using Umbraco.Core.Services;

namespace Hifi.PositionalContent
{
    public class PositionalContentChildDataTypeService
    {
        private readonly IDataTypeService dataTypeService;

        public PositionalContentChildDataTypeService(IDataTypeService dataTypeService)
        {
            this.dataTypeService = dataTypeService;
        }

        public IDataType Get(Guid myId, string property)
        {
            return (IDataType)Current.AppCaches.RuntimeCache.Get(
                Constants.CacheKey_GetTargetDataTypeDefinition + myId + property,
                () =>
                {
                    // Get instance of our own datatype so we can lookup the actual datatype from prevalue
                    var dtd = dataTypeService.GetDataType(myId);

                    var propEditor = Current.PropertyEditors[dtd.EditorAlias];
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
