using Limbo.Umbraco.BlockList.PropertyEditors;

namespace Limbo.Umbraco.BlockList.Constants;

/// <summary>
/// Static class with constants for the property editor UI aliases of this package.
/// </summary>
public static class BlockListPropertyEditorUiAliases {

    /// <summary>
    /// The alias of the main block list property editor UI.
    /// </summary>
    public const string BlockList = LimboBlockListPropertyEditor.EditorUiAlias;

    /// <summary>
    /// The alias of the type converter property editor UI.
    /// </summary>
    public const string TypeConverter = "Limbo.Umbraco.BlockList.PropertyEditorUi.TypeConverter";

    /// <summary>
    /// The alias of the cache level property editor UI.
    /// </summary>
    public const string CacheLevel = "Limbo.Umbraco.BlockList.PropertyEditorUi.CacheLevel";

}