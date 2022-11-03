using Limbo.Umbraco.BlockList.PropertyEditors;
using System;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Limbo.Umbraco.BlockList.Converters {

    /// <summary>
    /// Interface describing a type converter for <see cref="BlockListModel"/>.
    /// </summary>
    public interface IBlockListTypeConverter {

        /// <summary>
        /// Gets the name of the converter.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the name of the converter.
        /// </summary>
        public string? Icon => null;

        /// <summary>
        /// Returns the CLR type for this type converter.
        /// </summary>
        /// <param name="propertyType">The property type.</param>
        /// <param name="config">The configuration of the parent data type.</param>
        /// <returns>An instance of <see cref="Type"/>.</returns>
        Type GetType(IPublishedPropertyType propertyType, LimboBlockListConfiguration config);

        /// <summary>
        /// Converts the <see cref="BlockListModel"/> <paramref name="source"/> value to the desired type.
        /// </summary>
        /// <param name="owner">The <see cref="IPublishedElement"/> holding the property with the block list.</param>
        /// <param name="propertyType">The property type.</param>
        /// <param name="source">The <see cref="BlockListModel"/> value to be converted.</param>
        /// <param name="config">The configuration of the parent data type.</param>
        /// <returns>The desired output value based on the <see cref="BlockListModel"/>.</returns>
        object Convert(IPublishedElement owner, IPublishedPropertyType propertyType, BlockListModel? source, LimboBlockListConfiguration config);

    }

}