using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web.Http;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using Umbraco.Web.Composing;

namespace Hifi.PositionalContent
{
    [PluginController("PositionalContentApi")]
    public class PositionalContentBackofficeController : UmbracoAuthorizedJsonController
    {
        private readonly PropertyEditorCollection propertyEditors;

        public PositionalContentBackofficeController(PropertyEditorCollection propertyEditors)
        {
            this.propertyEditors = propertyEditors;
        }

        public IEnumerable<object> GetNestedContentDataTypes()
        {
            var r = Services.DataTypeService.GetAll()
                .Where(x => x.EditorAlias == "Our.Umbraco.NestedContent" || x.EditorAlias == "Umbraco.NestedContent")
                .OrderBy(x => x.Name)
                .Select(x => new
                {
                    guid = x.Key,
                    name = x.Name,
                    propertyEditorAlias = x.EditorAlias
                })
                .ToList();

            r.Insert(0, new
            {
                guid = new Guid(),
                name = "None",
                propertyEditorAlias = string.Empty
            });
            return r;
        }

        public object GetDataTypeById(Guid id)
        {
            if(id != new Guid())
            {
                var dtd = Services.DataTypeService.GetDataType(id);
                return FormatDataType(dtd);
            }
            return null;
        }

        public object GetDataTypeByAlias(string contentType, string contentTypeAlias, string propertyAlias)
        {
            IContentTypeComposition ct = null;

            var r = Services.DataTypeService.GetAll()
                .Where(x => x.EditorAlias == "HiFi.PositionalContent").ToList();

            switch (contentType)
            {
                case "member":
                    ct = Services.MemberTypeService.Get(contentTypeAlias);
                    break;
                case "content":
                    ct = Services.ContentTypeService.Get(contentTypeAlias);
                    break;
                case "media":
                    ct = Services.ContentTypeService.Get(contentTypeAlias);
                    break;
            }

            if (ct == null)
                return null;

            var prop = ct.CompositionPropertyTypes.SingleOrDefault(x => x.Alias == propertyAlias);
            if (prop == null)
                return null;

            var dtd = Services.DataTypeService.GetDataType(prop.DataTypeId);
            return FormatDataType(dtd);
        }

        protected object FormatDataType(IDataType dataType)
        {
            if (dataType == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            var propEditor = propertyEditors[dataType.EditorAlias];
            var config = propEditor.GetConfigurationEditor().ToValueEditor(dataType.Configuration);
            var valueEditor = propEditor.GetValueEditor();

            return new
            {
                guid = dataType.Key,
                propertyEditorAlias = dataType.EditorAlias,
                preValues = config,
                view = valueEditor.View
            };
        }
    }
}
