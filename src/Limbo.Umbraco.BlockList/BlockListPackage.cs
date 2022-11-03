﻿using System;
using System.Diagnostics;
using Umbraco.Cms.Core.Semver;

namespace Limbo.Umbraco.BlockList {

    /// <summary>
    /// Static class with various information and constants about the package.
    /// </summary>
    public static class BlockListPackage {

        /// <summary>
        /// Gets the alias of the package.
        /// </summary>
        public const string Alias = "Limbo.Umbraco.BlockList";

        /// <summary>
        /// Gets the friendly name of the package.
        /// </summary>
        public const string Name = "Limbo Block List";

        /// <summary>
        /// Gets the version of the package.
        /// </summary>
        public static readonly Version Version = typeof(BlockListPackage).Assembly.GetName().Version!;

        /// <summary>
        /// Gets the informational version of the package.
        /// </summary>
        public static readonly string InformationalVersion = FileVersionInfo.GetVersionInfo(typeof(BlockListPackage).Assembly.Location).ProductVersion!;

        /// <summary>
        /// Gets the semantic version of the package.
        /// </summary>
        public static readonly SemVersion SemVersion = InformationalVersion;

    }

}