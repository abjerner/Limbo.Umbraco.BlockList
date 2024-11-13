using System.Linq;
using Limbo.Umbraco.BlockList.Converters;
using Limbo.Umbraco.BlockList.Models.Api;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;

#pragma warning disable 1591

namespace Limbo.Umbraco.BlockList.Controllers;

[PluginController("Limbo")]
public class BlockListController : UmbracoAuthorizedApiController {

    private readonly BlockListTypeConverterCollection _converterCollection;

    public BlockListController(BlockListTypeConverterCollection converterCollection) {
        _converterCollection = converterCollection;
    }

    [HttpGet]
    public object GetTypeConverters() {
        return _converterCollection.Select(x => new BlockListTypeConverterApiModel(x));
    }

}