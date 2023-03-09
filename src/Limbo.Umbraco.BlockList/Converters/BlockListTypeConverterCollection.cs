using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Umbraco.Cms.Core.Composing;

#pragma warning disable 1591

namespace Limbo.Umbraco.BlockList.Converters {

    /// <summary>
    /// Collection of <see cref="IBlockListTypeConverter"/>.
    /// </summary>
    public sealed class BlockListTypeConverterCollection : BuilderCollectionBase<IBlockListTypeConverter> {

        private readonly Dictionary<string, IBlockListTypeConverter> _lookup;

        public BlockListTypeConverterCollection(Func<IEnumerable<IBlockListTypeConverter>> items) : base(items) {
            _lookup = new Dictionary<string, IBlockListTypeConverter>(StringComparer.OrdinalIgnoreCase);
            foreach (IBlockListTypeConverter item in this) {
                string? typeAlias = item.Alias;
                if (typeAlias != null && _lookup.ContainsKey(typeAlias) == false) {
                    _lookup.Add(typeAlias, item);
                }
            }
        }

        public bool TryGet(string typeName, [NotNullWhen(true)] out IBlockListTypeConverter? item) {
            return _lookup.TryGetValue(BlockListUtils.RemoveVersion(typeName), out item);
        }

    }

}