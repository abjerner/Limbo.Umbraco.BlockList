// [CHANGE: Umbraco 17 upgrade] Related: see documentation/umbraco-17-upgrade.md

using System.Text.Json.Serialization;
using Limbo.Umbraco.BlockList.Converters;
using Limbo.Umbraco.BlockList.Models;
using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.BlockList.PropertyEditors;

/// <summary>
/// Class representing the configuration of <see cref="LimboBlockListPropertyEditor"/> data type.
/// </summary>
/// <remarks>
/// The label, description and editing experience of each configuration field is declared client side as of Umbraco 14.
/// See the <c>Limbo.Umbraco.BlockList</c> property editor schema in <c>wwwroot/umbraco-package.json</c>.
/// </remarks>
public class LimboBlockListConfiguration : BlockListConfiguration {

    /// <summary>
    /// Gets or sets information about the selected type converter.
    /// </summary>
    [ConfigurationField("typeConverter")]
    public BlockListTypeConverter? TypeConverter { get; set; }

    /// <summary>
    /// Gets or sets the property cache level of the underlying property value converter. Defaults to <see cref="PropertyCacheLevel.Elements"/> if not specified.
    /// </summary>
    [ConfigurationField("cacheLevel")]
    public PropertyCacheLevel? CacheLevel { get; set; }

    /// <summary>
    /// Gets or sets whether the editor should use live editing mode.
    /// </summary>
    /// <remarks>Client side only - Umbraco removed this from its own configuration class in Umbraco 14.</remarks>
    [ConfigurationField("useLiveEditing")]
    public bool UseLiveEditing { get; set; }

    /// <summary>
    /// Gets or sets whether the editor should use the inline editor as the default block view.
    /// </summary>
    /// <remarks>Client side only - Umbraco removed this from its own configuration class in Umbraco 14.</remarks>
    [ConfigurationField("useInlineEditingAsDefault")]
    public bool UseInlineEditingAsDefault { get; set; }

    /// <summary>
    /// Gets or sets an optional CSS width override for the property editor - eg. <c>800px</c> or <c>100%</c>.
    /// </summary>
    /// <remarks>Client side only - Umbraco removed this from its own configuration class in Umbraco 14.</remarks>
    [ConfigurationField("maxPropertyWidth")]
    public string? MaxPropertyWidth { get; set; }

    /// <summary>
    /// Gets whether the block list editor is configured as a single picker (if max blocks is set to <c>1</c>).
    /// </summary>
    [JsonIgnore]
    public bool IsSinglePicker => ValidationLimit.Max == 1;

    /// <summary>
    /// Creates a new <see cref="LimboBlockListConfiguration"/> instance.
    /// </summary>
    /// <returns>A new instance of <see cref="LimboBlockListConfiguration"/>.</returns>
    public static LimboBlockListConfiguration Create() {
        return new LimboBlockListConfiguration();
    }

    /// <summary>
    /// Creates a new <see cref="LimboBlockListConfiguration"/> instance with the specified <typeparamref name="TConverter"/>.
    /// </summary>
    /// <typeparam name="TConverter">The type of the converter.</typeparam>
    /// <returns>A new instance of <see cref="LimboBlockListConfiguration"/>.</returns>
    public static LimboBlockListConfiguration Create<TConverter>() where TConverter : IBlockListTypeConverter {
        return new LimboBlockListConfiguration {
            TypeConverter = BlockListTypeConverter.Create<TConverter>()
        };
    }

}
