using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web.Mvc;
using umbraco;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using Newtonsoft.Json;

namespace Hifi.PositionalContent
{
    [PluginController("PositionalContent")]
    public class PositionalContentPreviewController : UmbracoAuthorizedController
    {
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult GetPartialViewResultAsHtmlForEditor()
        {
            var itemStr = Request["item"];
            var breakpointName = Request["breakpointName"];
            var previewModifierClass = Request["previewModifierClass"];
            var dtdGuid = Guid.Parse(Request["dtdGuid"]);
            var view = Request["view"];

            var item = JsonConvert.DeserializeObject<PositionalContentItem>(itemStr);
            var dimension = item.Dimensions[breakpointName];
            var model = new PositionalContentItemViewModel() {
                Content = item.GetContents(dtdGuid, dimension),
                Settings = item.GetSettings(dtdGuid, dimension),
                IsPreview = true,
                PreviewBreakpoint = breakpointName,
                ModifierClass = previewModifierClass
            };

            return View("/views/Partials/" + view + ".cshtml", model);
        }
    }
}
