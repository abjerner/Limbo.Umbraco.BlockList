using System;
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
                alias = BlockListUtils.GetTypeAlias(x.GetType()),
                icon = x.Icon ?? $"icon-box color-{x.GetType().Assembly.FullName?.Split('.')[0].ToLower()}",
                name = x.Name,
                description = x.GetType().AssemblyQualifiedName?.Split(new[] { ", Version" }, StringSplitOptions.None)[0] + ".dll"
            });
        }

    }

}