// <copyright file="ConfigurationBuilderExtensions.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the DomainTest type.</summary>
namespace Allors.Database.Adapters.Sql
{
    using System.Runtime.InteropServices;
    using Microsoft.Extensions.Configuration;

    /// <remark>
    /// When xunit will support generic IClassFixture change
    /// implementation so that only Fixture will provide the Config
    /// (now both Fixture and Profile do)
    /// https://github.com/xunit/xunit/issues/2557
    /// </remark>
    public class Config
    {
        private const string Path = "/config/system";

        public Config()
        {
            var platform = "other";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                platform = "windows";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                platform = "linux";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                platform = "osx";
            }

            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile(System.IO.Path.Combine(Path, "appSettings.json"), true);
            configurationBuilder.AddJsonFile(System.IO.Path.Combine(Path, $"appSettings.{platform}.json"), true);
            configurationBuilder.AddEnvironmentVariables();

            this.Root = configurationBuilder.Build();
        }

        public IConfigurationRoot Root { get; }
    }
}
