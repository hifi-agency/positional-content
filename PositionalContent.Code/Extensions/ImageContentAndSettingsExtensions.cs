using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Umbraco.Web;

using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;

namespace Hifi.PositionalContent
{

    public static class ImageContentAndSettingsExtensions
    {

        //Image Content
        public static IList<IPublishedElement> GetContents(this PositionalContentModel model, PositionalContentBreakpoint breakpoint = null)
        {
            if(breakpoint != null && breakpoint.OverrideContent && breakpoint.HasContent)
                return breakpoint.GetContents(model);
            else
            {
                var contents = model.Content<List<IPublishedElement>>(model.DtdGuid);
                if (contents == null)
                    return new List<IPublishedElement>() { model.Content<IPublishedElement>(model.DtdGuid) }.Where(x => x != null).ToList();
                else
                    return contents;
            }
        }

        public static IPublishedElement GetContent(this PositionalContentModel model, PositionalContentBreakpoint breakpoint = null)
        {
            return model.GetContents(breakpoint).FirstOrDefault();
        }

        public static E GetContent<E>(this PositionalContentModel model, PositionalContentBreakpoint breakpoint = null)
            where E : PublishedElementModel
        {
            var content = model.GetContent(breakpoint);
            return content.ToModel<E>();
        }

        public static IEnumerable<E> GetContents<E>(this PositionalContentModel model, PositionalContentBreakpoint breakpoint = null)
        where E : PublishedElementModel
        {
            var content = model.GetContents(breakpoint);
            return content.Select(x => x.ToModel<E>());
        }

        //Image Settings
        public static IList<IPublishedElement> GetSettings(this PositionalContentModel model, PositionalContentBreakpoint breakpoint = null)
        {
            if (breakpoint != null && breakpoint.OverrideSettings && breakpoint.HasSettings)
                return breakpoint.GetSettings(model);
            else
            {
                var settings = model.Settings<List<IPublishedElement>>(model.DtdGuid);
                if (settings == null)
                    return new List<IPublishedElement>() { model.Settings<IPublishedElement>(model.DtdGuid) }.Where(x => x != null).ToList();
                else
                    return settings;
            }
        }

        public static IPublishedElement GetSetting(this PositionalContentModel model, PositionalContentBreakpoint breakpoint = null)
        {
            return model.GetSettings(breakpoint).FirstOrDefault();
        }

        public static IEnumerable<E> GetSettings<E>(this PositionalContentModel model, PositionalContentBreakpoint breakpoint = null)
             where E : PublishedElementModel
        {
            var content = model.GetSettings(breakpoint);
            return content.Select(x => x.ToModel<E>());
        }

        public static E GetSetting<E>(this PositionalContentModel model, PositionalContentBreakpoint breakpoint = null)
            where E : PublishedElementModel
        {
            var content = model.GetSetting(breakpoint);
            return content.ToModel<E>();
        }

    }

}

