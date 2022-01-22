using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

#pragma warning disable 1591

namespace Limbo.Umbraco.BlockList.PropertyEditors {

    /// <summary>
    /// Represents a block list property editor.
    /// </summary>
    [DataEditor(EditorAlias, EditorName, EditorView, ValueType = ValueTypes.Json, Group = "Limbo", Icon = EditorIcon)]
    public class LimboBlockListPropertyEditor : BlockEditorPropertyEditor {

        private readonly IIOHelper _ioHelper;

        #region Constants

        internal const string EditorAlias = "Limbo.Umbraco.BlockList";

        internal const string EditorName = "Limbo Block List";

        internal const string EditorView = "blocklist";

        internal const string EditorIcon = "icon-thumbnail-list color-limbo";

        #endregion

        #region Constructors

        public LimboBlockListPropertyEditor(IDataValueEditorFactory dataValueEditorFactory, PropertyEditorCollection propertyEditors, IIOHelper ioHelper) : base(dataValueEditorFactory, propertyEditors) {
            _ioHelper = ioHelper;
        }

        #endregion

        #region Member methods

        protected override IConfigurationEditor CreateConfigurationEditor() => new LimboBlockListConfigurationEditor(_ioHelper);

        #endregion

    }

}