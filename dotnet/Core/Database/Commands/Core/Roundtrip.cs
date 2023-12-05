// <copyright file="Restore.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Commands
{
    using System.IO;
    using System.Linq;
    using System.Xml;
    using Allors.Database.Domain;
    using Allors.Database.Meta;
    using Allors.Database.Roundtrip;
    using Allors.Resources;
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
            using var transaction = database.CreateTransaction();

            var m = database.Services.Get<M>();

            using var stream = File.OpenWrite(fileInfo.FullName);

            var fixture = transaction.ToFixture();
            var fixtureWriter = new FixtureWriter(m);
            fixtureWriter.Write(stream, fixture);
         
            this.Logger.Info("End");
            return ExitCode.Success;
        }
    }
}
