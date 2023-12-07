﻿// <copyright file="Populate.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Commands
{
    using Allors.Database.Domain;
    using Allors.Fixture;
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

            var config = new Config { DataPath = this.Parent.DataPath };
            var fixture = new FixtureResource(database.MetaPopulation).Read();
            new Setup(database, fixture, config).Apply();

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
