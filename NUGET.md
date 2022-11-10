[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE.md) [![NuGet](https://img.shields.io/nuget/vpre/Limbo.Umbraco.BlockList.svg)](https://www.nuget.org/packages/Limbo.Umbraco.BlockList) [![NuGet](https://img.shields.io/nuget/dt/Limbo.Umbraco.BlockList.svg)](https://www.nuget.org/packages/Limbo.Umbraco.BlockList) [![Our Umbraco](https://img.shields.io/badge/our-umbraco-%233544B1)](https://our.umbraco.com/packages/developer-tools/limbo-block-list/)

`Limbo.Umbraco.BlockList` This package extends the default block list property editor in Umbraco, making it possible to control the CLR type returned by our version of the block list property editor.

## But why?

The default block list property editor exposes the value as an instance of `BlockListModel` representing the individual blocks as they are added by users in the backoffice.

If you wish to interpret the `BlockListModel` a bit before rendering the block list on the website, there is a few different ways to go about this. With this package, you can select a *type converter* which is then used for converting a `BlockListModel` instance to a desired type.

For us at [**@limbo-works**](https://github.com/limbo-works), we find this particular useful as we can use a *type converter* to control the output for our headless API, thereby better being able to tailor the output for our frontenders.

## Screenshots

![image](https://user-images.githubusercontent.com/3634580/150651412-d623fe90-c459-4c73-9f67-75461ae448e0.png)