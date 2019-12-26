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
using Umbraco.Web.Composing;

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
    [DataEditor("HiFi.PositionalContent", "Positional Content", "~/App_Plugins/PositionalContent/positionalcontent.html", ValueType = ValueTypes.Json)]
    public class PositionalContentPropertyEditor : DataEditor
    {
        private readonly ILogger logger;
        private readonly PositionalContentChildDataTypeService childDataTypeService;

        public PositionalContentPropertyEditor(ILogger logger, PositionalContentChildDataTypeService childDataTypeService)
        : base(logger)
        {
            this.logger = logger;
            this.childDataTypeService = childDataTypeService;
        }

        protected override IConfigurationEditor CreateConfigurationEditor() => new PositionalContentConfigurationEditor();

        #region Value Editor

        protected override IDataValueEditor CreateValueEditor() => new PositionalContentPropertyValueEditor(Attribute, logger, childDataTypeService);

        internal class PositionalContentPropertyValueEditor : DataValueEditor
        {
            private readonly ILogger logger;
            private readonly PositionalContentChildDataTypeService childDataTypeService;

            public PositionalContentPropertyValueEditor(DataEditorAttribute attribute, ILogger logger, PositionalContentChildDataTypeService childDataTypeService)
                : base(attribute)
            {
                this.logger = logger;
                this.childDataTypeService = childDataTypeService;
            }

            public override object Configuration
            {
                get => base.Configuration;
                set
                {
                    if (value == null)
                        throw new ArgumentNullException(nameof(value));
                    if (!(value is PositionalContentConfiguration configuration))
                        throw new ArgumentException($"Expected a {typeof(PositionalContentConfiguration).Name} instance, but got {value.GetType().Name}.", nameof(value));
                    base.Configuration = value;

                    HideLabel = configuration.HideLabel.TryConvertTo<bool>().Result;
                }
            }

            public override string ConvertDbToString(PropertyType propertyType, object propertyValue, IDataTypeService dataTypeService)
            {
                if (propertyValue == null || propertyValue.ToString().IsNullOrWhiteSpace())
                    return string.Empty;

                object outputValue = null;

                try
                {
                    var value = JsonConvert.DeserializeObject<PositionalContentModel>(propertyValue.ToString());
                    if (value!= null)
                    {
                        AssignValuesToDb(value, dataTypeService, AssignValueDbToString);

                        outputValue = JsonConvert.SerializeObject(value);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error<PositionalContentPropertyValueEditor>(string.Format("Error converting DB value to String for - {0}", propertyValue.ToString()), ex);
                }

                return base.ConvertDbToString(propertyType, outputValue, dataTypeService);
            }



            public override object ToEditor(Property property, IDataTypeService dataTypeService, string culture = null, string segment = null)
            {
                var val = property.GetValue(culture, segment);
                if (val == null || string.IsNullOrWhiteSpace(val.ToString()))
                    return string.Empty;

                try
                {
                    var value = JsonConvert.DeserializeObject<PositionalContentModel>(property.GetValue(culture, segment).ToString());
                    if (value.Items != null)
                    {

                        AssignValuesToDb(value, dataTypeService, AssignValueDbToEditor);

                        property.SetValue(JsonConvert.SerializeObject(value));
                    }
                }
                catch (Exception ex)
                {
                    logger.Error<PositionalContentPropertyValueEditor>(string.Format("Error converting DB value to Editor - {0}", val), ex);
                }

                return base.ToEditor(property, dataTypeService, culture, segment);
            }

            public override object FromEditor(ContentPropertyData editorValue, object currentValue)
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
                    logger.Error<PositionalContentPropertyValueEditor>(string.Format("Error converting DB value to Editor - {0}", editorValue.Value), ex);
                }

                return base.FromEditor(editorValue, currentValue);
            }

            protected object AssignValueDbToString(ChildDataType datatype, object rawData, IDataTypeService dataTypeService)
            {
                var prop = new Property(datatype.PropertyType);

                //culture?
                if (rawData != null)
                    prop.SetValue(rawData);

                var newValue = datatype.ValueEditor.ConvertDbToString(prop.PropertyType, rawData, dataTypeService);
                return newValue;
            }

            protected object AssignValueDbToEditor(ChildDataType datatype, object rawData, IDataTypeService dataTypeService)
            {
                var prop = new Property(datatype.PropertyType);
                
                //culture?
                if (rawData != null)
                    prop.SetValue(rawData);

                var newValue = datatype.ValueEditor.ToEditor(prop, dataTypeService);
                return (newValue == null) ? null : JToken.FromObject(newValue);

            }

            protected object AssignEditorToDb(ChildDataType datatype, object rawData, IDataTypeService dataTypeService)
            {
                var propData = new ContentPropertyData(rawData, datatype.PreValues);
                var newValue = datatype.ValueEditor.FromEditor(propData, rawData);
                return (newValue == null) ? null : JToken.FromObject(newValue);
            }

            protected void AssignValuesToDb(PositionalContentModel content, IDataTypeService dataTypeService, Func<ChildDataType, object, IDataTypeService, object> valueProcessor)
            {
                var imageContent = GetDataTypeToDb(content.DtdGuid, Constants.ImageContentDataType);
                var imageSettings = GetDataTypeToDb(content.DtdGuid, Constants.ImageSettingsDataType);
                var itemContent = GetDataTypeToDb(content.DtdGuid, Constants.ItemContentDataType);
                var itemSettings = GetDataTypeToDb(content.DtdGuid, Constants.ItemSettingsDataType);

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

            protected ChildDataType GetDataTypeToDb(Guid dtdGuid, string property, bool from = false)
            {
                var dtd = childDataTypeService.Get(dtdGuid, property);
                if (dtd != null)
                {
                    var propType = new PropertyType(dtd);
                    var propEditor = Current.PropertyEditors[dtd.EditorAlias];
                    var valEditor = propEditor.GetValueEditor(dtd.Configuration);

                    return new ChildDataType()
                    {
                        DataTypeDefinition = dtd,
                        PropertyEditor = propEditor,
                        ValueEditor = valEditor,
                        PreValues = dtd.Configuration,
                        PropertyType = propType,
                        Config = propEditor.DefaultConfiguration
                    };
                }
                return null;
            }
        }

        #endregion
    }
}
