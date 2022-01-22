using ClientDependency.Core;
using System;
using Umbraco.Core.Logging;
using Umbraco.Core.PropertyEditors;
using Umbraco.Core.Services;
using Umbraco.Web.PropertyEditors;

#pragma warning disable 1591

namespace Limbo.Umbraco.BlockList.PropertyEditors {

    /// <summary>
    /// Represents a block list property editor.
    /// </summary>
    [DataEditor(EditorAlias, EditorName, EditorView, ValueType = ValueTypes.Json, Group = "Limbo", Icon = EditorIcon)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "/App_Plugins/Limbo.Umbraco.BlockList/TypeConverter.js")]
    public class LimboBlockListPropertyEditor : BlockEditorPropertyEditor {

        #region Constants

        internal const string EditorAlias = "Limbo.Umbraco.BlockList";

        internal const string EditorName = "Limbo Block List";

        internal const string EditorView = "blocklist";

        internal const string EditorIcon = "icon-thumbnail-list color-limbo";

        #endregion

        #region Constructors

        public LimboBlockListPropertyEditor(ILogger logger, Lazy<PropertyEditorCollection> propertyEditors,
            IDataTypeService dataTypeService, IContentTypeService contentTypeService, ILocalizedTextService localizedTextService)
            : base(logger, propertyEditors, dataTypeService, contentTypeService, localizedTextService)
        { }

        #endregion

        #region Member methods

        protected override IConfigurationEditor CreateConfigurationEditor() => new LimboBlockListConfigurationEditor();

        #endregion

    }

}