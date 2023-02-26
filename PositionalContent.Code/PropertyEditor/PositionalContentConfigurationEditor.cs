using System.Collections.Generic;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

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
        

        public PositionalContentConfigurationEditor(IIOHelper ioHelper, IEditorConfigurationParser editorConfigurationParser) : base(ioHelper, editorConfigurationParser)
        {
        }
    }
}
