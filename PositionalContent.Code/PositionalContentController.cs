using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Common.Attributes;
using Umbraco.Cms.Web.Website.Controllers;

namespace Hifi.PositionalContent
{
    public class PositionalContentRenderItemViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(PositionalContentModel data, PositionalContentItem item)
        {
            var model = new PositionalContentItemViewModel() { Content = item.GetContents(data), Settings = item.GetSettings(data) };
            return View("/views/Partials/PositionalContent/Base.cshtml", model);
        }
    }
    
    public class PositionalContentRenderDimensionViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(PositionalContentModel data, PositionalContentItem item, PositionalContentItemDimension dimension)
        {
            var model = new PositionalContentItemViewModel() { Content = item.GetContents(data.DtdGuid, dimension), Settings = item.GetSettings(data.DtdGuid, dimension) };
            return View("/views/Partials/PositionalContent/Base.cshtml", model);
        }
    }
}
