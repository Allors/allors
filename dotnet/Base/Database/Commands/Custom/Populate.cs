// <copyright file="Populate.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Commands
{
    using Allors.Database.Domain;
    using Allors.Database.Services;
    using Allors.Fixture;
    using McMaster.Extensions.CommandLineUtils;
    using NLog;
    using Setup = Allors.Database.Domain.Setup;

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

            using (var session = database.CreateTransaction())
            {
                var automatedAgents = session.Scoped<AutomatedAgentByUniqueId>();
                var scheduler = automatedAgents.System;
                session.Services.Get<IUserService>().User = scheduler;

                new Allors.Database.Domain.Upgrade(session, this.Parent.DataPath).Execute();

                session.Derive();
                session.Commit();
            }

            this.Logger.Info("End");

            return ExitCode.Success;
        }
    }
}
