// [CHANGE: Umbraco 17 upgrade] Related: see documentation/umbraco-17-upgrade.md

using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.BlockList.PropertyEditors;

/// <remarks>
/// As of Umbraco 14, a configuration field no longer carries a name, description or view - the editing experience for
/// each field is declared client side instead. See <c>wwwroot/umbraco-package.json</c>.
/// </remarks>
internal sealed class LimboBlockListConfigurationEditor : ConfigurationEditor<LimboBlockListConfiguration> {

    public LimboBlockListConfigurationEditor(IIOHelper ioHelper) : base(ioHelper) { }

}
