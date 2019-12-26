using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using Umbraco.Core.Composing;

namespace Hifi.PositionalContent
{ 
    public class PositionalContentRouting : IComponent
    {
        public void Initialize()
        {
            RouteTable.Routes.MapRoute(
                "positionalcontent_preview",
                "umbraco/backoffice/positionalcontent/positionalcontentpreview/{action}",
                new
                {
                    controller = "PositionalContentPreview",
                }
            );
        }

        public void Terminate()
        { }
    }
}
