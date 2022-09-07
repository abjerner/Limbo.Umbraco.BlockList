using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

namespace Limbo.Umbraco.BlockList.PropertyEditors {

    internal class LimboBlockListConfigurationEditor : ConfigurationEditor<LimboBlockListConfiguration> {

        public LimboBlockListConfigurationEditor(IIOHelper ioHelper, IEditorConfigurationParser editorConfigurationParser) : base(ioHelper, editorConfigurationParser) { }

    }

}