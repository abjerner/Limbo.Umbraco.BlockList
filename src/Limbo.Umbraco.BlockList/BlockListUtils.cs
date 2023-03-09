using System;
using System.Diagnostics.CodeAnalysis;

namespace Limbo.Umbraco.BlockList {

    internal class BlockListUtils {

        private static readonly string[] _separator = { ", Version" };

        [return: NotNullIfNotNull("value")]
        public static string? RemoveVersion(string? value) {
            return value?.Split(_separator, StringSplitOptions.None)[0];
        }

        public static string? GetTypeAlias(Type type) {
            return RemoveVersion(type.AssemblyQualifiedName);
        }

    }

}