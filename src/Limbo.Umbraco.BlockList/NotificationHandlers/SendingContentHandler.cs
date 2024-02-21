using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

#pragma warning disable 1591

namespace Limbo.Umbraco.BlockList.NotificationHandlers;

public class SendingContentHandler : INotificationHandler<SendingContentNotification> {

    public void Handle(SendingContentNotification notification) {

        foreach (var variant in notification.Content.Variants) {

            foreach (var tab in variant.Tabs) {

                if (tab.Properties is null) continue;

                foreach (var property in tab.Properties) {

                    // See: https://github.com/umbraco/Umbraco-CMS/blob/v9/contrib/src/Umbraco.Web.UI.Client/src/views/propertyeditors/blocklist/umbBlockListPropertyEditor.component.js#L142
                    // As the <umbBlockListPropertyEditor /> component relies on the property editor alias being
                    // "Umbraco.BlockList", we need to change our own property editor alias to "Umbraco.BlockList"
                    // to trick Umbraco into thinking the property editor is Umbraco's own (╯°□°)╯︵ ┻━┻
                    if (property.Editor == "Limbo.Umbraco.BlockList") property.Editor = "Umbraco.BlockList";

                }

            }

        }

    }

}