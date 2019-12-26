using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.PropertyEditors;
using Newtonsoft.Json;

namespace Hifi.PositionalContent
{
    public class PositionalContentConfigurationEditor : ConfigurationEditor<PositionalContentConfiguration>
    {
        public override object DefaultConfigurationObject => new PositionalContentConfiguration()
        {
            ImageSizeMuliplier = "1",
            PreviewImageQauality = "50",
            CssNamespace = "default",
            InitialItemDimensions = new PositionalContentConfigurationInitialDimension() {
                Top = 5,
                Left = 5,
                Width = 15,
                Height = 10,
                HeightAuto = true
            }
        };

        public override PositionalContentConfiguration FromConfigurationEditor(IDictionary<string, object> editorValues, PositionalContentConfiguration configuration)
        {
            return base.FromConfigurationEditor(editorValues, configuration);
        }
    }
}
