using Skybrud.Essentials.Umbraco.Manifests.Extensions;

namespace Limbo.Umbraco.BlockList.Manifests.Extensions;

internal class ClipboardPastePropertyValueTranslator : IExtension {

    public string Type => "clipboardPastePropertyValueTranslator";

    public required string Alias { get; set; }

    public required string Name { get; set; }

    public required string Api { get; set; }

    public required string FromClipboardEntryValueType { get; set; }

    public required string ToPropertyEditorUi { get; set; }

}