// [CHANGE: Umbraco 17 upgrade] Related: see documentation/umbraco-17-upgrade.md

using System;
using System.Diagnostics.CodeAnalysis;
using Limbo.Umbraco.BlockList.Converters;
using Umbraco.Cms.Core.DeliveryApi;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

#pragma warning disable 1591

namespace Limbo.Umbraco.BlockList.PropertyEditors;

public class LimboBlockListPropertyValueConverter : BlockListPropertyValueConverter {

    private readonly BlockListTypeConverterCollection _converterCollection;

    #region Constructors

    public LimboBlockListPropertyValueConverter(IProfilingLogger proflog, BlockEditorConverter blockConverter,
        BlockListTypeConverterCollection converterCollection, IContentTypeService contentTypeService,
        IApiElementBuilder apiElementBuilder, IJsonSerializer jsonSerializer,
        BlockListPropertyValueConstructorCache constructorCache, IVariationContextAccessor variationContextAccessor,
        BlockEditorVarianceHandler blockEditorVarianceHandler)
        : base(proflog, blockConverter, contentTypeService, apiElementBuilder, jsonSerializer, constructorCache,
            variationContextAccessor, blockEditorVarianceHandler) {
        _converterCollection = converterCollection;
    }

    #endregion

    #region Member methods

    private bool TryGetConverter(LimboBlockListConfiguration config, [NotNullWhen(true)] out IBlockListTypeConverter? converter) {
        converter = null;
        return !string.IsNullOrWhiteSpace(config.TypeConverter?.Type) && _converterCollection.TryGet(config.TypeConverter.Type, out converter);
    }

    private static LimboBlockListConfiguration? GetConfiguration(IPublishedPropertyType propertyType) {
        return propertyType.DataType.ConfigurationObject as LimboBlockListConfiguration;
    }

    /// <inheritdoc />
    public override bool IsConverter(IPublishedPropertyType propertyType) {
        return propertyType.EditorAlias.InvariantEquals(LimboBlockListPropertyEditor.EditorAlias);
    }

    /// <inheritdoc />
    public override Type GetPropertyValueType(IPublishedPropertyType propertyType) {

        // Call the base value converter if the config isn't the right type
        if (GetConfiguration(propertyType) is not { } config) return base.GetPropertyValueType(propertyType);

        // Look up the selected converter and get it's desired type (or fall back to the base value converter)
        return TryGetConverter(config, out IBlockListTypeConverter? converter) ? converter.GetType(propertyType, config) : base.GetPropertyValueType(propertyType);

    }

    /// <inheritdoc />
    public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType) {

        // Default to "Elements" if configuration doesn't match (probably wouldn't happen)
        if (GetConfiguration(propertyType) is not { } config) return PropertyCacheLevel.Elements;

        // Return the configured cache level (or "Elements" if not specified)
        return config.CacheLevel ?? PropertyCacheLevel.Elements;

    }

    /// <inheritdoc />
    public override object? ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object? inter, bool preview) {

        // Get the value as returned by Umbraco's value converter
        object? value = base.ConvertIntermediateToObject(owner, propertyType, referenceCacheLevel, inter, preview);

        // Return "value" if the data type isn't configured with an item converter
        if (GetConfiguration(propertyType) is not { } config) return value;

        // Return "value" if item converter wasn't found, otherwise call the converter
        if (!TryGetConverter(config, out IBlockListTypeConverter? converter)) return value;

        // If value is a "BlockListItem" (with single block mode activated), we call "ConvertItem" instead
        if (value is BlockListItem item) return converter.ConvertItem(owner, propertyType, item, config);

        // For anything else (even if null), we call "Convert"
        return converter.Convert(owner, propertyType, value as BlockListModel, config);

    }

    #endregion

}