using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.BlockList.PropertyEditors {

    internal class LimboBlockListConfigurationEditor : ConfigurationEditor<LimboBlockListConfiguration> {

        public LimboBlockListConfigurationEditor(IIOHelper ioHelper) : base(ioHelper) { }

    }

}