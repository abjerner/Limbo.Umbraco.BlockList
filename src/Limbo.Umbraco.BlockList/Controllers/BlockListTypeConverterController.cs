// [CHANGE: Umbraco 17 upgrade] Related: see documentation/umbraco-17-upgrade.md

using System.Collections.Generic;
using System.Linq;
using Asp.Versioning;
using Limbo.Umbraco.BlockList.Converters;
using Limbo.Umbraco.BlockList.Models.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Api.Management.Controllers;
using Umbraco.Cms.Api.Management.Routing;

#pragma warning disable 1591

namespace Limbo.Umbraco.BlockList.Controllers;

/// <summary>
/// Management API controller exposing the registered <see cref="IBlockListTypeConverter"/> implementations to the
/// backoffice.
/// </summary>
/// <remarks>
/// Umbraco 14 replaced the AngularJS backoffice - and with it <c>UmbracoAuthorizedApiController</c> and
/// <c>[PluginController]</c> - with the Management API. Endpoints are served from
/// <c>/umbraco/management/api/v1/limbo/block-list/</c> and are secured by the backoffice access policy inherited
/// from <see cref="ManagementApiControllerBase"/>.
/// </remarks>
[ApiVersion("1.0")]
[VersionedApiBackOfficeRoute("limbo/block-list")]
public class BlockListTypeConverterController : ManagementApiControllerBase {

    private readonly BlockListTypeConverterCollection _converterCollection;

    public BlockListTypeConverterController(BlockListTypeConverterCollection converterCollection) {
        _converterCollection = converterCollection;
    }

    [HttpGet("type-converters")]
    [ProducesResponseType(typeof(IEnumerable<BlockListTypeConverterApiModel>), StatusCodes.Status200OK)]
    public IActionResult TypeConverters() {
        return Ok(_converterCollection.Select(x => new BlockListTypeConverterApiModel(x)));
    }

}
