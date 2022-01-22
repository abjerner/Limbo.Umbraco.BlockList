using System.Web.Http.Filters;
using Umbraco.Core.Composing;
using Umbraco.Web.Editors;
using Umbraco.Web.Models.ContentEditing;

#pragma warning disable 1591

namespace Limbo.Umbraco.BlockList.Components {
    
    public class BlockListComponent : IComponent {
        
        public void Initialize() {
            EditorModelEventManager.SendingContentModel += EditorModelEventManagerOnSendingContentModel;
        }

        public void Terminate() {
            EditorModelEventManager.SendingContentModel -= EditorModelEventManagerOnSendingContentModel;
        }
        
        private void EditorModelEventManagerOnSendingContentModel(HttpActionExecutedContext sender, EditorModelEventArgs<ContentItemDisplay> e) {
            
            foreach (var variant in e.Model.Variants) {

                foreach (var tab in variant.Tabs) {

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

}