using System.Collections.Generic;
using Skybrud.Essentials.Umbraco.Manifests.Conditions;
using Skybrud.Essentials.Umbraco.Manifests.Extensions;

namespace Limbo.Umbraco.BlockList.Manifests.Extensions;

internal class PropertyAction : IExtension {

    public string Type => "propertyAction";

    public required string Kind { get; set; }

    public required string Alias { get; set; }

    public required string Name { get; set; }

    public required List<string> ForPropertyEditorUis { get; set; }

    public List<ICondition> Conditions { get; set; } = [];

}