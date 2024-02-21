using Umbraco.Cms.Core.Composing;

namespace Limbo.Umbraco.BlockList.Converters;

internal sealed class BlockListTypeConverterCollectionBuilder : LazyCollectionBuilderBase<BlockListTypeConverterCollectionBuilder, BlockListTypeConverterCollection, IBlockListTypeConverter> {

    protected override BlockListTypeConverterCollectionBuilder This => this;

}