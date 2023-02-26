using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Web.Common.ApplicationBuilder;

namespace Hifi.PositionalContent
{ 
    
    public class PositionalContentRouting : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            /*builder.Services.Configure<UmbracoPipelineOptions>(options =>
            {
                options.AddFilter(new UmbracoPipelineFilter(nameof(PositionalContentController))
                {
                    Endpoints = app => app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllerRoute(
                            "PositionalContent Preview",
                            "umbraco/backoffice/positionalcontent/positionalcontentpreview/{action}",
                            new {Controller = "PositionalContentPreview", Action = "Index"});
                    })
                });
            });*/
        }
    }
}
