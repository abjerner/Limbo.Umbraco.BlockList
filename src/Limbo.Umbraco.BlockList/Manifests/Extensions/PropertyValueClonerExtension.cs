using Skybrud.Essentials.Umbraco.Manifests.Extensions;

namespace Limbo.Umbraco.BlockList.Manifests.Extensions;

internal class PropertyValueClonerExtension : IExtension {

    public string Type => "propertyValueCloner";

    public required string Alias { get; set; }

    public required string Name { get; set; }

    public required string Api { get; set; }

    public required string ForEditorAlias { get; set; }

}