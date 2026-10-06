import { css, html, nothing, when } from "@umbraco-cms/backoffice/external/lit";
import { UmbLitElement } from "@umbraco-cms/backoffice/lit-element";
import { UmbChangeEvent } from "@umbraco-cms/backoffice/event";
import { umbHttpClient } from "@umbraco-cms/backoffice/http-client";
import { UMB_ITEM_PICKER_MODAL, umbOpenModal } from "@umbraco-cms/backoffice/modal";

const ENDPOINT = "/umbraco/management/api/v1/limbo/block-list/type-converters";

async function getConverters() {

	// "security" is required. The backoffice HTTP client only resolves its "auth" callback - and thus only
	// sets the Authorization header - for requests that declare a security scheme. Without it the request is
	// sent unauthenticated, the Management API answers 401, and the backoffice interceptor reacts by
	// restarting the authorization flow, which logs the user out.
	return await umbHttpClient.get({
		url: ENDPOINT,
		security: [{ scheme: "bearer", type: "http" }],
	});

}

/**
 * Data type configuration UI for picking one of the registered "IBlockListTypeConverter" implementations.
 */
export class LimboTypeConverterPropertyEditorUiElement extends UmbLitElement {

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
			uui-button[look="placeholder"] {
				width: 100%;
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
		const type = typeof value === "string" ? value : (value.type ?? value.key);
		return type ? type.split(", Version")[0] : undefined;
	}

	async #load() {

		try {
			const response = await getConverters();
			this._converters = response?.data ?? [];
		} catch (error) {
			console.error("[Limbo Block List] Failed loading the available type converters.", error);
			this._converters = [];
		}

		this._loading = false;

		const selected = this.#selectedType;
		this._notFound = !!selected && !this._converters.some((x) => x.type === selected);

	}

	async #onAdd() {

		if (this.readonly) return;

		const converters = this._converters ?? [];

		if (!converters.length) return;

		const picked = await umbOpenModal(this, UMB_ITEM_PICKER_MODAL, {
			data: {
				headline: "Select converter",
				items: converters.map((converter) => ({
					label: converter.name,
					description: converter.description ?? undefined,
					icon: converter.icon,
					value: converter.type,
				})),
			},
			modal: {
				size: "medium"
			}
		}).catch(() => undefined);

		if (!picked?.value) return;

		this.value = { type: picked.value };
		this.dispatchEvent(new UmbChangeEvent());

	}

	#onRemove() {
		this.value = undefined;
		this.dispatchEvent(new UmbChangeEvent());
	}

	render() {

		if (this._loading) return html`<uui-loader></uui-loader>`;

		if (!this.value) {
			if (!this._converters?.length) return html`<div class="message">No converters are registered on the server.</div>`;
			return html`
				<uui-button
					look="placeholder"
					label=${this.localize.term("general_add")}
					@click=${this.#onAdd}>
					${this.localize.term("general_add")}
				</uui-button>
			`;
		}

		const selected = this.#selectedType;

		return html`
			${when(this._notFound, () => html`
				<div class="message error">
					The selected type
					<strong>${this.value.type}</strong>
					could not be found.
				</div>
			`)}
			<uui-ref-node
				standalone
				name=${this.selected?.name ?? this.value.type}
				detail=${this.selected?.description ?? ""}
				?readonly=${this.readonly}
				@open=${this.#onAdd}>
					${this.selected?.icon ? html`<umb-icon slot="icon" name=${this.selected.icon}></umb-icon>` : nothing}
					${when(!this.readonly, () => html`
						<uui-action-bar slot="actions">
							<uui-button label=${this.localize.term("general_remove")} color="danger" @click=${this.#onRemove}></uui-button>
						</uui-action-bar>
					`)}
			</uui-ref-node>

		`;

	}

}

customElements.define("limbo-block-list-type-converter-property-editor-ui", LimboTypeConverterPropertyEditorUiElement);

export default LimboTypeConverterPropertyEditorUiElement;
