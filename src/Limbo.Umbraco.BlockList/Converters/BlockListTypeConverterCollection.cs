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
                string? typeName = item.GetType().AssemblyQualifiedName;
                if (typeName != null && _lookup.ContainsKey(typeName) == false) {
                    _lookup.Add(typeName, item);
                }
            }
        }

        public bool TryGet(string typeName, [NotNullWhen(true)] out IBlockListTypeConverter? item) {
            return _lookup.TryGetValue(typeName, out item);
        }

    }

}