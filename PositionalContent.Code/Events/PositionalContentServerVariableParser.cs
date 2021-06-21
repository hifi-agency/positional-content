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
    public class PositionalContentServerVariableParser : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ServerVariablesParser.Parsing += ServerVariablesParser_Parsing;
        }

        void ServerVariablesParser_Parsing(object sender, Dictionary<string, object> e)
        {
            if (HttpContext.Current == null) return;
            var urlHelper = new UrlHelper(new RequestContext(new HttpContextWrapper(HttpContext.Current), new RouteData()));

            var mainDictionary = new Dictionary<string, object>
            {
                {
                    "apiBaseUrl",
                    urlHelper.GetUmbracoApiServiceBaseUrl<PositionalContentBackofficeController>(
                        controller => controller.GetNestedContentDataTypes())
                }
            };



            if (!e.Keys.Contains("positionalcontent"))
            {
                e.Add("positionalcontent", mainDictionary);
            }
        }
    }
}
