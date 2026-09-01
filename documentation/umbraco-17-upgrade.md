# Umbraco 17 upgrade

Recap of the work done when moving `Limbo.Umbraco.BlockList` from **Umbraco 13 / .NET 8** to
**Umbraco 17 / .NET 10** on the `v17/dev` branch.

The jump crosses the Umbraco 14 boundary, which replaced the AngularJS backoffice with a Lit/web-component
backoffice, replaced the backoffice HTTP API with the Management API, and moved Umbraco from Newtonsoft.Json to
System.Text.Json. Almost every seam this package plugs into therefore changed.

---

## 1. Project file

`src/Limbo.Umbraco.BlockList/Limbo.Umbraco.BlockList.csproj`

| | Before | After |
|---|---|---|
| Target framework | `net8` | `net10.0` |
| Version | `13.0.3` | `17.0.0-alpha000` |
| `Umbraco.Cms.Core` | `[13.1.0,13.999)` | `[17.0.0,17.9.9)` |
| `Umbraco.Cms.Web.Website` | `[13.1.0,13.999)` | `[17.0.0,17.9.9)` |
| `Umbraco.Cms.Web.BackOffice` | `[13.1.0,13.999)` | **removed** |
| `Umbraco.Cms.Api.Management` | – | `[17.0.0,17.9.9)` |
| `Skybrud.Essentials` | `1.1.63` | `1.1.68` |

- `Umbraco.Cms.Web.BackOffice` no longer exists past Umbraco 13; the backoffice server API lives in
  `Umbraco.Cms.Api.Management`.
- The Debug version suffix is now `alpha000-build<timestamp>` so debug packages still sort after the release
  version while keeping the pre-release tag.
- LESS/Web Compiler support (`compilerconfig.json`, `Styles.less`, `Styles.css`) was dropped - the new backoffice
  elements carry their styles in the element modules.
- `NoWarn` for `NU1902`/`NU1903` moved from the individual package references to a project-level property. The
  lower bound of the version range makes restore resolve Umbraco 17.0.0 exactly, and that version's transitive
  dependencies carry advisories that are fixed in later 17.x patches. Suppressing them keeps the intent of the v13
  branch, which suppressed `NU1902` the same way.

`debug.bat` now packs to `c:\nuget\Umbraco17`.

---

## 2. The alias-spoofing hack is gone

The v13 package registered its own editor alias `Limbo.Umbraco.BlockList` but reused Umbraco's AngularJS view
`blocklist`. That view hardcoded a check for the alias `Umbraco.BlockList`, so
`NotificationHandlers/SendingContentHandler.cs` rewrote our alias to Umbraco's on every `SendingContentNotification`
before the content reached the backoffice.

**Deleted.** `SendingContentNotification` does not exist in Umbraco 17, and the new backoffice does not need it: a
property editor *schema* and a property editor *UI* are separate extensions, so our schema can simply point at a UI
that renders Umbraco's block list editor.

---

## 3. Property editor

`PropertyEditors/LimboBlockListPropertyEditor.cs`

- Now derives from **`BlockListPropertyEditor`** instead of `BlockListPropertyEditorBase`. The variant merging
  overrides (`CanMergePartialPropertyValues`, `MergePartialPropertyValueForCulture`,
  `MergeVariantInvariantPropertyValue`) cast to `BlockListEditorPropertyValueEditor`, which is `internal sealed`;
  deriving from `BlockListPropertyEditorBase` would silently drop that behaviour. `DataEditorAttribute` is read with
  `inherit: false`, so declaring our own alias on the subclass is safe.
- `[DataEditor]` lost its `name`, `view`, `Group` and `Icon` arguments - those are client-side concerns now. What
  remains is `[DataEditor(EditorAlias, ValueType = ValueTypes.Json, ValueEditorIsReusable = false)]`.
- The constructor signature changed to
  `(IDataValueEditorFactory, IIOHelper, IBlockValuePropertyIndexValueFactory, IJsonSerializer)`.
- `EditorView` constant removed; `EditorUiAlias` constant added.

`PropertyEditors/LimboBlockListConfigurationEditor.cs`

- `ConfigurationEditor<T>` now takes only `IIOHelper` - `IEditorConfigurationParser` is gone.
- The whole constructor body was removed. `ConfigurationField` no longer carries a name, description or view, so
  there is nothing left to post-process (this also removed `BlockListUtils.AppendLinkToDescription`).

`PropertyEditors/LimboBlockListConfiguration.cs`

- `[ConfigurationField]` reduced to just the key.
- Umbraco removed `UseLiveEditing` and `UseInlineEditingAsDefault` from its own `BlockListConfiguration`, so they
  are declared on `LimboBlockListConfiguration` instead (keeping the fluent extension methods working). Added
  `MaxPropertyWidth` to match the built-in editor.
- `[IgnoreDataMember]` on `IsSinglePicker` became `[JsonIgnore]` (System.Text.Json).

`PropertyEditors/LimboBlockListPropertyValueConverter.cs`

- Base constructor grew to
  `(IProfilingLogger, BlockEditorConverter, IContentTypeService, IApiElementBuilder, IJsonSerializer, BlockListPropertyValueConstructorCache, IVariationContextAccessor, BlockEditorVarianceHandler)`.
- `propertyType.DataType.Configuration` became `propertyType.DataType.ConfigurationObject`.
- The converter dispatch logic itself is unchanged - `Convert` / `ConvertItem` still run on top of whatever the base
  converter produced, and every override still falls back to the base implementation when no type converter is
  selected.

---

## 4. Notification handlers

`LimboBlockListPropertyNotificationHandler` is unchanged and still registered for `ContentSavingNotification`,
`ContentCopyingNotification` and `ContentScaffoldedNotification`. It is still required: Umbraco's own
`BlockListPropertyNotificationHandler` matches on the `Umbraco.BlockList` alias only, so our properties would
otherwise not get new block keys when content is copied.

---

## 5. Manifest: `IManifestFilter` → `umbraco-package.json`

`Manifests/BlockListManifestFilter.cs` was **deleted** - `IManifestFilter` no longer exists.

The client side is now declared in `src/Limbo.Umbraco.BlockList/wwwroot/umbraco-package.json`. Umbraco discovers it
through the project's static web assets, which `StaticWebAssetBasePath` maps to
`App_Plugins/Limbo.Umbraco.BlockList/` (verified in the packed `.nupkg`).

### Why the client side is shaped the way it is

In the new backoffice a data type is created by picking a **property editor UI**, and the UI's
`meta.propertyEditorSchemaAlias` decides which schema the data type gets. For the Limbo editor to be pickable at
all, it therefore needs its own property editor UI - it cannot simply borrow `Umb.PropertyEditorUi.BlockList`,
because that UI is bound to Umbraco's `Umbraco.BlockList` schema.

Umbraco does **not** export its block list element class from a public entry point (only constants are exported from
`@umbraco-cms/backoffice/block-list`), so it cannot be subclassed. Instead
`js/property-editor-ui-block-list.element.js` resolves the `Umb.PropertyEditorUi.BlockList` manifest from
`umbExtensionsRegistry`, calls `loadManifestElement` on it - which defines the
`<umb-property-editor-ui-block-list>` custom element as a side effect - and then renders that tag, forwarding
`value`, `config`, `name`, `readonly` and `mandatory` down.

Two details of that forwarding are load bearing:

- `readonly` **must** be forwarded as a property binding (`.readonly=`), not as an attribute binding (`?readonly=`).
  Umbraco's element declares `readonly` as a plain accessor rather than a reactive property, so it observes no
  `readonly` attribute and an attribute binding would never reach the setter. Combined with `supportsReadOnly: true`
  on our manifest - which tells `umb-property` not to draw its own blocking overlay - the blocks would stay fully
  editable for read-only properties.
- The wrapper is an `UmbFormControlMixin` element and registers Umbraco's element via `addFormControlElement`.
  `umb-property` only sets up `UmbFormControlValidator` and the server validation binding when the property editor UI
  element exposes `checkValidity`, so a plain `UmbLitElement` wrapper would silently swallow the mandatory and
  min/max-amount validators.

Note that Umbraco's block list element does not dispatch `change` events - it writes directly to
`UMB_PROPERTY_CONTEXT`, which resolves through the wrapper's shadow root to the same context `umb-property` provides.
The `change` handler on the wrapper is therefore only a safety net.

### Extensions registered

| Type | Alias | Purpose |
|---|---|---|
| `propertyEditorSchema` | `Limbo.Umbraco.BlockList` | Matches the C# editor alias. Declares the data type settings: `blocks`, `validationLimit`, `typeConverter`, `cacheLevel`. |
| `propertyEditorUi` | `Limbo.PropertyEditorUi.BlockList` | The editing surface (wrapper described above) plus the `useLiveEditing` / `useInlineEditingAsDefault` / `maxPropertyWidth` settings. |
| `propertyEditorUi` | `Limbo.PropertyEditorUi.BlockList.TypeConverter` | Replaces the AngularJS `TypeConverter.html` view and its editor-service overlay. |
| `propertyEditorUi` | `Limbo.PropertyEditorUi.BlockList.CacheLevel` | Replaces the AngularJS `CacheLevel.html` view. |
| `propertyValueResolver` | `Limbo.PropertyValueResolver.BlockList` | Re-export of `UmbStandardBlockValueResolver`. |
| `propertyValueCloner` | `Limbo.PropertyValueCloner.BlockList` | Subclass of `UmbFlatLayoutBlockPropertyValueCloner`; gives blocks new keys on duplicate. |
| `propertyValidationPathTranslator` | `Limbo.PropertyValidationPathTranslator.BlockList` | Maps server validation paths onto the right block property. |
| `propertyContext` / `propertyAction` (kind `sortMode`) | `Limbo.…BlockList.SortMode` | Restores the sort-mode property action. |
| `propertyContext` (kind `clipboard`) | `Limbo.PropertyContext.BlockList.Clipboard` | **Required for the editor to work at all** - see below. |
| `propertyAction` (kinds `copyToClipboard`, `pasteFromClipboard`, `clear`) | `Limbo.PropertyAction.BlockList.…` | Restores the clipboard and clear property actions. |
| `clipboardCopyPropertyValueTranslator` / `clipboardPastePropertyValueTranslator` | `Limbo.Clipboard…Translator.…` | Convert between the block list value and the `block` clipboard entry type. Reimplemented from public exports; core's are private chunks. |

The clipboard **property context** is not optional. Umbraco's block list element consumes
`UMB_PROPERTY_CONTEXT` with API alias `UmbClipboardPropertyContext`, and core only registers it for
`Umb.PropertyEditorUi.BlockList`. Without a registration for the Limbo UI alias the consumer never resolves, the
console fills with

```
Uncaught (in promise) Context could not be found.
(Context Alias: UmbPropertyContext with API Alias: UmbClipboardPropertyContext)
```

and the block catalogue never opens - clicking *Add content* silently does nothing.

The last four exist because Umbraco registers its equivalents with `forEditorAlias: "Umbraco.BlockList"` /
`forPropertyEditorUis: ["Umb.PropertyEditorUi.BlockList"]`, which do not match the Limbo aliases.

> Note on the value cloner: it is constructed with `UMB_BLOCK_LIST_PROPERTY_EDITOR_SCHEMA_ALIAS`
> (`"Umbraco.BlockList"`), **not** the Limbo alias. The value editor is inherited from Umbraco's, so the stored value
> is a `BlockListValue`, which always writes its layout under the `Umbraco.BlockList` key.

One UUI trap worth remembering: `<uui-button>` renders its `label` property only while its default slot is empty,
and it counts a **whitespace-only text node** as slotted content. A line break between the opening and closing tag is
therefore enough to make the button render blank. Close the tag immediately - `...@click=${…}></uui-button>` - as
Umbraco's own templates do.

All JS files are plain ES modules using the bare specifiers from the backoffice import map - no npm, bundler or
build step was introduced. Because there is no compile step, the elements use `static properties` and
`customElements.define(...)` rather than Lit decorators.

---

## 6. Backoffice API → Management API

`Controllers/BlockListController.cs` was replaced by `Controllers/BlockListTypeConverterController.cs`.

| | Before | After |
|---|---|---|
| Base class | `UmbracoAuthorizedApiController` | `ManagementApiControllerBase` |
| Routing | `[PluginController("Limbo")]` | `[VersionedApiBackOfficeRoute("limbo/block-list")]` + `[ApiVersion("1.0")]` |
| Route | `/umbraco/backoffice/Limbo/BlockList/GetTypeConverters` | `/umbraco/management/api/v1/limbo/block-list/type-converters` |

Authorization (`BackOfficeAccess`) is inherited from `ManagementApiControllerBase`, so no extra wiring is needed
server side. The endpoint is part of the standard `management` OpenAPI document.

### Calling it from the client

`js/property-editor-ui-type-converter.element.js` calls the endpoint through `umbHttpClient`, and the call **must**
declare a security scheme:

```js
await umbHttpClient.get({
    url: '/umbraco/management/api/v1/limbo/block-list/type-converters',
    security: [{ scheme: 'bearer', type: 'http' }],
});
```

`umbHttpClient` is a hey-api client that Umbraco configures with an `auth` callback returning the backoffice access
token, but the client only resolves that callback - and therefore only sets the `Authorization` header - for requests
that pass `security`. Umbraco's generated SDK methods always pass it; a hand-written `.get({ url })` does not.

Without it the request goes out unauthenticated, the Management API answers **401**, and the backoffice's response
interceptor reacts to the 401 by restarting the authorization flow - so the symptom is *the editor gets logged out*
when opening the data type, not an error in the UI.

---

## 7. Newtonsoft.Json → System.Text.Json

- `Json/Newtonsoft/BlockListJsonConverter.cs` → `Json/BlockListTypeConverterJsonConverter.cs`, rewritten as a
  `System.Text.Json.Serialization.JsonConverter<BlockListTypeConverter?>`.
- Reading stays deliberately lenient so configurations saved by older versions still load: a plain string, an object
  with a `type` property, and the legacy object with a `key` property are all accepted. Writing is always normalized
  to `{ "type": "..." }`.
- `Models/Api/BlockListTypeConverterApiModel.cs` swapped `[JsonProperty]` for `[JsonPropertyName]` and became
  public, since it is returned from a public Management API controller. Its `Icon` fallback no longer derives a
  `color-<vendor>` CSS class (the new backoffice has no such classes) and `Description` is now just the type alias.
- Umbraco's configuration serializer already registers `JsonStringEnumConverter`, so `PropertyCacheLevel?` still
  round-trips as a string with no extra attributes.

---

## 8. Unchanged

`IBlockListTypeConverter`, `BlockListTypeConverterCollection`, `BlockListTypeConverterCollectionBuilder` and
`BlockListUtils.GetTypeAlias` / `RemoveVersion` were not touched. Converters are still identified by their
version-less assembly qualified name, so **existing data type configurations keep working** and third-party
`IBlockListTypeConverter` implementations only need a recompile against the new target framework.

`Extensions/BlockListExtensions.cs` kept its full surface. `SetUseSingleBlockMode` is now `[Obsolete]` because
Umbraco deprecated single block mode in favour of the dedicated *Single Block* property editor; `SetMaxPropertyWidth`
was added.

---

## 9. Deleted files

```
Manifests/BlockListManifestFilter.cs
NotificationHandlers/SendingContentHandler.cs
Controllers/BlockListController.cs          (replaced)
Json/Newtonsoft/BlockListJsonConverter.cs   (replaced)
compilerconfig.json
compilerconfig.json.defaults
wwwroot/CacheLevel.html
wwwroot/CacheLevel.js
wwwroot/TypeConverter.html
wwwroot/TypeConverter.js
wwwroot/TypeConverterOverlay.html
wwwroot/TypeConverterOverlay.js
wwwroot/Styles.less
wwwroot/Styles.css
```

---

## 10. Verification status

- ✅ `dotnet build -c Release` - succeeds, **0 warnings, 0 errors**.
- ✅ `dotnet build … /t:pack` - produces `releases/nuget/Limbo.Umbraco.BlockList.17.0.0-alpha000.nupkg`, with the
  client assets correctly mapped to `App_Plugins/Limbo.Umbraco.BlockList/`.
- ⚠️ **Not yet run against a live Umbraco 17 site.** The server-side API usage was verified by decompiling the
  Umbraco 17.0.0 assemblies, and the client manifests were modelled on Umbraco's own shipped
  `packages/block/umbraco-package.js`, but the backoffice behaviour has not been smoke-tested. The wrapper element
  in particular (§5) depends on Umbraco's internal element tag name `umb-property-editor-ui-block-list`, which is
  not part of the public API and could change in a future minor release.

### Suggested smoke test

1. Install the package into an Umbraco 17 site.
2. **Settings → Data Types → Create** and pick *Limbo Block List*; confirm the four configuration fields render
   (Available Blocks, Amount, Type Converter, Cache Level).
3. Confirm the Type Converter dropdown is populated - it calls
   `/umbraco/management/api/v1/limbo/block-list/type-converters`.
4. Add the data type to a document type, add some blocks in the content section, and save/publish.
5. Copy the content node and confirm the copied blocks got new keys.
6. Confirm the rendered value on the front end is the type returned by the selected `IBlockListTypeConverter`.
