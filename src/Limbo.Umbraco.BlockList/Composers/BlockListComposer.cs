// [CHANGE: Umbraco 17 upgrade] Related: see documentation/umbraco-17-upgrade.md

using Limbo.Umbraco.BlockList.Converters;
using Limbo.Umbraco.BlockList.NotificationHandlers;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Notifications;

#pragma warning disable 1591

namespace Limbo.Umbraco.BlockList.Composers;

public sealed class BlockListComposer : IComposer {

    public void Compose(IUmbracoBuilder builder) {

        builder
            .WithCollectionBuilder<BlockListTypeConverterCollectionBuilder>()
            .Add(() => builder.TypeLoader.GetTypes<IBlockListTypeConverter>());

        // Register misc notification handlers to let Umbraco do its thing when saving, copying or scaffolding content
        // with Limbo Block List properties. For one, this ensures that blocks are given new GUID keys when copying content
        builder
            .AddNotificationHandler<ContentSavingNotification, LimboBlockListPropertyNotificationHandler>()
            .AddNotificationHandler<ContentCopyingNotification, LimboBlockListPropertyNotificationHandler>()
            .AddNotificationHandler<ContentScaffoldedNotification, LimboBlockListPropertyNotificationHandler>();

        // Umbraco 14 removed "IManifestFilter" along with the AngularJS backoffice. The client side part of the
        // package is now declared in "wwwroot/umbraco-package.json", which Umbraco discovers on its own via the
        // static web assets of this project (App_Plugins/Limbo.Umbraco.BlockList).

        // The former "SendingContentHandler" is gone as well. It only existed to rewrite our editor alias to
        // "Umbraco.BlockList" so the AngularJS block list component would render. The new backoffice resolves the
        // editing experience through the property editor UI alias instead, so no rewriting is needed.

    }

}
