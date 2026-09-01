// [CHANGE: Umbraco 17 upgrade] Related: see documentation/umbraco-17-upgrade.md

import { css, html, nothing } from '@umbraco-cms/backoffice/external/lit';
import { UmbLitElement } from '@umbraco-cms/backoffice/lit-element';
import { UmbFormControlMixin } from '@umbraco-cms/backoffice/validation';
import { UmbChangeEvent } from '@umbraco-cms/backoffice/event';
import { umbExtensionsRegistry } from '@umbraco-cms/backoffice/extension-registry';
import { loadManifestElement } from '@umbraco-cms/backoffice/extension-api';
import { UMB_BLOCK_LIST_PROPERTY_EDITOR_UI_ALIAS } from '@umbraco-cms/backoffice/block-list';

/**
 * The tag name of Umbraco's own block list property editor UI element.
 *
 * Umbraco does not export the element class from a public entry point, so instead of subclassing it we load the
 * module behind the "Umb.PropertyEditorUi.BlockList" manifest - which defines the custom element as a side effect -
 * and then render the tag, forwarding the property editor contract in both directions.
 */
const UMB_BLOCK_LIST_ELEMENT_NAME = 'umb-property-editor-ui-block-list';

/**
 * "UmbFormControlMixin" is required rather than a plain "UmbLitElement": "umb-property" only wires up
 * "UmbFormControlValidator" and the server validation binding when the property editor UI element exposes
 * "checkValidity". Without it, the validators Umbraco's block list element registers for the mandatory flag and for
 * the configured amount of blocks (min/max) would never reach the property, and content would save without errors.
 */
export class LimboPropertyEditorUiBlockListElement extends UmbFormControlMixin(UmbLitElement, undefined) {

	static properties = {
		// "value" is intentionally not declared here - it is declared by "UmbFormControlMixin", and re-declaring it
		// would make Lit replace the accessor backing it.
		config: { attribute: false },
		name: { type: String },
		readonly: { type: Boolean, reflect: true },
		mandatory: { type: Boolean },
		mandatoryMessage: { type: String },
		_ready: { state: true },
		_error: { state: true },
	};

	static styles = [
		css`
			:host {
				display: block;
			}
		`,
	];

	#inner;

	constructor() {
		super();
		this.readonly = false;
		this.mandatory = false;
		this._ready = false;
		this._error = undefined;
	}

	connectedCallback() {
		super.connectedCallback();
		this.#ensureInnerElement();
	}

	async #ensureInnerElement() {

		if (this._ready) return;

		if (customElements.get(UMB_BLOCK_LIST_ELEMENT_NAME)) {
			this._ready = true;
			return;
		}

		const manifest = umbExtensionsRegistry.getByAlias(UMB_BLOCK_LIST_PROPERTY_EDITOR_UI_ALIAS);

		if (!manifest) {
			this._error = `Could not find the "${UMB_BLOCK_LIST_PROPERTY_EDITOR_UI_ALIAS}" extension.`;
			return;
		}

		try {
			await loadManifestElement(manifest.element);
			await customElements.whenDefined(UMB_BLOCK_LIST_ELEMENT_NAME);
			this._ready = true;
		} catch (error) {
			console.error('[Limbo Block List] Failed loading the block list property editor UI.', error);
			this._error = 'Failed loading the block list property editor UI.';
		}

	}

	/**
	 * Associates Umbraco's block list element as a nested form control, so its validators are taken into account when
	 * this element is validated.
	 */
	updated(changedProperties) {

		super.updated(changedProperties);

		const inner = this.shadowRoot?.querySelector(UMB_BLOCK_LIST_ELEMENT_NAME) ?? undefined;

		if (inner === this.#inner) return;

		if (this.#inner) this.removeFormControlElement(this.#inner);

		this.#inner = inner;

		if (inner) this.addFormControlElement(inner);

	}

	/**
	 * Umbraco's block list element does not dispatch change events - it writes straight to the property context - but
	 * the handler is kept as a safety net in case that ever changes.
	 */
	#onChange(event) {
		event.stopPropagation();
		this.value = event.target.value;
		this.dispatchEvent(new UmbChangeEvent());
	}

	render() {

		if (this._error) return html`<div class="uui-text"><uui-icon name="icon-alert"></uui-icon> ${this._error}</div>`;

		if (!this._ready) return html`<uui-loader></uui-loader>`;

		return html`
			<umb-property-editor-ui-block-list
				.value=${this.value ?? undefined}
				.config=${this.config ?? undefined}
				.name=${this.name ?? nothing}
				.readonly=${this.readonly}
				?mandatory=${this.mandatory}
				.mandatoryMessage=${this.mandatoryMessage}
				@change=${this.#onChange}>
			</umb-property-editor-ui-block-list>
		`;

	}

}

customElements.define('limbo-property-editor-ui-block-list', LimboPropertyEditorUiBlockListElement);

export default LimboPropertyEditorUiBlockListElement;
