using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Persistence;
using Umbraco.Web.UI.JavaScript;
using System.Web.Routing;
using System.Web.Mvc;
using Umbraco.Web;


namespace Hifi.PositionalContent
{
    public class PositionalContentRouting : ApplicationEventHandler
    {
        protected override void ApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            base.ApplicationInitialized(umbracoApplication, applicationContext);

            RouteTable.Routes.MapRoute(
                "positionalcontent_preview",
                "umbraco/backoffice/positionalcontent/positionalcontentpreview/{action}",
                new
                {
                    controller = "PositionalContentPreview",
                }
            );
        }
    }
}
