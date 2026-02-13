using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.PropertyEditors;

#pragma warning disable CS1591

namespace Limbo.Umbraco.BlockList.NotificationHandlers;

public class LimboBlockListPropertyNotificationHandler : BlockEditorPropertyNotificationHandlerBase<BlockListLayoutItem> {

    public LimboBlockListPropertyNotificationHandler(ILogger<BlockListPropertyNotificationHandler> logger) : base(logger) { }

    protected override string EditorAlias => PropertyEditors.LimboBlockListPropertyEditor.EditorAlias;

}