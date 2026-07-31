// [CHANGE: Umbraco 17 upgrade] Related: see documentation/umbraco-17-upgrade.md

import { UmbFlatLayoutBlockPropertyValueCloner } from '@umbraco-cms/backoffice/block';
import { UMB_BLOCK_LIST_PROPERTY_EDITOR_SCHEMA_ALIAS } from '@umbraco-cms/backoffice/block-list';

/**
 * Gives blocks new keys when a property value is duplicated - eg. when copying content or creating a blueprint.
 *
 * Note that the layout key is Umbraco's own block list alias rather than the Limbo editor alias: the value editor is
 * inherited from Umbraco's, so the stored value is a "BlockListValue", which always writes its layout under
 * "Umbraco.BlockList".
 */
export class LimboBlockListPropertyValueCloner extends UmbFlatLayoutBlockPropertyValueCloner {

	constructor(args) {
		super(UMB_BLOCK_LIST_PROPERTY_EDITOR_SCHEMA_ALIAS, args);
	}

}

export { LimboBlockListPropertyValueCloner as api };

export default LimboBlockListPropertyValueCloner;
