using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

#pragma warning disable 1591

namespace Limbo.Umbraco.BlockList.PropertyEditors {

    /// <summary>
    /// Represents a block list property editor.
    /// </summary>
    [DataEditor(EditorAlias, EditorName, EditorView, ValueType = ValueTypes.Json, Group = "Limbo", Icon = EditorIcon)]
    public class LimboBlockListPropertyEditor : BlockEditorPropertyEditor {

        private readonly IIOHelper _ioHelper;
        private readonly IEditorConfigurationParser _editorConfigurationParser;

        #region Constants

        public const string EditorAlias = "Limbo.Umbraco.BlockList";

        public const string EditorName = "Limbo Block List";

        public const string EditorView = "blocklist";

        public const string EditorIcon = "icon-thumbnail-list color-limbo";

        #endregion

        #region Constructors

        public LimboBlockListPropertyEditor(IDataValueEditorFactory dataValueEditorFactory, PropertyEditorCollection propertyEditors, IIOHelper ioHelper, IEditorConfigurationParser editorConfigurationParser) : base(dataValueEditorFactory, propertyEditors) {
            _ioHelper = ioHelper;
            _editorConfigurationParser = editorConfigurationParser;
        }

        #endregion

        #region Member methods

        protected override IConfigurationEditor CreateConfigurationEditor() => new LimboBlockListConfigurationEditor(_ioHelper, _editorConfigurationParser);

        #endregion

    }

}