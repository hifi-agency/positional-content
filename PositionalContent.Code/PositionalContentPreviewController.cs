using System;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.Common.Controllers;

namespace Hifi.PositionalContent
{
    public class PositionalContentPreviewController : UmbracoController
    {
        [HttpPost]
        public ActionResult GetPartialViewResultAsHtmlForEditor()
        {
            var itemStr = Request.Form["item"];
            var breakpointName = Request.Form["breakpointName"];
            var previewModifierClass = Request.Form["previewModifierClass"];
            var dtdGuid = Guid.Parse(Request.Form["dtdGuid"]);
            var view = Request.Form["view"];

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
