using System.Collections.Generic;
using System.Threading.Tasks;
using Limbo.Umbraco.BlockList.Manifests.Conditions;
using Limbo.Umbraco.BlockList.Manifests.Extensions;
using Skybrud.Essentials.Security.Extensions;
using Skybrud.Essentials.Umbraco.Manifests.Extensions;
using Skybrud.Essentials.Umbraco.Manifests.Extensions.PropertyEditors;
using Umbraco.Cms.Core.Manifest;
using Umbraco.Cms.Infrastructure.Manifest;

namespace Limbo.Umbraco.BlockList.Manifests;

#pragma warning disable CS1591

public class BlockListPackageManifestReader : IPackageManifestReader {

    public static string CacheBuster = BlockListPackage.InformationalVersion.ToMd5Hash();

    public const string Alias = BlockListPackage.Alias;

    public const string Name = BlockListPackage.Name;

    public async Task<IEnumerable<PackageManifest>> ReadPackageManifestsAsync() {

        List<PackageManifest> temp = [
            new() {
                Id = BlockListPackage.Alias,
                Name = BlockListPackage.Name,
                AllowTelemetry = true,
                Version = BlockListPackage.InformationalVersion,
                Extensions = [..GetExtensions()]
            }

        ];

        return await Task.FromResult(temp);

    }

    private static IEnumerable<IExtension> GetExtensions() {

        yield return new PropertyEditorSchemaExtension {
            Alias = "Limbo.Umbraco.BlockList",
            Name = "Limbo Block List",
            Meta = new PropertyEditorSchemaMeta {
                DefaultPropertyEditorUiAlias = "Limbo.PropertyEditorUi.BlockList",
                Settings = new PropertyEditorSettings {
                    Properties = [
                        new PropertyEditorSettingsProperty {
                            Alias = "blocks",
                            Label = "",
                            Description = "Define the available blocks.",
                            PropertyEditorUiAlias = "Umb.PropertyEditorUi.BlockListTypeConfiguration"
                        },
                        new PropertyEditorSettingsProperty {
                            Alias = "validationLimit",
                            Label = "Amount",
                            Description = "Set a required range of blocks",
                            PropertyEditorUiAlias = "Umb.PropertyEditorUi.NumberRange",
                            Config = new object[] { new { alias = "validationRange", value = new { min = 0 } } }
                        },
                        new PropertyEditorSettingsProperty {
                            Alias = "typeConverter",
                            Label = "Type Converter",
                            Description = "Select the type converter used for converting the block list value into a custom CLR type.",
                            PropertyEditorUiAlias = "Limbo.PropertyEditorUi.BlockList.TypeConverter"
                        },
                        new PropertyEditorSettingsProperty {
                            Alias = "cacheLevel",
                            Label = "Cache Level",
                            Description = "Select the cache level of the underlying property value converter.",
                            PropertyEditorUiAlias = "Limbo.PropertyEditorUi.BlockList.CacheLevel"
                        }
                    ]
                }
            }
        };

        yield return new PropertyEditorUiExtension {
            Alias = "Limbo.PropertyEditorUi.BlockList",
            Name = "Limbo Block List Property Editor UI",
            Element = "/App_Plugins/Limbo.Umbraco.BlockList/js/property-editor-ui-block-list.element.js",
            Meta = new PropertyEditorUiMeta {
                Label = "Limbo Block List",
                PropertyEditorSchemaAlias = "Limbo.Umbraco.BlockList",
                Icon = "icon-thumbnail-list",
                Group = "richContent",
                SupportsReadOnly = true,
                Settings = new PropertyEditorSettings {
                    Properties = [
                        new PropertyEditorSettingsProperty {
                            Alias = "useLiveEditing",
                            Label = "Live editing mode",
                            Description = "Live editing in editor overlays for live updated custom views or labels using custom expression.",
                            PropertyEditorUiAlias = "Umb.PropertyEditorUi.Toggle"
                        },
                        new PropertyEditorSettingsProperty {
                            Alias = "useInlineEditingAsDefault",
                            Label = "Inline editing mode",
                            Description = "Use the inline editor as the default block view.",
                            PropertyEditorUiAlias = "Umb.PropertyEditorUi.Toggle"
                        },
                        new PropertyEditorSettingsProperty {
                            Alias = "maxPropertyWidth",
                            Label = "Property editor width",
                            Description = "Optional CSS override, example: 800px or 100%",
                            PropertyEditorUiAlias = "Umb.PropertyEditorUi.TextBox"
                        }
                    ]
                }
            }
        };

        yield return new PropertyEditorUiExtension {
            Alias = "Limbo.PropertyEditorUi.BlockList.TypeConverter",
            Name = "Limbo Block List Type Converter Property Editor UI",
            Element = "/App_Plugins/Limbo.Umbraco.BlockList/js/property-editor-ui-type-converter.element.js",
            Meta = new PropertyEditorUiMeta {
                Label = "Limbo Block List Type Converter",
                Icon = "icon-autofill",
                Group = "common",
                PropertyEditorSchemaAlias = null!
            }
        };

        yield return new PropertyEditorUiExtension {
            Alias = "Limbo.PropertyEditorUi.BlockList.CacheLevel",
            Name = "Limbo Block List Cache Level Property Editor UI",
            Element = "/App_Plugins/Limbo.Umbraco.BlockList/js/property-editor-ui-cache-level.element.js",
            Meta = new PropertyEditorUiMeta {
                Label = "Limbo Block List Cache Level",
                Icon = "icon-box",
                Group = "common",
                PropertyEditorSchemaAlias = null!
            }
        };

        yield return new PropertyValueResolverExtension {
            Alias = "Limbo.PropertyValueResolver.BlockList",
            Name = "Limbo Block List Value Resolver",
            Api = "/App_Plugins/Limbo.Umbraco.BlockList/js/property-value-resolver.js",
            ForEditorAlias = "Limbo.Umbraco.BlockList"
        };

        yield return new PropertyValueClonerExtension {
            Alias = "Limbo.PropertyValueCloner.BlockList",
            Name = "Limbo Block List Value Cloner",
            Api = "/App_Plugins/Limbo.Umbraco.BlockList/js/property-value-cloner.js",
            ForEditorAlias = "Limbo.Umbraco.BlockList"
        };

        yield return new PropertyValidationPathTranslator {
            Alias = "Limbo.PropertyValidationPathTranslator.BlockList",
            Name = "Limbo Block List Property Validation Path Translator",
            Api = "/App_Plugins/Limbo.Umbraco.BlockList/js/property-validation-path-translator.js",
            ForEditorAlias = "Limbo.Umbraco.BlockList"
        };

        yield return new PropertyContext {
            Kind = "sortMode",
            Alias = "Limbo.PropertyContext.BlockList.SortMode",
            Name = "Limbo Block List Sort Mode Property Context",
            ForPropertyEditorUis = ["Limbo.PropertyEditorUi.BlockList"]
        };

        yield return new PropertyAction {
            Kind = "sortMode",
            Alias = "Limbo.PropertyAction.BlockList.SortMode",
            Name = "Limbo Block List Sort Mode Property Action",
            ForPropertyEditorUis = ["Limbo.PropertyEditorUi.BlockList"],
            Conditions = [new PropertyHasValueCondition()]
        };

        yield return new PropertyContext {
            Kind = "clipboard",
            Alias = "Limbo.PropertyContext.BlockList.Clipboard",
            Name = "Limbo Block List Clipboard Property Context",
            ForPropertyEditorUis = ["Limbo.PropertyEditorUi.BlockList"]
        };

        yield return new PropertyAction {
            Kind = "copyToClipboard",
            Alias = "Limbo.PropertyAction.BlockList.Clipboard.Copy",
            Name = "Limbo Block List Copy To Clipboard Property Action",
            ForPropertyEditorUis = ["Limbo.PropertyEditorUi.BlockList"],
            Conditions = [new PropertyHasValueCondition()]
        };

        yield return new PropertyAction {
            Kind = "pasteFromClipboard",
            Alias = "Limbo.PropertyAction.BlockList.Clipboard.Paste",
            Name = "Limbo Block List Paste From Clipboard Property Action",
            ForPropertyEditorUis = ["Limbo.PropertyEditorUi.BlockList"],
            Conditions = [new PropertyWritableCondition()]
        };

        yield return new ClipboardCopyPropertyValueTranslator {
            Alias = "Limbo.ClipboardCopyPropertyValueTranslator.BlockListToBlock",
            Name = "Limbo Block List To Block Clipboard Copy Property Value Translator",
            Api = "/App_Plugins/Limbo.Umbraco.BlockList/js/clipboard-copy-translator.js",
            FromPropertyEditorUi = "Limbo.PropertyEditorUi.BlockList",
            ToClipboardEntryValueType = "block"
        };

        yield return new ClipboardPastePropertyValueTranslator {
            Alias = "Limbo.ClipboardPastePropertyValueTranslator.BlockToBlockList",
            Name = "Limbo Block To Block List Clipboard Paste Property Value Translator",
            Api = "/App_Plugins/Limbo.Umbraco.BlockList/js/clipboard-paste-translator.js",
            FromClipboardEntryValueType = "block",
            ToPropertyEditorUi = "Limbo.PropertyEditorUi.BlockList"
        };

        yield return new PropertyAction {
            Kind = "clear",
            Alias = "Limbo.PropertyAction.BlockList.Clear",
            Name = "Limbo Block List Clear Property Action",
            ForPropertyEditorUis = ["Limbo.PropertyEditorUi.BlockList"]
        };

    }

}