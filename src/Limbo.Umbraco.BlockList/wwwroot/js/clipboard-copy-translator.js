// [CHANGE: Umbraco 17 upgrade] Related: see documentation/umbraco-17-upgrade.md

import { UmbControllerBase } from '@umbraco-cms/backoffice/class-api';
import { UMB_BLOCK_LIST_PROPERTY_EDITOR_SCHEMA_ALIAS } from '@umbraco-cms/backoffice/block-list';

/**
 * Translates a Limbo Block List property value into a block clipboard entry.
 *
 * Mirrors Umbraco's own "UmbBlockListToBlockClipboardCopyPropertyValueTranslator", which is only registered for the
 * "Umb.PropertyEditorUi.BlockList" property editor UI.
 */
export class LimboBlockListToBlockClipboardCopyPropertyValueTranslator extends UmbControllerBase {

	async translate(propertyValue) {

		if (!propertyValue) throw new Error('Property value is missing.');

		const valueClone = structuredClone(propertyValue);

		// The layout key is Umbraco's own block list alias - the value editor is inherited from Umbraco's, so the
		// stored value is a "BlockListValue", which always writes its layout under "Umbraco.BlockList"
		const layout = valueClone.layout?.[UMB_BLOCK_LIST_PROPERTY_EDITOR_SCHEMA_ALIAS] ?? undefined;

		layout?.forEach((layoutItem) => {
			delete layoutItem.$type;
		});

		return {
			contentData: valueClone.contentData ?? [],
			layout,
			settingsData: valueClone.settingsData ?? [],
		};

	}

}

export { LimboBlockListToBlockClipboardCopyPropertyValueTranslator as api };

export default LimboBlockListToBlockClipboardCopyPropertyValueTranslator;
