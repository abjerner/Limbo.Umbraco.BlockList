using Limbo.Umbraco.BlockList.Converters;
using Limbo.Umbraco.BlockList.Manifests;
using Limbo.Umbraco.BlockList.NotificationHandlers;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Notifications;

#pragma warning disable 1591

namespace Limbo.Umbraco.BlockList.Composers {

    public sealed class BlockListComposer : IComposer {

        public void Compose(IUmbracoBuilder builder) {

            builder
                .AddNotificationHandler<SendingContentNotification, SendingContentHandler>()
                .WithCollectionBuilder<BlockListTypeConverterCollectionBuilder>()
                .Add(() => builder.TypeLoader.GetTypes<IBlockListTypeConverter>());

            builder
                .ManifestFilters()
                .Append<BlockListManifestFilter>();

        }

    }

}