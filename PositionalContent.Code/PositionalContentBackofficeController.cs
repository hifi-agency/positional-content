using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;

namespace Hifi.PositionalContent
{
    [PluginController("PositionalContentApi")]
    public class PositionalContentBackofficeController : UmbracoAuthorizedApiController 
    {
        private readonly PropertyEditorCollection propertyEditors;
        private readonly IDataTypeService _dataTypeService;
        private readonly IMemberTypeService _memberTypeService;
        private readonly IContentTypeService _contentTypeService;

        public PositionalContentBackofficeController(PropertyEditorCollection propertyEditors, IDataTypeService dataTypeService, IMemberTypeService memberTypeService, IContentTypeService contentTypeService)
        {
            this.propertyEditors = propertyEditors;
            _dataTypeService = dataTypeService;
            _memberTypeService = memberTypeService;
            _contentTypeService = contentTypeService;
        }

        public IEnumerable<object> GetBlockListDataTypes()
        {
            var r = _dataTypeService.GetAll()
                //TODO change nestedContent to blocklist
                .Where(x =>  x.EditorAlias == "Umbraco.BlockList")
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
                var dtd = _dataTypeService.GetDataType(id);
                return FormatDataType(dtd);
            }
            return null;
        }

        public object GetDataTypeByAlias(string contentType, string contentTypeAlias, string propertyAlias)
        {
            IContentTypeComposition ct = null;

            var r = _dataTypeService.GetAll()
                .Where(x => x.EditorAlias == "HiFi.PositionalContent").ToList();

            switch (contentType)
            {
                case "member":
                    ct = _memberTypeService.Get(contentTypeAlias);
                    break;
                case "content":
                    ct = _contentTypeService.Get(contentTypeAlias);
                    break;
                case "media":
                    ct = _contentTypeService.Get(contentTypeAlias);
                    break;
            }

            if (ct == null)
                return null;

            var prop = ct.CompositionPropertyTypes.SingleOrDefault(x => x.Alias == propertyAlias);
            if (prop == null)
                return null;

            var dtd = _dataTypeService.GetDataType(prop.DataTypeId);
            return FormatDataType(dtd);
        }

        protected object FormatDataType(IDataType dataType)
        {
            if (dataType == null)
                throw new HttpRequestException("", null, HttpStatusCode.NotFound);

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
