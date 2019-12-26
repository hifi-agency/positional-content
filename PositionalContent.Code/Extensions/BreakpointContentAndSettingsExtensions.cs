using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Umbraco.Web;

using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;

namespace Hifi.PositionalContent
{

    public static class BreakpointContentAndSettingsExtensions
    {
        

        //Breakpoint Content
        public static IList<IPublishedElement> GetContents(this PositionalContentBreakpoint breakpoint, Guid dtdGuid)
        {
            var contents = breakpoint.Content<List<IPublishedElement>>(dtdGuid);
            if (contents == null)
                return new List<IPublishedElement>() { breakpoint.Content<IPublishedElement>(dtdGuid)}.Where(x => x != null).ToList();
            else
                return contents;
        }

        public static IPublishedElement GetContent(this PositionalContentBreakpoint breakpoint, Guid dtdGuid)
        {
            return breakpoint.GetContents(dtdGuid).FirstOrDefault();
        }

        public static IList<IPublishedElement> GetContents(this PositionalContentBreakpoint breakpoint, PositionalContentModel model)
        {
            return breakpoint.GetContents(model.DtdGuid);
        }

        public static IPublishedElement GetContent(this PositionalContentBreakpoint breakpoint, PositionalContentModel model)
        {
            return breakpoint.GetContent(model.DtdGuid);
        }

        public static IEnumerable<E> GetContents<E>(this PositionalContentBreakpoint breakpoint, PositionalContentModel model)
        where E : PublishedElementModel
        {
            var content = breakpoint.GetContents(model);
            return content.Select(x => x.ToModel<E>());
        }

        public static E GetContent<E>(this PositionalContentBreakpoint breakpoint, PositionalContentModel model)
            where E : PublishedElementModel
        {
            var content = breakpoint.GetContent(model);
            return content.ToModel<E>();
        }

        //Breakpoint Settings
        public static IList<IPublishedElement> GetSettings(this PositionalContentBreakpoint breakpoint, Guid dtdGuid)
        {
            var settings = breakpoint.Settings<List<IPublishedElement>>(dtdGuid);
            if (settings == null)
                return new List<IPublishedElement>() { breakpoint.Settings<IPublishedElement>(dtdGuid) }.Where(x => x != null).ToList();
            else
                return settings;
        }

        public static IPublishedElement GetSetting(this PositionalContentBreakpoint breakpoint, Guid dtdGuid)
        {
            return breakpoint.GetSettings(dtdGuid).FirstOrDefault();
        }

        public static IList<IPublishedElement> GetSettings(this PositionalContentBreakpoint breakpoint, PositionalContentModel model)
        {
            return breakpoint.GetSettings(model.DtdGuid);
        }

        public static IPublishedElement GetSetting(this PositionalContentBreakpoint breakpoint, PositionalContentModel model)
        {
            return breakpoint.GetSetting(model.DtdGuid);
        }

        public static IEnumerable<E> GetSettings<E>(this PositionalContentBreakpoint breakpoint, PositionalContentModel model)
        where E : PublishedElementModel
        {
            var content = breakpoint.GetSettings(model);
            return content.Select(x => x.ToModel<E>());
        }

        public static E GetSetting<E>(this PositionalContentBreakpoint breakpoint, PositionalContentModel model)
            where E : PublishedElementModel
        {
            var content = breakpoint.GetSetting(model);
            return content.ToModel<E>();
        }

    }

}

