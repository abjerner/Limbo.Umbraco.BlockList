using System;
using System.Diagnostics.CodeAnalysis;
using Umbraco.Cms.Core.PropertyEditors;

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

        public static void AppendLinkToDescription(ConfigurationField field, string text, string url) {
            string a = $"<a href=\"{url}\" class=\"btn btn-primary btn-xs limbo-block-list-button\" target=\"_blank\" rel=\"noreferrer noopener\">{text}</a>";
            field.Description = $"{field.Description}\r\n{a}";
        }

    }

}