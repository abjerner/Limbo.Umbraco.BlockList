// [CHANGE: Umbraco 17 upgrade] Related: see documentation/umbraco-17-upgrade.md

using System;
using System.Diagnostics.CodeAnalysis;
using Skybrud.Essentials.Common;

namespace Limbo.Umbraco.BlockList;

internal class BlockListUtils {

    private static readonly string[] _separator = [", Version"];

    [return: NotNullIfNotNull(nameof(value))]
    public static string? RemoveVersion(string? value) {
        return value?.Split(_separator, StringSplitOptions.None)[0];
    }

    public static string GetTypeAlias(Type type) {
        if (type.AssemblyQualifiedName is null) throw new PropertyNotSetException(nameof(type.AssemblyQualifiedName));
        return RemoveVersion(type.AssemblyQualifiedName);
    }

    // "AppendLinkToDescription" was removed as part of the Umbraco 17 upgrade. As of Umbraco 14, "ConfigurationField"
    // no longer carries a description - labels and descriptions are declared client side in "umbraco-package.json".

}