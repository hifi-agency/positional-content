using System;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.DependencyInjection;

public static class PositionalContentContentServiceProvider
{
    public static IUmbracoBuilder UmbracoBuilder { get; set; }

    public static IServiceProvider ServiceProvider
    {
        get
        {
           return UmbracoBuilder.Services.BuildServiceProvider();
        }
    }
}