using Skybrud.Essentials.Umbraco.Manifests.Extensions;

namespace Limbo.Umbraco.BlockList.Manifests.Extensions;

internal class ClipboardCopyPropertyValueTranslator : IExtension {

    public string Type => "clipboardCopyPropertyValueTranslator";

    public required string Alias { get; set; }

    public required string Name { get; set; }

    public required string Api { get; set; }

    public required string FromPropertyEditorUi { get; set; }

    public required string ToClipboardEntryValueType { get; set; }

}