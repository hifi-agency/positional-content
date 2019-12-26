using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Core.Composing;
using Umbraco.Web.JavaScript;

namespace Hifi.PositionalContent
{
    public class PositionalContentServerVariableParser : IComponent
    {
        public void Initialize()
        {
            ServerVariablesParser.Parsing += (sender, e) =>
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
            };
        }

        public void Terminate()
        { }
    }
}
