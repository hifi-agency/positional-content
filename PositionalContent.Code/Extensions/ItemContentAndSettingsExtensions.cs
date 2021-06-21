using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Umbraco.Web;

using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;

namespace Hifi.PositionalContent
{

    public static class ItemContentAndSettingsExtensions
    {

        //Item Content
        public static IList<IPublishedContent> GetContents(this PositionalContentItem item, Guid dtdGuid, PositionalContentItemDimension dimension = null)
        {
            if (dimension != null && dimension.OverrideContent && dimension.HasContent)
                return dimension.GetContents(dtdGuid);
            else
            {
                var contents = item.Content<List<IPublishedContent>>(dtdGuid);
                if (contents == null)
                    return new List<IPublishedContent>() { item.Content<IPublishedContent>(dtdGuid) }.Where(x => x != null).ToList();
                else
                    return contents;
            }
        }

        public static IPublishedContent GetContent(this PositionalContentItem item, Guid dtdGuid, PositionalContentItemDimension dimension = null)
        {
            return item.GetContents(dtdGuid, dimension).FirstOrDefault();
        }

        public static IList<IPublishedContent> GetContents(this PositionalContentItem item, PositionalContentModel model, PositionalContentItemDimension dimension = null)
        {
            return item.GetContents(model.DtdGuid, dimension);
        }

        public static IPublishedContent GetContent(this PositionalContentItem item, PositionalContentModel model, PositionalContentItemDimension dimension = null)
        {
            return item.GetContent(model.DtdGuid, dimension);
        }

        public static IEnumerable<E> GetContents<E>(this PositionalContentItem item, PositionalContentModel model, PositionalContentItemDimension dimension = null)
        where E : PublishedContentModel
        {
            var content = item.GetContents(model, dimension);
            return content.Select(x => x.ToModel<E>());
        }

        public static E GetContent<E>(this PositionalContentItem item, PositionalContentModel model, PositionalContentItemDimension dimension = null)
            where E : PublishedContentModel
        {
            var content = item.GetContent(model, dimension);
            return content.ToModel<E>();
        }


        //Item Settings
        public static IList<IPublishedContent> GetSettings(this PositionalContentItem item, Guid dtdGuid, PositionalContentItemDimension dimension = null)
        {
            if (dimension != null && dimension.OverrideSettings && dimension.HasSettings)
                return dimension.GetSettings(dtdGuid);
            else
            {
                var settings = item.Settings<List<IPublishedContent>>(dtdGuid);
                if (settings == null)
                    return new List<IPublishedContent>() { item.Settings<IPublishedContent>(dtdGuid) }.Where(x => x != null).ToList();
                else
                    return settings;
            }
        }

        public static IPublishedContent GetSetting(this PositionalContentItem item, Guid dtdGuid, PositionalContentItemDimension dimension = null)
        {
            return item.GetSettings(dtdGuid, dimension).FirstOrDefault();
        }

        public static IList<IPublishedContent> GetSettings(this PositionalContentItem item, PositionalContentModel model, PositionalContentItemDimension dimension = null)
        {
            return item.GetSettings(model.DtdGuid, dimension);
        }

        public static IPublishedContent GetSetting(this PositionalContentItem item, PositionalContentModel model, PositionalContentItemDimension dimension = null)
        {
            return item.GetSetting(model.DtdGuid, dimension);
        }

        public static IEnumerable<E> GetSettings<E>(this PositionalContentItem item, PositionalContentModel model, PositionalContentItemDimension dimension = null)
             where E : PublishedContentModel
        {
            var content = item.GetSettings(model, dimension);
            return content.Select(x => x.ToModel<E>());
        }

        public static E GetSetting<E>(this PositionalContentItem item, PositionalContentModel model, PositionalContentItemDimension dimension = null)
            where E : PublishedContentModel
        {
            var content = item.GetSetting(model, dimension);
            return content.ToModel<E>();
        }

    }

}

