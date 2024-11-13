using System.Runtime.Serialization;
using Limbo.Umbraco.BlockList.Converters;
using Limbo.Umbraco.BlockList.Models;
using Newtonsoft.Json.Linq;
using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.BlockList.PropertyEditors;

/// <summary>
/// Class representing the configuration of <see cref="LimboBlockListPropertyEditor"/> data type.
/// </summary>
public class LimboBlockListConfiguration : BlockListConfiguration {

    /// <summary>
    /// Gets a reference to a <see cref="JObject"/> with information about the selected type converter.
    /// </summary>
    [ConfigurationField("typeConverter", "Type Converter", "/App_Plugins/Limbo.Umbraco.BlockList/TypeConverter.html", Description = "Select a type converter.")]
    public BlockListTypeConverter? TypeConverter { get; set; }

    /// <summary>
    /// Gets or sets the property cache level of the underlying property value converter. Defaults to <see cref="PropertyCacheLevel.Elements"/> if not specified.
    /// </summary>
    [ConfigurationField("cacheLevel", "Cache Level", "/App_Plugins/Limbo.Umbraco.BlockList/CacheLevel.html", Description = "Select the cache level of the underlying property value converter.")]
    public PropertyCacheLevel? CacheLevel { get; set; }

    /// <summary>
    /// Gets whether the block list editor is configured as a single picker (if max blocks is set to <c>1</c>).
    /// </summary>
    [IgnoreDataMember]
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