// <copyright file="Populate.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Commands
{
    using System.Collections.Generic;
    using System.Reflection;
    using Allors.Database.Domain;
    using Allors.Database.Meta;
    using Allors.Database.Population;
    using Allors.Population;
    using McMaster.Extensions.CommandLineUtils;
    using NLog;

    [Command(Description = "Add file contents to the index")]
    public class Populate
    {
        public Program Parent { get; set; }

        public Logger Logger => LogManager.GetCurrentClassLogger();

        public int OnExecute(CommandLineApplication app)
        {
            this.Logger.Info("Begin");

            var database = this.Parent.Database;

            database.Init();

            var recordsFromResource = new RecordsFromResource(database.MetaPopulation);
            var config = new Config { DataPath = this.Parent.DataPath, RecordsByClass = recordsFromResource.RecordsByClass };

            new Setup(database, config).Apply();

            using (var transaction = database.CreateTransaction())
            {
                new Allors.Database.Domain.Upgrade(transaction, this.Parent.DataPath).Execute();

                transaction.Derive();
                transaction.Commit();
            }

            this.Logger.Info("End");

            return ExitCode.Success;
        }
    }
}
