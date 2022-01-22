using Limbo.Umbraco.BlockList.Converters;
using Skybrud.Essentials.Json.Extensions;
using System;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models.Blocks;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.PropertyEditors.ValueConverters;

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

        private bool TryGetConverter(LimboBlockListConfiguration config, out IBlockListTypeConverter converter) {
            converter = null;
            string key = config.TypeConverter?.GetString("key");
            return !string.IsNullOrWhiteSpace(key) && _converterCollection.TryGet(key, out converter);
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
            return TryGetConverter(config, out IBlockListTypeConverter converter) ? converter.GetType(propertyType, config) : base.GetPropertyValueType(propertyType);

        }

        /// <inheritdoc />
        public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType) {
            return PropertyCacheLevel.Elements;
        }

        /// <inheritdoc />
        public override object ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object inter, bool preview) {

            // Get the BlockListModel value from the parent value converter
            BlockListModel value = base.ConvertIntermediateToObject(owner, propertyType, referenceCacheLevel, inter, preview) as BlockListModel;

            // Return "value" if the data type isn't configured with an item converter
            if (propertyType.DataType.Configuration is not LimboBlockListConfiguration config) return value;

            // Return "value" if item converter wasn't found, otherwise call the converter
            return TryGetConverter(config, out IBlockListTypeConverter converter) ? converter.Convert(owner, propertyType, value, config) : value;

        }

        #endregion

    }

}