using Umbraco.Cms.Core.PropertyEditors;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Limbo.Umbraco.BlockList.PropertyEditors;

/// <remarks>
/// With the new backoffice, <see cref="BlockListConfiguration.BlockConfiguration"/> now only contains properties for
/// the settings defined by the block list schema, whereas settings now defined by block list UI are now omitted.
///
/// But as we still need those properties in order to set the block list configuration programmatically, this class
/// does exactly that.
/// </remarks>
public class LimboBlockConfiguration : BlockListConfiguration.BlockConfiguration {

    public string? BackgroundColor { get; set; }

    public string? IconColor { get; set; }

    public string? Thumbnail { get; set; }

    public string? View { get; set; }

    public string? Stylesheet { get; set; }

    public string? Label { get; set; }

    public string? EditorSize { get; set; }

    public bool ForceHideContentEditorInOverlay { get; set; }

    public LimboBlockConfiguration() { }

    public LimboBlockConfiguration(BlockListConfiguration.BlockConfiguration existing) {
        ContentElementTypeKey = existing.ContentElementTypeKey;
        SettingsElementTypeKey = existing.SettingsElementTypeKey;
        if (existing is not LimboBlockConfiguration limbo) return;
        BackgroundColor = limbo.BackgroundColor;
        IconColor = limbo.IconColor;
        Thumbnail = limbo.Thumbnail;
        View = limbo.View;
        Stylesheet = limbo.Stylesheet;
        Label = limbo.Label;
        EditorSize = limbo.EditorSize;
        ForceHideContentEditorInOverlay = limbo.ForceHideContentEditorInOverlay;
    }

    public LimboBlockConfiguration(LimboBlockConfiguration existing) {
        ContentElementTypeKey = existing.ContentElementTypeKey;
        SettingsElementTypeKey = existing.SettingsElementTypeKey;
        BackgroundColor = existing.BackgroundColor;
        IconColor = existing.IconColor;
        Thumbnail = existing.Thumbnail;
        View = existing.View;
        Stylesheet = existing.Stylesheet;
        Label = existing.Label;
        EditorSize = existing.EditorSize;
        ForceHideContentEditorInOverlay = existing.ForceHideContentEditorInOverlay;
    }

}