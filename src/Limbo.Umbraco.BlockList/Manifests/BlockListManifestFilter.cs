using System.Collections.Generic;
using Skybrud.Essentials.Strings.Extensions;
using Umbraco.Cms.Core.Manifest;

namespace Limbo.Umbraco.BlockList.Manifests {

    /// <inheritdoc />
    public class BlockListManifestFilter : IManifestFilter {

        /// <inheritdoc />
        public void Filter(List<PackageManifest> manifests) {
            manifests.Add(new PackageManifest {
                PackageName = BlockListPackage.Alias.ToKebabCase(),
                BundleOptions = BundleOptions.Independent,
                Scripts = new[] {
                    $"/App_Plugins/{BlockListPackage.Alias}/TypeConverter.js",
                    $"/App_Plugins/{BlockListPackage.Alias}/TypeConverterOverlay.js"
                },
                Stylesheets = new[] {
                    $"/App_Plugins/{BlockListPackage.Alias}/Styles.css"
                }
            });
        }

    }

}