using Limbo.Umbraco.BlockList.Converters;
using Limbo.Umbraco.BlockList.NotificationHandlers;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.WebAssets;

#pragma warning disable 1591

namespace Limbo.Umbraco.BlockList.Composers {

    public sealed class BlockListComposer : IComposer {

        public void Compose(IUmbracoBuilder builder) {
            
            builder
                .AddNotificationHandler<SendingContentNotification, SendingContentHandler>()
                .WithCollectionBuilder<BlockListTypeConverterCollectionBuilder>()
                .Add(() => builder.TypeLoader.GetTypes<IBlockListTypeConverter>());

            builder
                .BackOfficeAssets()
                .Append<BackOfficeJavaScriptAsset>();

        }

        public class BackOfficeJavaScriptAsset : IAssetFile {

            public AssetType DependencyType => AssetType.Javascript;

            public string FilePath { get; set; }

            public BackOfficeJavaScriptAsset() {
                FilePath = "/App_Plugins/Limbo.Umbraco.BlockList/TypeConverter.js";
            }

        }

    }

}