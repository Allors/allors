// <copyright file="Import.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Commands
{
    using System.IO;
    using System.Xml;
    using McMaster.Extensions.CommandLineUtils;
    using NLog;

    [Command(Description = "Import the population from file")]
    public class Restore
    {
        public Program Parent { get; set; }

        public Logger Logger => LogManager.GetCurrentClassLogger();

        [Option("-f", Description = "Backup file (default is population.xml)")]
        public string FileName { get; set; }

        public int OnExecute(CommandLineApplication app)
        {
            this.Logger.Info("Begin");

            var fileName = this.FileName ?? this.Parent.Configuration["populationFile"] ?? "population.xml";
            var fileInfo = new FileInfo(fileName);

            using (var reader = XmlReader.Create(fileInfo.FullName))
            {
                this.Logger.Info("Restoring {file}", fileInfo.FullName);
                this.Parent.Database.Restore(reader);
            }

            this.Logger.Info("End");
            return ExitCode.Success;
        }
    }
}
