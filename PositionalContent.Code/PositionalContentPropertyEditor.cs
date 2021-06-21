using System;
using System.Collections.Generic;
using System.Linq;
using ClientDependency.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Models.Editors;
using Umbraco.Core.PropertyEditors;
using Umbraco.Core.Services;
using Umbraco.Web.PropertyEditors;

namespace Hifi.PositionalContent
{
    [PropertyEditorAsset(ClientDependencyType.Javascript, "~/App_Plugins/PositionalContent/breakpoint/breakpoint.service.js", Priority = 11)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "~/App_Plugins/PositionalContent/breakpoint/breakpoint.directive.js", Priority = 12)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "~/App_Plugins/PositionalContent/breakpointControls/breakpointControls.service.js", Priority = 13)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "~/App_Plugins/PositionalContent/breakpointControls/breakpointControls.directive.js", Priority = 14)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "~/App_Plugins/PositionalContent/cropper/cropper.service.js", Priority = 15)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "~/App_Plugins/PositionalContent/cropper/cropper.directive.js", Priority = 16)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "~/App_Plugins/PositionalContent/contentEditor/positionalcontenteditor.controller.js", Priority = 2)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "~/App_Plugins/PositionalContent/directives/imageOnLoad.directive.js", Priority = 6)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "~/App_Plugins/PositionalContent/directives/move.directive.js", Priority = 7)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "~/App_Plugins/PositionalContent/directives/resize.directive.js", Priority = 8)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "~/App_Plugins/PositionalContent/directives/selectOnClick.directive.js", Priority = 9)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "~/App_Plugins/PositionalContent/globalControls/globalControls.directive.js", Priority = 17)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "~/App_Plugins/PositionalContent/globalControls/globalControls.service.js", Priority = 17)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "~/App_Plugins/PositionalContent/item/item.service.js", Priority = 18)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "~/App_Plugins/PositionalContent/item/item.directive.js", Priority = 19)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "~/App_Plugins/PositionalContent/propertyEditor/propertyEditorBreakpoint.controller.js", Priority = 3)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "~/App_Plugins/PositionalContent/propertyEditor/propertyEditorPicker.controller.js", Priority = 4)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "~/App_Plugins/PositionalContent/propertyEditor/propertyEditorInitialDimensions.controller.js", Priority = 5)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "~/App_Plugins/PositionalContent/resources/resources.js", Priority = 10)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "~/App_Plugins/PositionalContent/services/image.service.js", Priority = 21)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "~/App_Plugins/PositionalContent/services/premium.service.js", Priority = 20)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "~/App_Plugins/PositionalContent/services/scale.service.js", Priority = 22)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "~/App_Plugins/PositionalContent/services/util.service.js", Priority = 23)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "~/App_Plugins/PositionalContent/positionalcontent.controller.js", Priority = 1)]
    [PropertyEditorAsset(ClientDependencyType.Css, "~/App_Plugins/PositionalContent/positionalcontent-backoffice.css", Priority = 24)]
    [PropertyEditor("HiFi.PositionalContent", "Positional Content", "~/App_Plugins/PositionalContent/positionalcontent.html", ValueType = "JSON")]
    public class PositionalContentPropertyEditor : PropertyEditor
    {
        private IDictionary<string, object> _defaultPreValues;
        public override IDictionary<string, object> DefaultPreValues
        {
            get { return _defaultPreValues; }
            set { _defaultPreValues = value; }
        }

        public PositionalContentPropertyEditor()
        {
            // Setup default values
            _defaultPreValues = new Dictionary<string, object>
            {
                { "imageSizeMultiplier", "1" },
                { "previewImageQuality", "50" },
                { "cssNamespace", "default" },
                { "initialItemDimensions", "{ top: 5, left: 5, width: 15, height: 10, heightAuto: true }" }
            };
        }

        #region Pre Value Editor

        protected override PreValueEditor CreatePreValueEditor()
        {
            return new PositionalContentPreValueEditor();
        }

        internal class PositionalContentPreValueEditor : PreValueEditor
        {

            [PreValueField(Constants.ItemContentDataType, "Item Content Data Type", "~/App_Plugins/PositionalContent/propertyEditor/propertyEditorPicker.html", Description = "Positional content items")]
            public string ItemContentDataType { get; set; }

            [PreValueField(Constants.ItemSettingsDataType, "Item Settings Data Type", "~/App_Plugins/PositionalContent/propertyEditor/propertyEditorPicker.html", Description = "Positional content settings")]
            public string ItemSettingsDataType { get; set; }

            [PreValueField(Constants.ImageContentDataType, "Image Content Data Type", "~/App_Plugins/PositionalContent/propertyEditor/propertyEditorPicker.html", Description = "Content attached to the background image.")]
            public string ImageContentDataType { get; set; }

            [PreValueField(Constants.ImageSettingsDataType, "Image Settings Data Type", "~/App_Plugins/PositionalContent/propertyEditor/propertyEditorPicker.html", Description = "Settings attached to the background image.")]
            public string ImageSettingsDataType { get; set; }

            [PreValueField("breakpoints", "Breakpoints", "~/App_Plugins/PositionalContent/propertyEditor/propertyEditorBreakpoints.html", Description = "Breakpoints to setup for multiple responsive overlays.")]
            public string Breakpoints { get; set; }

            [PreValueField("initialItemDimensions", "Initial Item Dimensions", "~/App_Plugins/PositionalContent/propertyEditor/propertyEditorIntialDimesions.html", Description = "Defines the initial dimensions used when a new item is added")]
            public string InitialItemDimensions { get; set; }

            [PreValueField("hideLabel", "Hide Label", "boolean", Description = "Hide the Umbraco property title and description, making the control span the entire page width")]
            public bool HideLabel { get; set; }

            [PreValueField("cssNamespace", "CSS Namespace", "textstring", Description = "Name used to distinguish this positional content wrapper from other instances when building the Css for the breakpoint styles.")]
            public string CssNamespace { get; set; }

            [PreValueField("previewModifierClass", "Preview Modifier Class", "textstring", Description = "Passes through a cleass nameto the preview model, this can be used to apply styles specific to this instance of Positional Content.")]
            public string PreviewModifierClass { get; set; }

            [PreValueField("imageSizeMultiplier", "Image Size Multiplier", "number", Description = "Sets the size multipler of the original image for higher density screen resolutions.")]
            public string ImageSizeMuliplier { get; set; }

            [PreValueField("previewImageQuality", "Preview Image Quality", "number", Description = "Sets the quality setting of the preview image in the editor, lowering this will improve the speed of the editor.")]
            public string PreviewImageQauality { get; set; }

        }

        #endregion

        #region Value Editor

        protected override PropertyValueEditor CreateValueEditor()
        {
            return new PositionalContentPropertyValueEditor(base.CreateValueEditor());
        }

        internal class PositionalContentPropertyValueEditor : PropertyValueEditorWrapper
        {
            public PositionalContentPropertyValueEditor(PropertyValueEditor wrapped)
                : base(wrapped)
            { }

            public override string ConvertDbToString(Property property, PropertyType propertyType, IDataTypeService dataTypeService)
            {
                if (property.Value == null || property.Value.ToString().IsNullOrWhiteSpace())
                    return string.Empty;

                // Something weird is happening in core whereby ConvertDbToString is getting
                // called loads of times on publish, forcing the property value to get converted
                // again, which in tern screws up the values. To get round it, we create a 
                // dummy property copying the original properties value, this way not overwriting
                // the original property value allowing it to be re-converted again later
                var prop2 = new Property(propertyType, property.Value);

                try
                {
                    var value = JsonConvert.DeserializeObject<PositionalContentModel>(property.Value.ToString());
                    if (value!= null)
                    {
                        AssignValuesToDb(value, dataTypeService, AssignValueDbToString);

                        prop2.Value = JsonConvert.SerializeObject(value);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error<PositionalContentPropertyValueEditor>(string.Format("Error converting DB value to String for - {0}", property.Value), ex);
                }

                return base.ConvertDbToString(prop2, propertyType, dataTypeService);
            }



            public override object ConvertDbToEditor(Property property, PropertyType propertyType, IDataTypeService dataTypeService)
            {
                if (property.Value == null || property.Value.ToString().IsNullOrWhiteSpace())
                    return string.Empty;

                // Something weird is happening in core whereby ConvertDbToString is getting
                // called loads of times on publish, forcing the property value to get converted
                // again, which in tern screws up the values. To get round it, we create a 
                // dummy property copying the original properties value, this way not overwriting
                // the original property value allowing it to be re-converted again later
                var prop2 = new Property(propertyType, property.Value);

                try
                {
                    var value = JsonConvert.DeserializeObject<PositionalContentModel>(property.Value.ToString());
                    if (value.Items != null)
                    {

                        AssignValuesToDb(value, dataTypeService, AssignValueDbToEditor);

                        prop2.Value = JsonConvert.SerializeObject(value);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error<PositionalContentPropertyValueEditor>(string.Format("Error converting DB value to Editor - {0}", property.Value), ex);
                }

                return base.ConvertDbToEditor(prop2, propertyType, dataTypeService);
            }

            public override object ConvertEditorToDb(ContentPropertyData editorValue, object currentValue)
            {
                if (editorValue.Value == null || editorValue.Value.ToString().IsNullOrWhiteSpace())
                    return string.Empty;

                try
                {
                    var value = JsonConvert.DeserializeObject<PositionalContentModel>(editorValue.Value.ToString());

                    AssignValuesToDb(value, null, AssignEditorToDb);

                    return JsonConvert.SerializeObject(value);
                }
                catch (Exception ex)
                {
                    LogHelper.Error<PositionalContentPropertyValueEditor>(string.Format("Error converting DB value to Editor - {0}", editorValue.Value), ex);
                }

                return base.ConvertEditorToDb(editorValue, currentValue);
            }

            public static object AssignValueDbToString(ChildDataType datatype, object rawData, IDataTypeService dataTypeService)
            {
                var prop = new Property(datatype.PropertyType, rawData == null ? null : rawData.ToString());
                var newValue = datatype.PropertyEditor.ValueEditor.ConvertDbToString(prop, datatype.PropertyType, dataTypeService);
                return newValue;
            }

            public static object AssignValueDbToEditor(ChildDataType datatype, object rawData, IDataTypeService dataTypeService)
            {
                var prop = new Property(datatype.PropertyType, rawData == null ? null : rawData.ToString());
                var newValue = datatype.PropertyEditor.ValueEditor.ConvertDbToEditor(prop, datatype.PropertyType, dataTypeService);
                return (newValue == null) ? null : JToken.FromObject(newValue);

            }

            public static object AssignEditorToDb(ChildDataType datatype, object rawData, IDataTypeService dataTypeService)
            {
                var propData = new ContentPropertyData(rawData, datatype.PreValues, new Dictionary<string, object>());
                var newValue = datatype.PropertyEditor.ValueEditor.ConvertEditorToDb(propData, rawData);
                return (newValue == null) ? null : JToken.FromObject(newValue);
            }

            public static void AssignValuesToDb(PositionalContentModel content, IDataTypeService dataTypeService, Func<ChildDataType, object, IDataTypeService, object> valueProcessor)
            {
                var imageContent = PositionalContentHelper.GetDataTypeToDb(content.DtdGuid, Constants.ImageContentDataType);
                var imageSettings = PositionalContentHelper.GetDataTypeToDb(content.DtdGuid, Constants.ImageSettingsDataType);
                var itemContent = PositionalContentHelper.GetDataTypeToDb(content.DtdGuid, Constants.ItemContentDataType);
                var itemSettings = PositionalContentHelper.GetDataTypeToDb(content.DtdGuid, Constants.ItemSettingsDataType);

                if(imageContent != null)
                    content.content = valueProcessor(imageContent, content.content, dataTypeService);

                if (itemSettings != null)
                    content.settings = valueProcessor(itemSettings, content.settings, dataTypeService);

                foreach (var breakpoint in content.Breakpoints)
                {
                    if (imageContent != null)
                        breakpoint.Value.content = valueProcessor(imageContent, breakpoint.Value.content, dataTypeService);

                    if (imageSettings != null)
                        breakpoint.Value.settings = valueProcessor(imageSettings, breakpoint.Value.settings, dataTypeService);
                }

                foreach (var item in content.Items)
                {
                    if(itemContent != null)
                        item.content = valueProcessor(itemContent, item.content, dataTypeService);

                    if (itemSettings != null)
                        item.settings = valueProcessor(itemSettings, item.settings, dataTypeService);

                    foreach (var dimension in item.Dimensions)
                    {
                        if (itemContent != null)
                            dimension.Value.content = valueProcessor(itemContent, dimension.Value.content, dataTypeService);

                        if (itemSettings != null)
                            dimension.Value.settings = valueProcessor(itemSettings, dimension.Value.settings, dataTypeService);
                    }
                }
            }
        }

        #endregion
    }
}
