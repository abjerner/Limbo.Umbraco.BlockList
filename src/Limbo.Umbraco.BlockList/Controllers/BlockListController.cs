using Limbo.Umbraco.BlockList.Converters;
using System.Linq;
using System.Web.Http;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

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
            return Json(_converterCollection.ToArray().Select(x => new {
                assembly = x.GetType().Assembly.FullName,
                key = x.GetType().AssemblyQualifiedName,
                name = x.Name
            }));
        }

    }

}