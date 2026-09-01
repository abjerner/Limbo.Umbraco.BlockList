// [CHANGE: Umbraco 17 upgrade] Related: see documentation/umbraco-17-upgrade.md

using System;
using System.Collections.Generic;
using Limbo.Umbraco.BlockList.Converters;
using Limbo.Umbraco.BlockList.Models;
using Limbo.Umbraco.BlockList.PropertyEditors;
using Umbraco.Cms.Core.PropertyEditors;
using static Umbraco.Cms.Core.PropertyEditors.BlockListConfiguration;

namespace Limbo.Umbraco.BlockList.Extensions;

/// <summary>
/// Static class with various extension methods related to this package.
/// </summary>
public static class BlockListExtensions {

    /// <summary>
    /// Sets the value of the <see cref="LimboBlockListConfiguration.TypeConverter"/> property.
    /// </summary>
    /// <typeparam name="TConfig">The type of the configuration.</typeparam>
    /// <typeparam name="TConverter">The type of the converter.</typeparam>
    /// <param name="config">The configuration to change.</param>
    /// <returns>The <typeparamref name="TConfig"/> instance - useful for method chaining.</returns>
    public static TConfig SetTypeConverter<TConfig, TConverter>(this TConfig config) where TConfig : LimboBlockListConfiguration where TConverter : IBlockListTypeConverter {
        config.TypeConverter = BlockListTypeConverter.Create<TConverter>();
        return config;
    }

    /// <summary>
    /// Sets the value of the <see cref="LimboBlockListConfiguration.TypeConverter"/> property.
    /// </summary>
    /// <typeparam name="TConfig">The type of the configuration.</typeparam>
    /// <param name="config">The configuration to change.</param>
    /// <param name="converter">The converter to be used.</param>
    /// <returns>The <typeparamref name="TConfig"/> instance - useful for method chaining.</returns>
    public static TConfig SetTypeConverter<TConfig>(this TConfig config, BlockListTypeConverter? converter) where TConfig : LimboBlockListConfiguration {
        config.TypeConverter = converter;
        return config;
    }

    /// <summary>
    /// Sets the value of the <see cref="LimboBlockListConfiguration.CacheLevel"/> property.
    /// </summary>
    /// <typeparam name="TConfig">The type of the configuration.</typeparam>
    /// <param name="config">The configuration to change.</param>
    /// <param name="value">The new value.</param>
    /// <returns>The <typeparamref name="TConfig"/> instance - useful for method chaining.</returns>
    public static TConfig SetCacheLevel<TConfig>(this TConfig config, PropertyCacheLevel value) where TConfig : LimboBlockListConfiguration {
        config.CacheLevel = value;
        return config;
    }

    /// <summary>
    /// Sets the value of the <see cref="BlockListConfiguration.Blocks"/> property.
    /// </summary>
    /// <typeparam name="TConfig">The type of the configuration.</typeparam>
    /// <param name="config">The configuration to change.</param>
    /// <param name="value">The new value.</param>
    /// <returns>The <typeparamref name="TConfig"/> instance - useful for method chaining.</returns>
    public static TConfig SetBlocks<TConfig>(this TConfig config, BlockConfiguration[] value) where TConfig : LimboBlockListConfiguration {
        config.Blocks = value;
        return config;
    }

    /// <summary>
    /// Sets the value of the <see cref="BlockListConfiguration.Blocks"/> property.
    /// </summary>
    /// <typeparam name="TConfig">The type of the configuration.</typeparam>
    /// <param name="config">The configuration to change.</param>
    /// <param name="value">The new value.</param>
    /// <returns>The <typeparamref name="TConfig"/> instance - useful for method chaining.</returns>
    public static TConfig SetBlocks<TConfig>(this TConfig config, IEnumerable<BlockConfiguration> value) where TConfig : LimboBlockListConfiguration {
        config.Blocks = [..value];
        return config;
    }

    /// <summary>
    /// Sets the value of the <see cref="BlockListConfiguration.ValidationLimit"/> property.
    /// </summary>
    /// <typeparam name="TConfig">The type of the configuration.</typeparam>
    /// <param name="config">The configuration to change.</param>
    /// <param name="value">The new value.</param>
    /// <returns>The <typeparamref name="TConfig"/> instance - useful for method chaining.</returns>
    public static TConfig SetValidationLimit<TConfig>(this TConfig config, NumberRange value) where TConfig : LimboBlockListConfiguration {
        config.ValidationLimit = value;
        return config;
    }

    /// <summary>
    /// Sets the value of the <see cref="BlockListConfiguration.ValidationLimit"/> property.
    /// </summary>
    /// <typeparam name="TConfig">The type of the configuration.</typeparam>
    /// <param name="config">The configuration to change.</param>
    /// <param name="min">The minimum amount of allowed blocks.</param>
    /// <param name="max">The maximum amount of allowed blocks.</param>
    /// <returns>The <typeparamref name="TConfig"/> instance - useful for method chaining.</returns>
    public static TConfig SetValidationLimit<TConfig>(this TConfig config, int? min, int? max) where TConfig : LimboBlockListConfiguration {
        config.ValidationLimit = new NumberRange { Min = min, Max = max };
        return config;
    }

    /// <summary>
    /// Sets the value of the <see cref="BlockListConfiguration.UseSingleBlockMode"/> property.
    /// </summary>
    /// <typeparam name="TConfig">The type of the configuration.</typeparam>
    /// <param name="config">The configuration to change.</param>
    /// <param name="value">The new value.</param>
    /// <returns>The <typeparamref name="TConfig"/> instance - useful for method chaining.</returns>
    [Obsolete("Umbraco deprecated single block mode in favor of the dedicated 'Single Block' property editor.")]
    public static TConfig SetUseSingleBlockMode<TConfig>(this TConfig config, bool value) where TConfig : LimboBlockListConfiguration {
#pragma warning disable CS0618
        config.UseSingleBlockMode = value;
#pragma warning restore CS0618
        return config;
    }

    /// <summary>
    /// Sets the value of the <see cref="LimboBlockListConfiguration.MaxPropertyWidth"/> property.
    /// </summary>
    /// <typeparam name="TConfig">The type of the configuration.</typeparam>
    /// <param name="config">The configuration to change.</param>
    /// <param name="value">The new value - e.g. <c>800px</c> or <c>100%</c>.</param>
    /// <returns>The <typeparamref name="TConfig"/> instance - useful for method chaining.</returns>
    public static TConfig SetMaxPropertyWidth<TConfig>(this TConfig config, string? value) where TConfig : LimboBlockListConfiguration {
        config.MaxPropertyWidth = value;
        return config;
    }

    /// <summary>
    /// Sets the value of the <see cref="LimboBlockListConfiguration.UseLiveEditing"/> property.
    /// </summary>
    /// <typeparam name="TConfig">The type of the configuration.</typeparam>
    /// <param name="config">The configuration to change.</param>
    /// <param name="value">The new value.</param>
    /// <returns>The <typeparamref name="TConfig"/> instance - useful for method chaining.</returns>
    public static TConfig UseLiveEditing<TConfig>(this TConfig config, bool value) where TConfig : LimboBlockListConfiguration {
        config.UseLiveEditing = value;
        return config;
    }

    /// <summary>
    /// Sets the value of the <see cref="LimboBlockListConfiguration.UseInlineEditingAsDefault"/> property.
    /// </summary>
    /// <typeparam name="TConfig">The type of the configuration.</typeparam>
    /// <param name="config">The configuration to change.</param>
    /// <param name="value">The new value.</param>
    /// <returns>The <typeparamref name="TConfig"/> instance - useful for method chaining.</returns>
    public static TConfig UseInlineEditingAsDefault<TConfig>(this TConfig config, bool value) where TConfig : LimboBlockListConfiguration {
        config.UseInlineEditingAsDefault = value;
        return config;
    }

}