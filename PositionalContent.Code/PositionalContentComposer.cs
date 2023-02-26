using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PositionalContent.Code.Composers;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.WebAssets;
using Umbraco.Cms.Web.Common.ApplicationBuilder;

namespace Hifi.PositionalContent
{
    public class PositionalContentComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.Services.AddSingleton<PositionalContentChildDataTypeService>();
            builder.Services.AddTransient<PositionalContentPreviewController>();
            builder.Services.AddTransient<PositionalContentContentService>();
            
            builder.Services.Configure<UmbracoPipelineOptions>(options =>
            {
                options.AddFilter(new UmbracoPipelineFilter(nameof(PositionalContentPreviewController))
                {
                    Endpoints = app => app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllerRoute(
                            "positional-content-preview-controller",
                            "/umbraco/backoffice/positionalcontent/positionalcontentpreview/{action}",
                            new {Controller = "PositionalContentPreview", Action = "GetPartialViewResultAsHtmlForEditor"});
                    })
                });
            });

            builder.WithCollectionBuilder<PositionalContentAssets>();
            PositionalContentContentServiceProvider.UmbracoBuilder = builder;
        }
    }
    
    
    public class PositionalContentAssets : CustomBackOfficeAssetsCollectionBuilder
{
    protected override IEnumerable<IAssetFile> CreateItems(IServiceProvider factory)
    {
        var files = base.CreateItems(factory).ToList();
        //JS
        files.Add(new JavaScriptFile("../App_Plugins/PositionalContent/breakpoint/breakpoint.service.js"));
        files.Add(new JavaScriptFile("../App_Plugins/PositionalContent/breakpoint/breakpoint.directive.js"));
        files.Add(
            new JavaScriptFile("../App_Plugins/PositionalContent/breakpointControls/breakpointControls.service.js"));
        files.Add(new JavaScriptFile(
            "../App_Plugins/PositionalContent/breakpointControls/breakpointControls.directive.js"));
        files.Add(new JavaScriptFile("../App_Plugins/PositionalContent/cropper/cropper.service.js"));
        files.Add(new JavaScriptFile("../App_Plugins/PositionalContent/cropper/cropper.directive.js"));
        files.Add(new JavaScriptFile(
            "../App_Plugins/PositionalContent/contentEditor/positionalcontenteditor.controller.js"));
        files.Add(new JavaScriptFile("../App_Plugins/PositionalContent/directives/imageOnLoad.directive.js"));
        files.Add(new JavaScriptFile("../App_Plugins/PositionalContent/directives/move.directive.js"));
        files.Add(new JavaScriptFile("../App_Plugins/PositionalContent/directives/resize.directive.js"));
        files.Add(new JavaScriptFile("../App_Plugins/PositionalContent/directives/selectOnClick.directive.js"));
        files.Add(new JavaScriptFile("../App_Plugins/PositionalContent/globalControls/globalControls.directive.js"));
        files.Add(new JavaScriptFile("../App_Plugins/PositionalContent/globalControls/globalControls.service.js"));
        files.Add(new JavaScriptFile("../App_Plugins/PositionalContent/item/item.service.js"));
        files.Add(new JavaScriptFile("../App_Plugins/PositionalContent/item/item.directive.js"));
        files.Add(new JavaScriptFile(
            "../App_Plugins/PositionalContent/propertyEditor/propertyEditorBreakpoint.controller.js"));
        files.Add(new JavaScriptFile(
            "../App_Plugins/PositionalContent/propertyEditor/propertyEditorPicker.controller.js"));
        files.Add(new JavaScriptFile(
            "../App_Plugins/PositionalContent/propertyEditor/propertyEditorInitialDimensions.controller.js"));
        files.Add(new JavaScriptFile("../App_Plugins/PositionalContent/resources/resources.js"));
        files.Add(new JavaScriptFile("../App_Plugins/PositionalContent/services/image.service.js"));
        files.Add(new JavaScriptFile("../App_Plugins/PositionalContent/services/premium.service.js"));
        files.Add(new JavaScriptFile("../App_Plugins/PositionalContent/services/scale.service.js"));
        files.Add(new JavaScriptFile("../App_Plugins/PositionalContent/services/util.service.js"));
        files.Add(new JavaScriptFile("../App_Plugins/PositionalContent/positionalcontent.controller.js"));
        //CSS
        files.Add(new CssFile("../App_Plugins/PositionalContent/positionalcontent-backoffice.css"));
        return files;
    }
}
}
