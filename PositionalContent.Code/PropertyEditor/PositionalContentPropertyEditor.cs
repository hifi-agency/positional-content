using System;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.Editors;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Strings;
using Umbraco.Cms.Core.WebAssets;
using Umbraco.Cms.Infrastructure.Serialization;
using Umbraco.Cms.Infrastructure.WebAssets;
using Umbraco.Cms.Web.Common;
using Umbraco.Extensions;

namespace Hifi.PositionalContent
{
    [DataEditor("HiFi.PositionalContent", "Positional Content",
        "../App_Plugins/PositionalContent/positionalcontent.html", ValueType = ValueTypes.Json)]
    public class PositionalContentPropertyEditor : DataEditor
    {
        private readonly IUmbracoBuilder _umbracoBuilder;
        private readonly IEditorConfigurationParser _editorConfigurationParser;
        private readonly PropertyEditorCollection _propertyEditorCollection;
        private readonly IDataTypeService _dataTypeService;
        private readonly IUmbracoHelperAccessor _umbracoHelperAccessor;
        private readonly ILocalizedTextService _localizedTextService;
        private readonly IIOHelper _ioHelper;
        private readonly IShortStringHelper _shortStringHelper;
        private readonly ILogger logger;
        private readonly PositionalContentChildDataTypeService childDataTypeService;


        public PositionalContentPropertyEditor(
            PositionalContentChildDataTypeService positionalContentChildDataTypeService,
            IEditorConfigurationParser editorConfigurationParser, PropertyEditorCollection propertyEditorCollection,
            IDataTypeService dataTypeService, IDataValueEditorFactory dataValueEditorFactory,
            IUmbracoHelperAccessor umbracoHelperAccessor, ILocalizedTextService localizedTextService,
            IIOHelper ioHelper, IShortStringHelper shortStringHelper) : base(dataValueEditorFactory)
        {
            _editorConfigurationParser = editorConfigurationParser;
            _propertyEditorCollection = propertyEditorCollection;
            _dataTypeService = dataTypeService;
            _umbracoHelperAccessor = umbracoHelperAccessor;
            _localizedTextService = localizedTextService;
            _ioHelper = ioHelper;
            _shortStringHelper = shortStringHelper;
            childDataTypeService = positionalContentChildDataTypeService;
        }


        protected override IConfigurationEditor CreateConfigurationEditor() =>
            new PositionalContentConfigurationEditor(_ioHelper, _editorConfigurationParser);

        #region Value Editor

        protected override IDataValueEditor CreateValueEditor() => new PositionalContentPropertyValueEditor(
            _propertyEditorCollection, _dataTypeService, _umbracoHelperAccessor, _localizedTextService,
            _shortStringHelper, childDataTypeService, new JsonNetSerializer());

        internal class PositionalContentPropertyValueEditor : DataValueEditor
        {
            private readonly ILogger logger;
            private readonly IShortStringHelper _shortStringHelper;
            private readonly PropertyEditorCollection _propertyEditorCollection;
            private readonly IDataTypeService _dataTypeService;
            private readonly IUmbracoHelperAccessor _umbracoHelperAccessor;
            private readonly PositionalContentChildDataTypeService childDataTypeService;


            public override object Configuration
            {
                get => base.Configuration;
                set
                {
                    if (value == null)
                        throw new ArgumentNullException(nameof(value));
                    if (!(value is PositionalContentConfiguration configuration))
                        throw new ArgumentException(
                            $"Expected a {typeof(PositionalContentConfiguration).Name} instance, but got {value.GetType().Name}.",
                            nameof(value));
                    base.Configuration = value;

                    HideLabel = configuration.HideLabel.TryConvertTo<bool>().Result;
                }
            }

            public override string ConvertDbToString(IPropertyType propertyType, object? propertyValue)
            {
                if (propertyValue == null || propertyValue.ToString().IsNullOrWhiteSpace())
                    return string.Empty;

                object outputValue = null;

                try
                {
                    var value = JsonConvert.DeserializeObject<PositionalContentModel>(propertyValue.ToString());
                    if (value != null)
                    {
                        AssignValuesToDb(value, _dataTypeService, AssignValueDbToString);

                        outputValue = JsonConvert.SerializeObject(value);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(
                        string.Format("Error converting DB value to String for - {0}", propertyValue.ToString()), ex);
                }

                return base.ConvertDbToString(propertyType, outputValue);
            }

            public override object? ToEditor(IProperty property, string? culture = null, string? segment = null)
            {
                var val = property.GetValue(culture, segment);
                if (val == null || string.IsNullOrWhiteSpace(val.ToString()))
                    return string.Empty;


                try
                {
                    var value = JsonConvert.DeserializeObject<PositionalContentModel>(property
                        .GetValue(culture, segment).ToString());
                    FixImageIds(value);
                    if (value.Items != null)
                    {
                        AssignValuesToDb(value, _dataTypeService, AssignValueDbToEditor);

                        property.SetValue(JsonConvert.SerializeObject(value));
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(string.Format("Error converting DB value to Editor - {0}", val), ex);
                }

                return base.ToEditor(property, culture, segment);
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
                    logger.LogError($"Error converting DB value to Editor - {editorValue.Value}", ex);
                }

                return base.FromEditor(editorValue, currentValue);
            }

            protected object AssignValueDbToString(ChildDataType datatype, object rawData)
            {
                var prop = new Property(datatype.PropertyType);

                //culture?
                if (rawData != null)
                    prop.SetValue(rawData);

                var newValue = datatype.ValueEditor.ConvertDbToString(prop.PropertyType, rawData);
                return newValue;
            }

            protected object AssignValueDbToEditor(ChildDataType datatype, object rawData)
            {
                var prop = new Property(datatype.PropertyType);

                //culture?
                if (rawData != null)
                    prop.SetValue(rawData);

                var newValue = datatype.ValueEditor.ToEditor(prop);
                return (newValue == null) ? null : JToken.FromObject(newValue);
            }

            protected object AssignEditorToDb(ChildDataType datatype, object rawData)
            {
                var propData = new ContentPropertyData(rawData, datatype.PreValues);
                var newValue = datatype.ValueEditor.FromEditor(propData, rawData);
                return (newValue == null) ? null : JToken.FromObject(newValue);
            }

            protected void AssignValuesToDb(PositionalContentModel content, IDataTypeService dataTypeService,
                Func<ChildDataType, object, object> valueProcessor)
            {
                var imageContent = GetDataTypeToDb(content.DtdGuid, Constants.ImageContentDataType);
                var imageSettings = GetDataTypeToDb(content.DtdGuid, Constants.ImageSettingsDataType);
                var itemContent = GetDataTypeToDb(content.DtdGuid, Constants.ItemContentDataType);
                var itemSettings = GetDataTypeToDb(content.DtdGuid, Constants.ItemSettingsDataType);

                if (imageContent != null)
                    content.content = valueProcessor(imageContent, content.content);

                if (itemSettings != null)
                    content.settings = valueProcessor(itemSettings, content.settings);

                foreach (var breakpoint in content.Breakpoints)
                {
                    if (imageContent != null)
                        breakpoint.Value.content = valueProcessor(imageContent, breakpoint.Value.content);

                    if (imageSettings != null)
                        breakpoint.Value.settings = valueProcessor(imageSettings, breakpoint.Value.settings);
                }

                foreach (var item in content.Items)
                {
                    if (itemContent != null)
                        item.content = valueProcessor(itemContent, item.content);

                    if (itemSettings != null)
                        item.settings = valueProcessor(itemSettings, item.settings);

                    foreach (var dimension in item.Dimensions)
                    {
                        if (itemContent != null)
                            dimension.Value.content = valueProcessor(itemContent, dimension.Value.content);

                        if (itemSettings != null)
                            dimension.Value.settings = valueProcessor(itemSettings, dimension.Value.settings);
                    }
                }
            }

            protected ChildDataType GetDataTypeToDb(Guid dtdGuid, string property, bool from = false)
            {
                var dtd = childDataTypeService.Get(dtdGuid, property);
                if (dtd != null)
                {
                    var propType = new PropertyType(_shortStringHelper, dtd);
                    var propEditor = _propertyEditorCollection[dtd.EditorAlias];
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

            protected void FixImageIds(PositionalContentModel model)
            {
                if (model != null)
                {
                    if (model.PrimaryImage != null)
                    {
                        model.PrimaryImage.ImageId = FixImageId(model.PrimaryImage.ImageId);
                    }

                    foreach (var i in model.Breakpoints)
                    {
                        i.Value.ImageId = FixImageId(i.Value.ImageId);
                    }
                }
            }

            protected string FixImageId(string input)
            {
                if (input != null && input.Length < 6 && !_umbracoHelperAccessor.TryGetUmbracoHelper(out var helper))
                {
                    var currentImage = helper.Media(input);
                    if (currentImage != null)
                    {
                        return currentImage.Key.ToString();
                    }
                }

                return input;
            }

            public PositionalContentPropertyValueEditor(PropertyEditorCollection propertyEditorCollection,
                IDataTypeService dataTypeService, IUmbracoHelperAccessor umbracoHelperAccessor,
                ILocalizedTextService localizedTextService, IShortStringHelper shortStringHelper,
                PositionalContentChildDataTypeService childDataTypeService, IJsonSerializer? jsonSerializer) : base(
                localizedTextService, shortStringHelper, jsonSerializer)
            {
                _shortStringHelper = shortStringHelper;
                _propertyEditorCollection = propertyEditorCollection;
                _dataTypeService = dataTypeService;
                _umbracoHelperAccessor = umbracoHelperAccessor;
                this.childDataTypeService = childDataTypeService;
                this.View = "../App_Plugins/PositionalContent/positionalcontent.html";
            }
        }

        #endregion
    }
}