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
    public class PositionalContentController : SurfaceController
    {

        public ActionResult RenderItem(PositionalContentModel data, PositionalContentItem item)
        {
            var model = new PositionalContentItemViewModel() { Content = item.GetContents(data), Settings = item.GetSettings(data) };
            return View("/views/Partials/PositionalContent/Base.cshtml", model);
        }

        public ActionResult RenderDimension(PositionalContentModel data, PositionalContentItem item, PositionalContentItemDimension dimension)
        {
            var model = new PositionalContentItemViewModel() { Content = item.GetContents(data.DtdGuid, dimension), Settings = item.GetSettings(data.DtdGuid, dimension) };
            return View("/views/Partials/PositionalContent/Base.cshtml", model);
        }

    }
}
