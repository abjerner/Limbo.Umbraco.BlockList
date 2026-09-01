// [CHANGE: Umbraco 17 upgrade] Related: see documentation/umbraco-17-upgrade.md

import { css, html, nothing } from '@umbraco-cms/backoffice/external/lit';
import { UmbLitElement } from '@umbraco-cms/backoffice/lit-element';
import { UmbChangeEvent } from '@umbraco-cms/backoffice/event';
import { umbHttpClient } from '@umbraco-cms/backoffice/http-client';

const ENDPOINT = '/umbraco/management/api/v1/limbo/block-list/type-converters';

/**
 * Data type configuration UI for picking one of the registered "IBlockListTypeConverter" implementations.
 *
 * Replaces the AngularJS "TypeConverter.html" view and its editor service overlay.
 */
export class LimboPropertyEditorUiTypeConverterElement extends UmbLitElement {

	static properties = {
		value: { attribute: false },
		readonly: { type: Boolean, reflect: true },
		_converters: { state: true },
		_loading: { state: true },
		_notFound: { state: true },
	};

	static styles = [
		css`
			:host {
				display: block;
			}
			#description {
				color: var(--uui-color-text-alt);
				font-size: var(--uui-type-small-size);
				margin-top: var(--uui-size-space-2);
				word-break: break-all;
			}
		`,
	];

	constructor() {
		super();
		this.readonly = false;
		this._converters = [];
		this._loading = true;
		this._notFound = false;
	}

	connectedCallback() {
		super.connectedCallback();
		this.#load();
	}

	/**
	 * Returns the version-less assembly qualified name of the currently selected type converter, if any.
	 *
	 * The value has been persisted in a few different shapes over the years - a plain string, an object with a
	 * "key" property, and (currently) an object with a "type" property - so all three are accepted here.
	 */
	get #selectedType() {
		const value = this.value;
		if (!value) return undefined;
		const type = typeof value === 'string' ? value : (value.type ?? value.key);
		return type ? type.split(', Version')[0] : undefined;
	}

	async #load() {

		try {
			// "security" is required. The backoffice HTTP client only resolves its "auth" callback - and thus only
			// sets the Authorization header - for requests that declare a security scheme. Without it the request is
			// sent unauthenticated, the Management API answers 401, and the backoffice interceptor reacts by
			// restarting the authorization flow, which logs the user out.
			const response = await umbHttpClient.get({
				url: ENDPOINT,
				security: [{ scheme: 'bearer', type: 'http' }],
			});
			this._converters = response?.data ?? [];
		} catch (error) {
			console.error('[Limbo Block List] Failed loading the available type converters.', error);
			this._converters = [];
		}

		this._loading = false;

		const selected = this.#selectedType;
		this._notFound = !!selected && !this._converters.some((x) => x.type === selected);

	}

	#onChange(event) {
		const type = event.target.value;
		this.value = type ? { type } : undefined;
		this._notFound = false;
		this.dispatchEvent(new UmbChangeEvent());
	}

	#renderDescription() {
		const selected = this.#selectedType;
		if (!selected) return nothing;
		return html`<div id="description">${selected}</div>`;
	}

	render() {

		if (this._loading) return html`<uui-loader></uui-loader>`;

		const selected = this.#selectedType;

		const options = [
			{ name: 'None', value: '', selected: !selected },
			...this._converters.map((x) => ({ name: x.name, value: x.type, selected: x.type === selected })),
		];

		return html`
			${this._notFound
				? html`<uui-box><strong>The selected type converter could not be found:</strong> ${selected}</uui-box>`
				: nothing}
			<uui-select
				label="Type converter"
				.options=${options}
				?disabled=${this.readonly}
				@change=${this.#onChange}></uui-select>
			${this.#renderDescription()}
		`;

	}

}

customElements.define('limbo-property-editor-ui-block-list-type-converter', LimboPropertyEditorUiTypeConverterElement);

export default LimboPropertyEditorUiTypeConverterElement;
