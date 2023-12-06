// <copyright file="Restore.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Commands
{
    using System;
    using System.IO;
    using Allors.Database;
    using Allors.Database.Meta;
    using Allors.Database.Fixture;
    using Allors.Database.Fixture.Xml;
    using Allors.Database.Roundtrip;
    using Allors.Fixture;
    using McMaster.Extensions.CommandLineUtils;
    using NLog;

    [Command(Description = "Roundtrip to population file")]
    public class Roundtrip
    {
        public Program Parent { get; set; }

        public Logger Logger => LogManager.GetCurrentClassLogger();

        [Option("-f", Description = "population file")]
        public string FileName { get; set; } = "population.xml";

        public int OnExecute(CommandLineApplication app)
        {
            this.Logger.Info("Begin");

            var fileName = this.FileName ?? this.Parent.Configuration["populationFile"];
            var fileInfo = new FileInfo(fileName);

            this.Logger.Info("Saving {file}", fileInfo.FullName);

            var database = this.Parent.Database;

            var fixtureFile = new FixtureFile(database, fileInfo);
            fixtureFile.Write();

            this.Logger.Info("End");
            return ExitCode.Success;
        }
    }
}
