using System.Diagnostics.CodeAnalysis;
using Limbo.Umbraco.BlockList.Converters;
using Limbo.Umbraco.BlockList.Json.Newtonsoft;
using Newtonsoft.Json;

namespace Limbo.Umbraco.BlockList.Models;

/// <summary>
/// Class describing a selected converter.
/// </summary>
[JsonConverter(typeof(BlockListJsonConverter))]
public class BlockListTypeConverter {

    /// <summary>
    /// Gets or sets the alias of the CLR type of the type converter.
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// Initializes a new instance with the specified <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The alias of the CLR type of the type converter.</param>
    [SetsRequiredMembers]
    public BlockListTypeConverter(string type) {
        Type = type;
    }

    /// <summary>
    /// Creates a new instance based on the specified <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the converter.</typeparam>
    /// <returns>An instance of <see cref="BlockListTypeConverter"/>.</returns>
    public static BlockListTypeConverter Create<T>() where T : IBlockListTypeConverter {
        return new BlockListTypeConverter(BlockListUtils.GetTypeAlias(typeof(T)));
    }

}