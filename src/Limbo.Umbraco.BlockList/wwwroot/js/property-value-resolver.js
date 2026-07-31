// [CHANGE: Umbraco 17 upgrade] Related: see documentation/umbraco-17-upgrade.md

// Umbraco registers its own value resolver for the "Umbraco.BlockList" editor alias only, so the Limbo editor alias
// needs its own registration. The behaviour is identical, hence the plain re-export.
export { UmbStandardBlockValueResolver as api } from '@umbraco-cms/backoffice/block';
