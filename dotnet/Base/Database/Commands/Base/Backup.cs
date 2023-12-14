// <copyright file="Restore.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Commands
{
    using System.IO;
    using System.Xml;
    using McMaster.Extensions.CommandLineUtils;
    using NLog;

    [Command(Description = "Make a backup of the database")]
    public class Backup
    {
        public Program Parent { get; set; }

        public Logger Logger => LogManager.GetCurrentClassLogger();

        [Option("-f", Description = "Backup file")]
        public string FileName { get; set; }

        public int OnExecute(CommandLineApplication app)
        {
            this.Logger.Info("Begin");

            var fileName = this.FileName ?? this.Parent.Configuration["backupFile"] ?? "backup.xml";
            var fileInfo = new FileInfo(fileName);

            using (var stream = File.Create(fileInfo.FullName))
            {
                using (var writer = XmlWriter.Create(stream))
                {
                    this.Logger.Info("Saving {file}", fileInfo.FullName);
                    this.Parent.Database.Backup(writer);
                }
            }

            this.Logger.Info("End");
            return ExitCode.Success;
        }
    }
}
