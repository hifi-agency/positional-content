using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PositionalContent.Code;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Extensions;

namespace Hifi.PositionalContent
{

    public class PositionalContentServerVariableParser : INotificationHandler<ServerVariablesParsingNotification>
    {

        private readonly LinkGenerator _linkGenerator;
        private readonly UmbracoApiControllerTypeCollection _umbracoApiControllerTypeCollection;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PositionalContentServerVariableParser(LinkGenerator linkGenerator,UmbracoApiControllerTypeCollection umbracoApiControllerTypeCollection, IHttpContextAccessor httpContextAccessor)
        {
            _linkGenerator = linkGenerator;
            _umbracoApiControllerTypeCollection = umbracoApiControllerTypeCollection;
            _httpContextAccessor = httpContextAccessor;
        }
        public void Handle(ServerVariablesParsingNotification notification)
        {
            if (_httpContextAccessor.HttpContext == null) return;


            var mainDictionary = new Dictionary<string, object>
            {
                {
                    "apiBaseUrl",
                    _linkGenerator.GetUmbracoApiServiceBaseUrl<PositionalContentBackofficeController>(c => c.GetBlockListDataTypes())
                }
            };

            if (!notification.ServerVariables.Keys.Contains("positionalcontent"))
            {
                notification.ServerVariables.Add("positionalcontent", mainDictionary);
            }
        }
    }
}
