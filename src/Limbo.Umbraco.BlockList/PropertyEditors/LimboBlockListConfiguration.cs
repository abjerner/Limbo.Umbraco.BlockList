using System.Runtime.Serialization;
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
    public string? TypeConverter { get; set; }

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

}