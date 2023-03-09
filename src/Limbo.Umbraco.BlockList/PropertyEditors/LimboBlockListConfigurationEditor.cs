using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

namespace Limbo.Umbraco.BlockList.PropertyEditors {

    internal class LimboBlockListConfigurationEditor : ConfigurationEditor<LimboBlockListConfiguration> {

        public LimboBlockListConfigurationEditor(IIOHelper ioHelper, IEditorConfigurationParser editorConfigurationParser) : base(ioHelper, editorConfigurationParser) {

            foreach (var field in Fields) {

                if (field.View is not null) field.View = field.View.Replace("{version}", BlockListPackage.InformationalVersion);

                switch (field.Key) {

                    case "typeConverter":
                        BlockListUtils.AppendLinkToDescription(field, "See the documentation &rarr;", "https://packages.limbo.works/6b6ffcbf");
                        break;

                }

            }

        }

    }

}