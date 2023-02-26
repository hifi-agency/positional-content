using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Hifi.PositionalContent
{

    public static class ItemContentAndSettingsExtensions
    {

        //Item Content
        public static IList<IPublishedElement> GetContents(this PositionalContentItem item, Guid dtdGuid, PositionalContentItemDimension dimension = null)
        {
            if (dimension != null && dimension.OverrideContent && dimension.HasContent)
                return dimension.GetContents(dtdGuid);
            else
            {
                var contents = item.Content<List<IPublishedElement>>(dtdGuid);
                if (contents == null)
                    return new List<IPublishedElement>() { item.Content<IPublishedElement>(dtdGuid) }.Where(x => x != null).ToList();
                else
                    return contents;
            }
        }

        public static IPublishedElement GetContent(this PositionalContentItem item, Guid dtdGuid, PositionalContentItemDimension dimension = null)
        {
            return item.GetContents(dtdGuid, dimension).FirstOrDefault();
        }

        public static IList<IPublishedElement> GetContents(this PositionalContentItem item, PositionalContentModel model, PositionalContentItemDimension dimension = null)
        {
            return item.GetContents(model.DtdGuid, dimension);
        }

        public static IPublishedElement GetContent(this PositionalContentItem item, PositionalContentModel model, PositionalContentItemDimension dimension = null)
        {
            return item.GetContent(model.DtdGuid, dimension);
        }

        public static IEnumerable<E> GetContents<E>(this PositionalContentItem item, PositionalContentModel model, PositionalContentItemDimension dimension = null)
        where E : PublishedElementModel
        {
            var content = item.GetContents(model, dimension);
            return content.Select(x => x.ToModel<E>());
        }

        public static E GetContent<E>(this PositionalContentItem item, PositionalContentModel model, PositionalContentItemDimension dimension = null)
            where E : PublishedElementModel
        {
            var content = item.GetContent(model, dimension);
            return content.ToModel<E>();
        }


        //Item Settings
        public static IList<IPublishedElement> GetSettings(this PositionalContentItem item, Guid dtdGuid, PositionalContentItemDimension dimension = null)
        {
            if (dimension != null && dimension.OverrideSettings && dimension.HasSettings)
                return dimension.GetSettings(dtdGuid);
            else
            {
                var settings = item.Settings<List<IPublishedElement>>(dtdGuid);
                if (settings == null)
                    return new List<IPublishedElement>() { item.Settings<IPublishedElement>(dtdGuid) }.Where(x => x != null).ToList();
                else
                    return settings;
            }
        }

        public static IPublishedElement GetSetting(this PositionalContentItem item, Guid dtdGuid, PositionalContentItemDimension dimension = null)
        {
            return item.GetSettings(dtdGuid, dimension).FirstOrDefault();
        }

        public static IList<IPublishedElement> GetSettings(this PositionalContentItem item, PositionalContentModel model, PositionalContentItemDimension dimension = null)
        {
            return item.GetSettings(model.DtdGuid, dimension);
        }

        public static IPublishedElement GetSetting(this PositionalContentItem item, PositionalContentModel model, PositionalContentItemDimension dimension = null)
        {
            return item.GetSetting(model.DtdGuid, dimension);
        }

        public static IEnumerable<E> GetSettings<E>(this PositionalContentItem item, PositionalContentModel model, PositionalContentItemDimension dimension = null)
             where E : PublishedElementModel
        {
            var content = item.GetSettings(model, dimension);
            return content.Select(x => x.ToModel<E>());
        }

        public static E GetSetting<E>(this PositionalContentItem item, PositionalContentModel model, PositionalContentItemDimension dimension = null)
            where E : PublishedElementModel
        {
            var content = item.GetSetting(model, dimension);
            return content.ToModel<E>();
        }

    }

}

