using System.Collections.Generic;
using System.Threading.Tasks;
using Limbo.Umbraco.BlockList.Constants;
using Limbo.Umbraco.BlockList.Manifests.Conditions;
using Limbo.Umbraco.BlockList.Manifests.Extensions;
using Limbo.Umbraco.BlockList.PropertyEditors;
using Skybrud.Essentials.Security.Extensions;
using Skybrud.Essentials.Umbraco.Constants;
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
            Alias = LimboBlockListPropertyEditor.EditorAlias,
            Name = LimboBlockListPropertyEditor.EditorName,
            Meta = new PropertyEditorSchemaMeta {
                DefaultPropertyEditorUiAlias = LimboBlockListPropertyEditor.EditorUiAlias,
                Settings = new PropertyEditorSettings {
                    Properties = [
                        new PropertyEditorSettingsProperty {
                            Alias = "blocks",
                            Label = "Available Blocks",
                            Description = "Define the available blocks.",
                            PropertyEditorUiAlias = UmbPropertyEditorUiAliases.BlockListTypeConfiguration
                        },
                        new PropertyEditorSettingsProperty {
                            Alias = "validationLimit",
                            Label = "Amount",
                            Description = "Set a required range of blocks",
                            PropertyEditorUiAlias = UmbracoPropertyEditorUiAliases.NumberRange,
                            Config = new object[] { new { alias = "validationRange", value = new { min = 0 } } }
                        },
                        new PropertyEditorSettingsProperty {
                            Alias = "typeConverter",
                            Label = "Type Converter",
                            Description = "Select the type converter used for converting the block list value into a custom CLR type.",
                            PropertyEditorUiAlias = BlockListPropertyEditorUiAliases.TypeConverter
                        },
                        new PropertyEditorSettingsProperty {
                            Alias = "cacheLevel",
                            Label = "Cache Level",
                            Description = "Select the cache level of the underlying property value converter.",
                            PropertyEditorUiAlias = BlockListPropertyEditorUiAliases.CacheLevel
                        }
                    ]
                }
            }
        };



        yield return new PropertyEditorUiExtension {
            Alias = BlockListPropertyEditorUiAliases.BlockList,
            Name = $"{Name}: Block List Property Editor UI",
            Element = $"/App_Plugins/{Alias}/js/property-editor-ui-block-list.element.js",
            Meta = new PropertyEditorUiMeta {
                Label = "Limbo Block List",
                PropertyEditorSchemaAlias = LimboBlockListPropertyEditor.EditorAlias,
                Icon = "icon-thumbnail-list",
                Group = "Limbo",
                SupportsReadOnly = true,
                Settings = new PropertyEditorSettings {
                    Properties = [
                        new PropertyEditorSettingsProperty {
                            Alias = "useLiveEditing",
                            Label = "Live editing mode",
                            Description = "Live editing in editor overlays for live updated custom views or labels using custom expression.",
                            PropertyEditorUiAlias = UmbracoPropertyEditorUiAliases.Toggle
                        },
                        new PropertyEditorSettingsProperty {
                            Alias = "useInlineEditingAsDefault",
                            Label = "Inline editing mode",
                            Description = "Use the inline editor as the default block view.",
                            PropertyEditorUiAlias = UmbracoPropertyEditorUiAliases.Toggle
                        },
                        new PropertyEditorSettingsProperty {
                            Alias = "maxPropertyWidth",
                            Label = "Property editor width",
                            Description = "Optional CSS override, example: 800px or 100%",
                            PropertyEditorUiAlias = UmbracoPropertyEditorUiAliases.TextBox
                        },
                        new PropertyEditorSettingsProperty {
                            Alias = "createModalSize",
                            Label = "#blockEditor_labelCreateModalSize",
                            PropertyEditorUiAlias = UmbracoPropertyEditorUiAliases.OverlaySize,
                            Config = new[] {
                                new PropertyEditorConfigProperty {
                                    Alias = "defaultOptionLabel",
                                    Value = "Auto"
                                }
                            }
                        }
                    ]
                }
            }
        };

        yield return new PropertyEditorUiExtension {
            Alias = BlockListPropertyEditorUiAliases.TypeConverter,
            Name = $"{Name}: Type Converter Property Editor UI",
            Element = $"/App_Plugins/{Alias}/js/property-editor-ui-type-converter.element.js",
            Meta = new PropertyEditorUiMeta {
                Label = "Limbo Block List Type Converter",
                Icon = "icon-autofill",
                Group = "common",
                PropertyEditorSchemaAlias = null!
            }
        };

        yield return new PropertyEditorUiExtension {
            Alias = BlockListPropertyEditorUiAliases.CacheLevel,
            Name = $"{Name}: Cache Level Property Editor UI",
            Element = $"/App_Plugins/{Alias}/js/property-editor-ui-cache-level.element.js",
            Meta = new PropertyEditorUiMeta {
                Label = "Limbo Block List Cache Level",
                Icon = "icon-box",
                Group = "common",
                PropertyEditorSchemaAlias = null!
            }
        };

        yield return new PropertyValueResolverExtension {
            Alias = $"{Alias}.PropertyValueResolver",
            Name = $"{Name}: Value Resolver",
            Api = $"/App_Plugins/{Alias}/js/property-value-resolver.js",
            ForEditorAlias = LimboBlockListPropertyEditor.EditorAlias
        };

        yield return new PropertyValueClonerExtension {
            Alias = $"{Alias}.PropertyValueCloner",
            Name = $"{Name}: Value Cloner",
            Api = $"/App_Plugins/{Alias}/js/property-value-cloner.js",
            ForEditorAlias = LimboBlockListPropertyEditor.EditorAlias
        };

        yield return new PropertyValidationPathTranslator {
            Alias = $"{Alias}.PropertyValidationPathTranslator",
            Name = $"{Name}: Property Validation Path Translator",
            Api = $"/App_Plugins/{Alias}/js/property-validation-path-translator.js",
            ForEditorAlias = LimboBlockListPropertyEditor.EditorAlias
        };

        yield return new PropertyContext {
            Kind = "sortMode",
            Alias = $"{Alias}.PropertyContext.SortMode",
            Name = $"{Name}: Sort Mode Property Context",
            ForPropertyEditorUis = [LimboBlockListPropertyEditor.EditorUiAlias]
        };

        yield return new PropertyAction {
            Kind = "sortMode",
            Alias = $"{Alias}.PropertyAction.SortMode",
            Name = $"{Name}: Sort Mode Property Action",
            ForPropertyEditorUis = [LimboBlockListPropertyEditor.EditorUiAlias],
            Conditions = [new PropertyHasValueCondition()]
        };

        yield return new PropertyContext {
            Kind = "clipboard",
            Alias = $"{Alias}.PropertyContext.Clipboard",
            Name = $"{Name}: Clipboard Property Context",
            ForPropertyEditorUis = [LimboBlockListPropertyEditor.EditorUiAlias]
        };

        yield return new PropertyAction {
            Kind = "copyToClipboard",
            Alias = $"{Alias}.PropertyAction.Clipboard.Copy",
            Name = $"{Name}: Copy To Clipboard Property Action",
            ForPropertyEditorUis = [LimboBlockListPropertyEditor.EditorUiAlias],
            Conditions = [new PropertyHasValueCondition()]
        };

        yield return new PropertyAction {
            Kind = "pasteFromClipboard",
            Alias = $"{Alias}.PropertyAction.Clipboard.Paste",
            Name = $"{Name}: Paste From Clipboard Property Action",
            ForPropertyEditorUis = [LimboBlockListPropertyEditor.EditorUiAlias],
            Conditions = [new PropertyWritableCondition()]
        };

        yield return new ClipboardCopyPropertyValueTranslator {
            Alias = $"{Alias}.ClipboardCopyPropertyValueTranslator.BlockListToBlock",
            Name = $"{Name}: Clipboard Copy Property Value Translator",
            Api = $"/App_Plugins/{Alias}/js/clipboard-copy-translator.js",
            FromPropertyEditorUi = LimboBlockListPropertyEditor.EditorUiAlias,
            ToClipboardEntryValueType = "block"
        };

        yield return new ClipboardPastePropertyValueTranslator {
            Alias = $"{Alias}.ClipboardPastePropertyValueTranslator.BlockToBlockList",
            Name = $"{Name}: Clipboard Paste Property Value Translator",
            Api = $"/App_Plugins/{Alias}/js/clipboard-paste-translator.js",
            FromClipboardEntryValueType = "block",
            ToPropertyEditorUi = LimboBlockListPropertyEditor.EditorUiAlias
        };

        yield return new PropertyAction {
            Kind = "clear",
            Alias = $"{Alias}.PropertyAction.Clear",
            Name = $"{Name}: Clear Property Action",
            ForPropertyEditorUis = [LimboBlockListPropertyEditor.EditorUiAlias]
        };

    }

}