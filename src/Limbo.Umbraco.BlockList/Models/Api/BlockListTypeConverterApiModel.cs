using System;
using Limbo.Umbraco.BlockList.Converters;
using Newtonsoft.Json;

namespace Limbo.Umbraco.BlockList.Models.Api;

internal class BlockListTypeConverterApiModel {

    private readonly IBlockListTypeConverter _converter;
    private readonly Type _type;

    [JsonProperty("assembly")]
    public string Assembly => _type.Assembly.FullName ?? string.Empty;

    [JsonProperty("type")]
    public string Type => BlockListUtils.GetTypeAlias(_type);

    [JsonProperty("icon")]
    public string Icon => _converter.Icon ?? $"icon-box color-{_type.Assembly.FullName?.Split('.')[0].ToLower()}";

    [JsonProperty("name")]
    public string Name => _converter.Name;

    [JsonProperty("description")]
    public string Description => $"{BlockListUtils.RemoveVersion(_type.AssemblyQualifiedName)}.dll";

    public BlockListTypeConverterApiModel(IBlockListTypeConverter converter) {
        _converter = converter;
        _type = converter.GetType();
    }

}