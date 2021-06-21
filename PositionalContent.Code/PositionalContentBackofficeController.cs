using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web.Http;
using umbraco;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace Hifi.PositionalContent
{
    [PluginController("PositionalContentApi")]
    public class PositionalContentBackofficeController : UmbracoAuthorizedJsonController
    {
        public IEnumerable<object> GetNestedContentDataTypes()
        {
            var r = Services.DataTypeService.GetAllDataTypeDefinitions()
                .Where(x => x.PropertyEditorAlias == "Our.Umbraco.NestedContent" || x.PropertyEditorAlias == "Umbraco.NestedContent")
                .OrderBy(x => x.Name)
                .Select(x => new
                {
                    guid = x.Key,
                    name = x.Name,
                    propertyEditorAlias = x.PropertyEditorAlias
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
                var dtd = Services.DataTypeService.GetDataTypeDefinitionById(id);
                return FormatDataType(dtd);
            }
            return null;
        }

        public object GetDataTypeByAlias(string contentType, string contentTypeAlias, string propertyAlias)
        {
            IContentTypeComposition ct = null;

            var r = Services.DataTypeService.GetAllDataTypeDefinitions()
                .Where(x => x.PropertyEditorAlias == "HiFi.PositionalContent").ToList();

            switch (contentType)
            {
                case "member":
                    ct = Services.MemberTypeService.Get(contentTypeAlias);
                    break;
                case "content":
                    ct = Services.ContentTypeService.GetContentType(contentTypeAlias);
                    break;
                case "media":
                    ct = Services.ContentTypeService.GetMediaType(contentTypeAlias);
                    break;
            }

            if (ct == null)
                return null;

            var prop = ct.CompositionPropertyTypes.SingleOrDefault(x => x.Alias == propertyAlias);
            if (prop == null)
                return null;

            var dtd = Services.DataTypeService.GetDataTypeDefinitionById(prop.DataTypeDefinitionId);
            return FormatDataType(dtd);
        }

        protected object FormatDataType(IDataTypeDefinition dtd)
        {
            if (dtd == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            var propEditor = PropertyEditorResolver.Current.GetByAlias(dtd.PropertyEditorAlias);

            // Force converter before passing prevalues to view
            var preValues = Services.DataTypeService.GetPreValuesCollectionByDataTypeId(dtd.Id);
            var convertedPreValues = propEditor.PreValueEditor.ConvertDbToEditor(propEditor.DefaultPreValues,
                preValues);

            return new
            {
                guid = dtd.Key,
                propertyEditorAlias = dtd.PropertyEditorAlias,
                preValues = convertedPreValues,
                view = propEditor.ValueEditor.View
            };
        }
    }
}
