// [CHANGE: Umbraco 17 upgrade] Related: see documentation/umbraco-17-upgrade.md

import { UmbControllerBase } from '@umbraco-cms/backoffice/class-api';
import { UMB_BLOCK_LIST_PROPERTY_EDITOR_SCHEMA_ALIAS } from '@umbraco-cms/backoffice/block-list';

/**
 * Translates a block clipboard entry into a Limbo Block List property value.
 *
 * Mirrors Umbraco's own "UmbBlockToBlockListClipboardPastePropertyValueTranslator", which is only registered for the
 * "Umb.PropertyEditorUi.BlockList" property editor UI.
 */
export class LimboBlockToBlockListClipboardPastePropertyValueTranslator extends UmbControllerBase {

	async translate(value) {

		if (!value) throw new Error('Value is missing.');

		const valueClone = structuredClone(value);

		return {
			contentData: valueClone.contentData,
			settingsData: valueClone.settingsData,
			expose: [],
			layout: {
				[UMB_BLOCK_LIST_PROPERTY_EDITOR_SCHEMA_ALIAS]: valueClone.layout ?? undefined,
			},
		};

	}

	/**
	 * Only allow pasting blocks whose element types are actually allowed by this data type.
	 */
	async isCompatibleValue(propertyValue, config) {

		const allowedContentTypes =
			config.find((x) => x.alias === 'blocks')?.value.map((x) => x.contentElementTypeKey) ?? [];

		return propertyValue.contentData.map((x) => x.contentTypeKey)?.every((x) => allowedContentTypes.includes(x)) ?? false;

	}

}

export { LimboBlockToBlockListClipboardPastePropertyValueTranslator as api };

export default LimboBlockToBlockListClipboardPastePropertyValueTranslator;
