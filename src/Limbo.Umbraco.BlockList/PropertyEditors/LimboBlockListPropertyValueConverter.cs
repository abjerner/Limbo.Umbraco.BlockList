using Limbo.Umbraco.BlockList.Converters;
using Skybrud.Essentials.Json.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Extensions;

#pragma warning disable 1591

namespace Limbo.Umbraco.BlockList.PropertyEditors {

    public class LimboBlockListPropertyValueConverter : BlockListPropertyValueConverter {

        private readonly BlockListTypeConverterCollection _converterCollection;

        #region Constructors

        public LimboBlockListPropertyValueConverter(IProfilingLogger proflog, BlockEditorConverter blockConverter,
            BlockListTypeConverterCollection converterCollection) : base(proflog, blockConverter) {
            _converterCollection = converterCollection;
        }

        #endregion

        #region Member methods

        private bool TryGetConverter(LimboBlockListConfiguration config, [NotNullWhen(true)] out IBlockListTypeConverter? converter) {
            converter = null;
            return !string.IsNullOrWhiteSpace(config.TypeConverter) && _converterCollection.TryGet(config.TypeConverter, out converter);
        }

        /// <inheritdoc />
        public override bool IsConverter(IPublishedPropertyType propertyType) {
            return propertyType.EditorAlias.InvariantEquals(LimboBlockListPropertyEditor.EditorAlias);
        }

        /// <inheritdoc />
        public override Type GetPropertyValueType(IPublishedPropertyType propertyType) {

            // Call the base value converter if the config isn't the right type
            if (propertyType.DataType.Configuration is not LimboBlockListConfiguration config) return base.GetPropertyValueType(propertyType);

            // Look up the selected converter and get it's desired type (or fall back to the base value converter)
            return TryGetConverter(config, out IBlockListTypeConverter? converter) ? converter.GetType(propertyType, config) : base.GetPropertyValueType(propertyType);

        }

        /// <inheritdoc />
        public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType) {
            return PropertyCacheLevel.Elements;
        }

        /// <inheritdoc />
        public override object? ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object? inter, bool preview) {

            // Get the BlockListModel value from the parent value converter
            BlockListModel? value = base.ConvertIntermediateToObject(owner, propertyType, referenceCacheLevel, inter, preview) as BlockListModel;

            // Return "value" if the data type isn't configured with an item converter
            if (propertyType.DataType.Configuration is not LimboBlockListConfiguration config) return value;

            // Return "value" if item converter wasn't found, otherwise call the converter
            return TryGetConverter(config, out IBlockListTypeConverter? converter) ? converter.Convert(owner, propertyType, value, config) : value;

        }

        #endregion

    }

}