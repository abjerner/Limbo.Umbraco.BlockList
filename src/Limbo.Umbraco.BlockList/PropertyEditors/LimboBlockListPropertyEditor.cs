// [CHANGE: Umbraco 17 upgrade] Related: see documentation/umbraco-17-upgrade.md

using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;

#pragma warning disable 1591

namespace Limbo.Umbraco.BlockList.PropertyEditors;

/// <summary>
/// Represents a block list property editor.
/// </summary>
/// <remarks>
/// The editor derives directly from Umbraco's own <see cref="BlockListPropertyEditor"/> rather than from
/// <see cref="BlockListPropertyEditorBase"/>, as the variant merging logic relies on internal types and therefore
/// can't be re-implemented outside of Umbraco.
/// </remarks>
[DataEditor(EditorAlias, ValueType = ValueTypes.Json, ValueEditorIsReusable = false)]
public class LimboBlockListPropertyEditor : BlockListPropertyEditor {

    private readonly IIOHelper _ioHelper;

    #region Constants

    public const string EditorAlias = "Limbo.Umbraco.BlockList";

    public const string EditorName = "Limbo Block List";

    public const string EditorUiAlias = "Limbo.PropertyEditorUi.BlockList";

    public const string EditorIcon = "icon-thumbnail-list";

    #endregion

    #region Constructors

    public LimboBlockListPropertyEditor(IDataValueEditorFactory dataValueEditorFactory, IIOHelper ioHelper,
        IBlockValuePropertyIndexValueFactory blockValuePropertyIndexValueFactory, IJsonSerializer jsonSerializer)
        : base(dataValueEditorFactory, ioHelper, blockValuePropertyIndexValueFactory, jsonSerializer) {
        _ioHelper = ioHelper;
    }

    #endregion

    #region Member methods

    protected override IConfigurationEditor CreateConfigurationEditor() => new LimboBlockListConfigurationEditor(_ioHelper);

    #endregion

}