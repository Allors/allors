// <copyright file="ConfigurationBuilderExtensions.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the DomainTest type.</summary>

namespace Allors.Database.Adapters.Sql
{
    using System.Runtime.InteropServices;
    using Microsoft.Extensions.Configuration;

    public abstract class FixtureBase<T>
    {

        private const string Path = "/config/system";

        private const string DatabaseParam = "[database]";

        protected FixtureBase()
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

            this.Configuration = configurationBuilder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        public string Database => typeof(T).Name;

        public string ConnectionString
        {
            get
            {
                var connectionString = this.Configuration[ConnectionStringKey];
                connectionString = connectionString.Replace(DatabaseParam, this.Database);
                return connectionString;
            }
        }

        public abstract string ConnectionStringKey { get; }
    }
}
