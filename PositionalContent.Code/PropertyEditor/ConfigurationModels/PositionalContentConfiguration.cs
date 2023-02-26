using Umbraco.Cms.Core.PropertyEditors;

namespace Hifi.PositionalContent
{
    public class PositionalContentConfiguration
    {
        [ConfigurationField(Constants.ItemContentDataType, "Item Content Data Type", "~/App_Plugins/PositionalContent/propertyEditor/propertyEditorPicker.html", Description = "Positional content items")]
        public ContentDataTypeContainer ItemContentDataType { get; set; }

        [ConfigurationField(Constants.ItemSettingsDataType, "Item Settings Data Type", "~/App_Plugins/PositionalContent/propertyEditor/propertyEditorPicker.html", Description = "Positional content settings")]
        public ContentDataTypeContainer ItemSettingsDataType { get; set; }

        [ConfigurationField(Constants.ImageContentDataType, "Image Content Data Type", "~/App_Plugins/PositionalContent/propertyEditor/propertyEditorPicker.html", Description = "Content attached to the background image.")]
        public ContentDataTypeContainer ImageContentDataType { get; set; }

        [ConfigurationField(Constants.ImageSettingsDataType, "Image Settings Data Type", "~/App_Plugins/PositionalContent/propertyEditor/propertyEditorPicker.html", Description = "Settings attached to the background image.")]
        public ContentDataTypeContainer ImageSettingsDataType { get; set; }

        [ConfigurationField("breakpoints", "Breakpoints", "~/App_Plugins/PositionalContent/propertyEditor/propertyEditorBreakpoints.html", Description = "Breakpoints to setup for multiple responsive overlays.")]
        public PositionalContentConfigurationBreakpoint[] Breakpoints { get; set; }

        [ConfigurationField("initialItemDimensions", "Initial Item Dimensions", "~/App_Plugins/PositionalContent/propertyEditor/propertyEditorIntialDimesions.html", Description = "Defines the initial dimensions used when a new item is added")]
        public PositionalContentConfigurationInitialDimension InitialItemDimensions { get; set; }

        [ConfigurationField("hideLabel", "Hide Label", "boolean", Description = "Hide the Umbraco property title and description, making the control span the entire page width")]
        public bool HideLabel { get; set; }

        [ConfigurationField("cssNamespace", "CSS Namespace", "textstring", Description = "Name used to distinguish this positional content wrapper from other instances when building the Css for the breakpoint styles.")]
        public string CssNamespace { get; set; }

        [ConfigurationField("previewModifierClass", "Preview Modifier Class", "textstring", Description = "Passes through a cleass nameto the preview model, this can be used to apply styles specific to this instance of Positional Content.")]
        public string PreviewModifierClass { get; set; }

        [ConfigurationField("imageSizeMultiplier", "Image Size Multiplier", "number", Description = "Sets the size multipler of the original image for higher density screen resolutions.")]
        public string ImageSizeMuliplier { get; set; }

        [ConfigurationField("previewImageQuality", "Preview Image Quality", "number", Description = "Sets the quality setting of the preview image in the editor, lowering this will improve the speed of the editor.")]
        public string PreviewImageQauality { get; set; }
    }
}
