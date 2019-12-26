using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Umbraco.Web;

using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using System.Globalization;
using Umbraco.Web.Composing;

namespace Hifi.PositionalContent
{

    public static class PositionalContentExtensionHelpers
    {
        public static NumberFormatInfo NumberFormatInfo
        {
            get
            {
                NumberFormatInfo info = new NumberFormatInfo();
                info.NumberDecimalSeparator = ".";
                info.PercentDecimalSeparator = ".";
                return info;
            }
        }

        public static IEnumerable<PositionalContentBreakpoint> BreakpointsAsList(this PositionalContentModel model)
        {
            return model.Breakpoints.Select(x => x.Value);
        }

        public static PositionalContentBreakpoint SingleBreakpoint(this PositionalContentModel model)
        {
            return model.Breakpoints.Select(x => x.Value).FirstOrDefault();
        }

        public static IEnumerable<PositionalContentItemDimension> DimensionsAsList(this PositionalContentItem item)
        {
            return item.Dimensions.Select(x => x.Value);
        }

        public static PositionalContentItemDimension SingleDimension(this PositionalContentItem item)
        {
            return item.Dimensions.Select(x => x.Value).FirstOrDefault();
        }

        public static string ImageUrl(this PositionalContentBreakpoint breakpoint, PositionalContentModel model, bool leaveOffCropSettings = false, int multiplier = 1)
        {
            var image = breakpoint.Image(model);
            if(image != null)
            {
                if (leaveOffCropSettings)
                    return image.Url;
                else
                    return string.Format("{0}{1}", image.Url, breakpoint.CropSettings(multiplier));
            }
            return string.Empty;
        }

        public static IPublishedContent Image(this PositionalContentBreakpoint breakpoint, PositionalContentModel model)
        {
            if(breakpoint != null)
            {
                string imageId = breakpoint.ImageId;
                if (string.IsNullOrEmpty(breakpoint.ImageId))
                {
                    var image = model.PrimaryImage;
                    if (image != null)
                        imageId = image.ImageId;
                }
                return Current.UmbracoHelper.Media(imageId);
            }
            return null;
        }

        public static string CropSettings(this PositionalContentBreakpoint breakpoint, int multiplier = 1)
        {
            return "?crop=" + breakpoint.Left.ToString(NumberFormatInfo) + ',' + breakpoint.Top.ToString(NumberFormatInfo) + ',' + breakpoint.Right.ToString(NumberFormatInfo) + ',' + breakpoint.Bottom.ToString(NumberFormatInfo) + "&cropmode=percentage&width=" + breakpoint.CropWidth * multiplier + "&height=" + breakpoint.CropHeight * multiplier + "&mode=crop";
        }

        public static string HeightCss(this PositionalContentItemDimension dimension)
        {
            if (dimension != null)
                if (dimension.HeightAuto)
                    return "height: auto;";
                else
                    return string.Format("height: {0}%;", dimension.Height.ToString(NumberFormatInfo));
            else
                return string.Empty;
        }

        public static string WidthCss(this PositionalContentItemDimension dimension)
        {
            if (dimension != null)
                if (dimension.WidthAuto)
                    return "width: auto;";
                else
                    return string.Format("width: {0}%;", dimension.Width.ToString(NumberFormatInfo));
            else
                return string.Empty;
        }

        public static Dictionary<string, string> PositionalStyles(this PositionalContentItemDimension dimension)
        {
            var styles = new Dictionary<string, string>();

            if(dimension.WidthAuto)
                styles["width"] = "auto";
            else
                styles["width"] = dimension.Width.AppendPercentage();

            if (dimension.HeightAuto)
                styles["height"] = "auto";
            else
                styles["height"] = dimension.Height.AppendPercentage();

            if (dimension.PinToBottom)
                styles["bottom"] = dimension.Bottom.AppendPercentage();
            else
                styles["top"] = dimension.Top.AppendPercentage();

            if (dimension.PinToRight)
                styles["right"] = dimension.Right.AppendPercentage();
            else
                styles["left"] = dimension.Left.AppendPercentage();

            return styles;
        }

        public static string PositionalStylesAsString(this PositionalContentItemDimension item)
        {
            var output = item.PositionalStyles();

            return String.Join("", output.Select(x => string.Format("{0}:{1};", x.Key, x.Value)));
        }

        public static string AppendPercentage(this float val)
        {
            return string.Format("{0}%", val.ToString(NumberFormatInfo));
        }

        public static string GetInnerMessage(Exception ex)
        {
            if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
                return ex.InnerException.Message;

            return ex.Message;
        }

    }

    internal static class PositionalContentExtensionHelperInternal
    {
        public static E ToModel<E>(this IPublishedElement content)
            where E : PublishedElementModel
        {
            var type = typeof(E);
            if (content != null && content.ContentType.Alias.ToLower() == type.Name.ToLower())
                return Activator.CreateInstance(typeof(E), new object[] { content }) as E;
            else
                return null;
        }
    }

}

