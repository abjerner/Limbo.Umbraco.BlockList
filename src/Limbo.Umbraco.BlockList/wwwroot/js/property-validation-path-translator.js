// [CHANGE: Umbraco 17 upgrade] Related: see documentation/umbraco-17-upgrade.md

import { UmbBlockEditorValidationPropertyPathTranslatorBase } from '@umbraco-cms/backoffice/block';

/**
 * Translates server side validation paths into client side data paths, so validation messages returned by the
 * Management API end up on the right block property. Mirrors Umbraco's own block list translator, which is only
 * registered for the "Umbraco.BlockList" editor alias.
 */
export class LimboBlockListValidationPropertyPathTranslator extends UmbBlockEditorValidationPropertyPathTranslatorBase {

	async translate(paths, data) {
		if (!data.value) return paths;
		paths = await this._translateBlockData(paths, data.value.contentData, '$.value.contentData');
		paths = await this._translateBlockData(paths, data.value.settingsData, '$.value.settingsData');
		return paths;
	}

}

export { LimboBlockListValidationPropertyPathTranslator as api };

export default LimboBlockListValidationPropertyPathTranslator;
