using System.Collections.Generic;
using Skybrud.Essentials.Umbraco.Manifests.Extensions;

namespace Limbo.Umbraco.BlockList.Manifests.Extensions;

internal class PropertyContext : IExtension {

    public string Type => "propertyContext";

    public required string Kind { get; set; }

    public required string Alias { get; set; }

    public required string Name { get; set; }

    public required List<string> ForPropertyEditorUis { get; set; }

}