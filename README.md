# Limbo Block List

[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/abjerner/Limbo.Umbraco.BlockList/blob/v3/main/LICENSE.md)
[![NuGet](https://img.shields.io/nuget/vpre/Limbo.Umbraco.BlockList.svg)](https://www.nuget.org/packages/Limbo.Umbraco.BlockList)
[![NuGet](https://img.shields.io/nuget/dt/Limbo.Umbraco.BlockList.svg)](https://www.nuget.org/packages/Limbo.Umbraco.BlockList)
[![Limbo.Umbraco.BlockList at packages.limbo.works](https://img.shields.io/badge/limbo-packages-blue)](https://packages.limbo.works/limbo.umbraco.blocklist/)
[![Umbraco Marketplace](https://img.shields.io/badge/umbraco-marketplace-%233544B1)](https://marketplace.umbraco.com/package/limbo.umbraco.blocklist)

This package extends the default block list property editor in Umbraco, making it possible to control the CLR type returned by our version of the block list property editor.

<table>
  <tr>
    <td><strong>License:</strong></td>
    <td><a href="./LICENSE.md"><strong>MIT License</strong></a></td>
  </tr>
  <tr>
    <td><strong>Umbraco:</strong></td>
    <td>
      Umbraco 13
      <sub><sup>(and <a href="https://github.com/abjerner/Limbo.Umbraco.BlockList/tree/v3/main">Umbraco 10-12</a>, <a href="https://github.com/abjerner/Limbo.Umbraco.BlockList/tree/v2/main">Umbraco 9</a> and <a href="https://github.com/abjerner/Limbo.Umbraco.BlockList/tree/v1/main">Umbraco 8</a>)</sup></sub>
    </td>
  </tr>
  <tr>
    <td><strong>Target Framework:</strong></td>
    <td>
      .NET 8
      <sub><sup>(and <a href="https://github.com/abjerner/Limbo.Umbraco.BlockList/tree/v3/main">.NET 6</a>, <a href="https://github.com/abjerner/Limbo.Umbraco.BlockList/tree/v2/main">.NET 5</a> and <a href="https://github.com/abjerner/Limbo.Umbraco.BlockList/tree/v1/main">.NET 4.7.2</a>)</sup></sub>
    </td>
  </tr>
</table>



<br /><br />

## But why?

The default block list property editor exposes the value as an instance of `BlockListModel` representing the invividual blocks as they are added by users in the backoffice.

If you wish to interpret the `BlockListModel` a bit before rendering the block list on the website, there is a few different ways to go about this. With this package, you can select a *type converter* which is then used for converting `BlockListModel` to a desired type.

For us at [**@limbo-works**](https://github.com/limbo-works), we find this particular usefull as we can use a *type converter* to control the output for our headless API, thereby better being able to tailor the output for our frontenders.



<br /><br />

## Installation

The Umbraco 13 version of this package is only available via [**NuGet**](https://www.nuget.org/packages/Limbo.Umbraco.BlockList/13.0.0). To install the package, you can use either .NET CLI:

```
dotnet add package Limbo.Umbraco.BlockList --version 13.0.0
```

or the NuGet Package Manager:

```
Install-Package Limbo.Umbraco.BlockList -Version 13.0.0
```

**Umbraco 9**  
For the Umbraco 9 version of this package, see the [**v2/main**](https://github.com/abjerner/Limbo.Umbraco.BlockList/tree/v2/main) branch instead.

**Umbraco 8**  
For the Umbraco 8 version of this package, see the [**v1/main**](https://github.com/abjerner/Limbo.Umbraco.BlockList/tree/v1/main) branch instead.



<br /><br />

## Examples

To create your custom type converter, you can implement the `IBlockListTypeConverter` interface. In the example below, the type converter will return a flat array of `BlockItem` representing the individual block list items - or if the maximum amount of blocks it set to 1, an instance `BlockList`:

```csharp
using System;
using System.Linq;
using Limbo.Umbraco.BlockList.Converters;
using Limbo.Umbraco.BlockList.PropertyEditors;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace UmbracoTests.BlockList {
    
    public class BlockListTypeConverter : IBlockListTypeConverter {

        public string Name => "Block Item Converter";

        public Type GetType(IPublishedPropertyType propertyType, LimboBlockListConfiguration config) {
            return config.IsSinglePicker ? typeof(BlockItem) : typeof(IReadOnlyList<BlockItem>);
        }

        public object? Convert(IPublishedElement owner, IPublishedPropertyType propertyType, BlockListModel? source, LimboBlockListConfiguration config) {

            if (source == null) return config.IsSinglePicker ? null : Array.Empty<BlockItem>();

            if (!config.IsSinglePicker) return source.Select(x => new BlockItem(x)).ToArray();

            BlockListItem? first = source.FirstOrDefault();
            return first == null ? null : new BlockItem(first);

        }

    }

}
```

```csharp
using System;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Extensions;

namespace UmbracoTests.BlockList {
    
    public class BlockItem {

        public Guid ContentKey { get; }

        public string Type { get; }

        public IPublishedElement Content { get; }

        public BlockItem(BlockListItem blockListItem) {
            ContentKey = blockListItem.Content.Key;
            Type = blockListItem.Content.ContentType.Alias;
            Content = blockListItem.Content;
        }

    }

}
```

By creating your own class implementing the `IBlockListTypeConverter` interface, it will show up for the **Type Converter** option on the data type:

![image](https://user-images.githubusercontent.com/3634580/150651412-d623fe90-c459-4c73-9f67-75461ae448e0.png)
