// <copyright file="Restore.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Commands
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Allors.Database.Domain;
    using Allors.Database.Meta;
    using Allors.Database.Population;
    using Allors.Database.Population.Resx;
    using McMaster.Extensions.CommandLineUtils;
    using NLog;

    [Command(Description = "Roundtrip records to file")]
    public class Roundtrip
    {
        public Program Parent { get; set; }

        public Logger Logger => LogManager.GetCurrentClassLogger();

        [Option("-d", Description = "directory")]
        public string DirectoryName { get; set; }

        public int OnExecute(CommandLineApplication app)
        {
            var database = this.Parent.Database;
            var m = database.Services.Get<M>();

            this.Logger.Info("Begin");

            var directory = this.DirectoryName ?? this.Parent.Configuration["roundtrip"] ?? "../../../../Population";
            var directoryInfo = new DirectoryInfo(directory);

            this.Logger.Info("Records");

            var recordsFileInfo = new FileInfo(Path.Combine(directoryInfo.FullName, "Records.xml"));

            this.Logger.Info("Saving {file}", recordsFileInfo.FullName);

            var recordsFromFile = new RecordsFromFile(recordsFileInfo, database.MetaPopulation);
            var roundtrip = new RecordRoundtripStrategy(database, recordsFromFile.RecordsByClass);
            var recordsToFile = new RecordsToFile(recordsFileInfo, database.MetaPopulation, roundtrip);
            recordsToFile.Roundtrip();

            this.Logger.Info("Translations");

            using var transaction = database.CreateTransaction();
            var enumerations = transaction.Extent<Enumeration>();

            // IDictionary<IClass, IDictionary< string, Translations[]>>
            var translationsByIsoCodeByClass = enumerations
                .GroupBy(v => v.Strategy.Class)
                .ToDictionary(v => v.Key, v => v
                    .SelectMany(w => w.LocalisedNames)
                    .Select(w => new Translation(v.Key, w.Locale.Key, w.EnumerationWhereLocalisedName.Key, w.Text))
                    .GroupBy(w => w.IsoCode)
                    .ToDictionary(w => w.Key, w => new Translations(v.Key, w.Key, w.ToDictionary(x => x.Key, x => x.Value))) as IDictionary<string, Translations>
                );

            var translationsToFile = new TranslationsToFile(directoryInfo, translationsByIsoCodeByClass, m.Enumeration.LocalisedNames);

            translationsToFile.Roundtrip();

            this.Logger.Info("End");
            return ExitCode.Success;
        }
    }
}
