using Limbo.Umbraco.BlockList.Converters;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;

#pragma warning disable 1591

namespace Limbo.Umbraco.BlockList.Controllers {

    [PluginController("Limbo")]
    public class BlockListController : UmbracoAuthorizedApiController {

        private readonly BlockListTypeConverterCollection _converterCollection;

        public BlockListController(BlockListTypeConverterCollection converterCollection) {
            _converterCollection = converterCollection;
        }

        [HttpGet]
        public object GetTypeConverters() {
            return _converterCollection.ToArray().Select(x => new {
                assembly = x.GetType().Assembly.FullName,
                key = x.GetType().AssemblyQualifiedName,
                name = x.Name
            });
        }

    }

}