// [CHANGE: Umbraco 17 upgrade] Related: see documentation/umbraco-17-upgrade.md

using System;
using System.Text.Json.Serialization;
using Limbo.Umbraco.BlockList.Converters;

namespace Limbo.Umbraco.BlockList.Models.Api;

/// <summary>
/// API model describing an <see cref="IBlockListTypeConverter"/> as returned by the Management API.
/// </summary>
public class BlockListTypeConverterApiModel {

    private readonly IBlockListTypeConverter _converter;
    private readonly Type _type;

    /// <summary>
    /// Gets the full name of the assembly declaring the type converter.
    /// </summary>
    [JsonPropertyName("assembly")]
    public string Assembly => _type.Assembly.FullName ?? string.Empty;

    /// <summary>
    /// Gets the alias (version-less assembly qualified name) of the type converter.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => BlockListUtils.GetTypeAlias(_type);

    /// <summary>
    /// Gets the icon of the type converter.
    /// </summary>
    [JsonPropertyName("icon")]
    public string Icon => _converter.Icon ?? "icon-box";

    /// <summary>
    /// Gets the friendly name of the type converter.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name => _converter.Name;

    /// <summary>
    /// Gets a description of the type converter.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description => BlockListUtils.GetTypeAlias(_type);

    /// <summary>
    /// Initializes a new instance based on the specified <paramref name="converter"/>.
    /// </summary>
    /// <param name="converter">The type converter.</param>
    public BlockListTypeConverterApiModel(IBlockListTypeConverter converter) {
        _converter = converter;
        _type = converter.GetType();
    }

}
