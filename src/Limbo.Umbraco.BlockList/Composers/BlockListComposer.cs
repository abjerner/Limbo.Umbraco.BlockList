using Limbo.Umbraco.BlockList.Components;
using Limbo.Umbraco.BlockList.Converters;
using Umbraco.Core;
using Umbraco.Core.Composing;

#pragma warning disable 1591

namespace Limbo.Umbraco.BlockList.Composers {

    internal static class BlockListCompositionExtensions {
        
        public static BlockListTypeConverterCollectionBuilder BlockListTypeConverters(this Composition composition) {
            return composition.WithCollectionBuilder<BlockListTypeConverterCollectionBuilder>();
        }

    }

    public sealed class BlockListComposer : IUserComposer {
        
        public void Compose(Composition composition) {

            composition
                .RegisterUnique<BlockListTypeConverterCollection>();

            composition
                .BlockListTypeConverters()
                .Add(() => composition.TypeLoader.GetTypes<IBlockListTypeConverter>());

            composition
                .Components()
                .Append<BlockListComponent>();

        }

    }

}