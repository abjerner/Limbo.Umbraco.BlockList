# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this is

A single-project NuGet package (`Limbo.Umbraco.BlockList`) that wraps Umbraco's built-in Block List property editor so the CLR type returned to the site/API can be swapped out via a pluggable *type converter* selected per data type in the backoffice. Umbraco 17 / .NET 10, no test project.

`documentation/umbraco-17-upgrade.md` is the recap of the Umbraco 13 → 17 migration and explains why most of the current structure looks the way it does. Read it before changing anything in `wwwroot/` or the property editor classes.

## Commands

```bash
# Restore/build
dotnet build src/Limbo.Umbraco.BlockList

# Release pack -> releases/nuget/  (release.bat)
dotnet build src/Limbo.Umbraco.BlockList --configuration Release /t:rebuild /t:pack -p:PackageOutputPath=../../releases/nuget

# Debug pack -> local NuGet feed (debug.bat, Windows path)
dotnet build src/Limbo.Umbraco.BlockList --configuration Debug /t:rebuild /t:pack -p:PackageOutputPath=c:\nuget\Umbraco17
```

Debug builds get an auto timestamped `VersionSuffix` (`alpha000-buildyyyyMMddHHmm`) so they can be re-installed into a local test site repeatedly. The version lives in `<VersionPrefix>`/`<VersionSuffix>` in the `.csproj`, and is *also* hardcoded in `wwwroot/umbraco-package.json` and in the install snippets in `README.md` — bump all three.

There is no npm/bundler step. `wwwroot/js/*.js` are plain ES modules that rely on the backoffice import map for the `@umbraco-cms/backoffice/*` bare specifiers; because nothing compiles them, they use `static properties` + `customElements.define(...)` rather than Lit decorators.

## Architecture

Server side is wired from `Composers/BlockListComposer.cs`; client side from `wwwroot/umbraco-package.json`. Trace from those two when in doubt.

**Alias split.** The editor alias `Limbo.Umbraco.BlockList` is deliberately *not* Umbraco's `Umbraco.BlockList`. Consequences that keep biting:

- Anything Umbraco registers with `forEditorAlias: "Umbraco.BlockList"` or `forPropertyEditorUis: ["Umb.PropertyEditorUi.BlockList"]` does not apply to us and has to be re-registered under our aliases (value resolver, value cloner, validation path translator, sort mode — see the manifest).
- Likewise server-side: `LimboBlockListPropertyNotificationHandler` re-registers the block-editor `ContentSaving`/`ContentCopying`/`ContentScaffolded` behaviour (e.g. re-keying blocks on copy) that Umbraco only wires for its own alias.
- But the *stored value* is a `BlockListValue` (inherited value editor), which always writes its layout under the `Umbraco.BlockList` key. Client code touching `value.layout[...]` must use `UMB_BLOCK_LIST_PROPERTY_EDITOR_SCHEMA_ALIAS`, not our alias.

**Converter pipeline.**
- `IBlockListTypeConverter` (Converters/) — the public extension point. Implementations are discovered by `TypeLoader` and registered into `BlockListTypeConverterCollection` via a `LazyCollectionBuilderBase`. Implementors override `GetType` + `Convert`; `ConvertItem` (single-block mode) is a default interface method that throws unless overridden.
- Converters are identified by an **assembly-qualified name with the version segment stripped** (`BlockListUtils.GetTypeAlias` / `RemoveVersion`). This is what's persisted in the data type config, so converter identity survives package version bumps but *not* renaming/moving the class or assembly.
- `LimboBlockListConfiguration` extends Umbraco's `BlockListConfiguration` with `TypeConverter`, `CacheLevel` and the UI-only settings Umbraco dropped from its own config class. `IsSinglePicker` is derived from `ValidationLimit.Max == 1`.
- `LimboBlockListPropertyValueConverter` subclasses Umbraco's `BlockListPropertyValueConverter`. It calls `base` first, then hands the resulting `BlockListModel`/`BlockListItem` to the selected converter. Every override falls back to the base implementation when the config isn't a `LimboBlockListConfiguration` or no converter is selected — preserve that fallback.
- `Models/BlockListTypeConverter` is the persisted config value; its System.Text.Json converter (`Json/BlockListTypeConverterJsonConverter`) tolerantly reads a bare string, `{ "type": ... }`, or a legacy `{ "key": ... }`, but always writes `{ "type": ... }`. Keep it lenient — it's the compatibility bridge for pre-v17 data types.

**Property editor.** `LimboBlockListPropertyEditor` derives from Umbraco's concrete `BlockListPropertyEditor`, not from `BlockListPropertyEditorBase`, because the variant-merging overrides cast to an `internal sealed` value editor that can't be reached from outside Umbraco. Only `CreateConfigurationEditor()` is overridden. `DataEditorAttribute` is read with `inherit: false`, so redeclaring the alias on the subclass is safe.

**Backoffice UI.** `wwwroot/umbraco-package.json` declares a `propertyEditorSchema` (`Limbo.Umbraco.BlockList`) plus three `propertyEditorUi`s. Umbraco does not export its block list *element* from a public entry point, so `js/property-editor-ui-block-list.element.js` resolves the `Umb.PropertyEditorUi.BlockList` manifest from `umbExtensionsRegistry`, `loadManifestElement`s it (which defines `<umb-property-editor-ui-block-list>` as a side effect) and renders that tag, forwarding `value`/`config`/`readonly` down and `change` up. This depends on an Umbraco-internal tag name — if the editor stops rendering after an Umbraco upgrade, check that first.

The type converter picker is fed by `Controllers/BlockListTypeConverterController` (`ManagementApiControllerBase`, `[VersionedApiBackOfficeRoute("limbo/block-list")]`), i.e. `/umbraco/management/api/v1/limbo/block-list/type-converters`, projected through `Models/Api/BlockListTypeConverterApiModel`.

## Conventions

- `src/.editorconfig` is authoritative (4 spaces, CRLF, no final newline, file-scoped namespaces, `System` usings first). Match the surrounding file rather than reformatting.
- Public API is fully XML-documented (`DocumentationFile` is on); internal/plumbing classes instead use `#pragma warning disable 1591` at the top. Follow whichever the file already does.
- Nullable is enabled project-wide. The build is expected to stay at **0 warnings** — `NU1902`/`NU1903` are suppressed project-wide because the version range floor resolves Umbraco 17.0.0 exactly.
- `BlockListPackage` holds all package constants (alias, name, URLs, version) — reference it instead of re-typing strings.

## Branches

Long-lived per major Umbraco version: `v13/main` targets Umbraco 13 (still the GitHub default branch); `v1`–`v3` are EOL. The current checkout is `v17/dev`.
