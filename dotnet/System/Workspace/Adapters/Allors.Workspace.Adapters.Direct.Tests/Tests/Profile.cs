// <copyright file="Profile.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using Allors.Workspace.Adapters.Tests;
using Allors.Workspace.Meta.Static;

namespace Allors.Workspace.Adapters.Direct.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Allors.Database;
    using Allors.Database.Adapters.Memory;
    using Allors.Database.Configuration;
    using Allors.Database.Services;
    using Allors.Workspace;
    using Allors.Workspace.Configuration;
    using Allors.Workspace.Meta;
    using Database.Domain;
    using Database.Population;
    using Database.Population.Resx;
    using Configuration = Allors.Workspace.Adapters.Direct.Configuration;
    using Connection = Direct.Connection;
    using IClass = Database.Meta.Class;
    using IWorkspaceServices = Allors.Workspace.IWorkspaceServices;
    using Person = Allors.Workspace.Domain.Person;
    using Task = System.Threading.Tasks.Task;
    using User = Allors.Database.Domain.User;

    public class Profile : IProfile
    {
        private readonly Func<IWorkspaceServices> servicesBuilder;
        private readonly Configuration configuration;

        private User user;

        public Database Database { get; }

        public Connection Connection { get; private set; }

        IWorkspace IProfile.Workspace => this.Workspace;

        public IWorkspace Workspace { get; private set; }

        public M M => this.Workspace.Services.Get<M>();

        public Profile(Fixture fixture)
        {
            var metaPopulation = new MetaBuilder().Build();
            var objectFactory = new ReflectionObjectFactory(metaPopulation, typeof(Person));

            this.servicesBuilder = () => new WorkspaceServices(objectFactory, metaPopulation);
            this.configuration = new Configuration("Default", metaPopulation);

            this.Database = new Database(
                new DefaultDatabaseServices(fixture.Engine, fixture.M),
                new Allors.Database.Adapters.Memory.Configuration
                {
                    ObjectFactory = new ObjectFactory(fixture.M.MetaPopulation, typeof(Allors.Database.Domain.Person)),
                });

            this.Database.Init();

            var config = new Config
            {
                RecordsByClass = new RecordsFromResource(this.Database.MetaPopulation).RecordsByClass,
                Translation = new TranslationsFromResource(this.Database.MetaPopulation, new TranslationConfiguration())
            };

            new Setup(this.Database, config).Apply();

            using var transaction = this.Database.CreateTransaction();

            var administrator = transaction.Build<Allors.Database.Domain.Person>(v => v.UserName = "administrator");
            transaction.Scoped<UserGroupByUniqueId>().Administrators.AddMember(administrator);
            transaction.Services.Get<IUserService>().User = administrator;

            new TestPopulation(transaction).Apply();
            transaction.Derive();
            transaction.Commit();
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public Task DisposeAsync() => Task.CompletedTask;

        public IWorkspace CreateExclusiveWorkspace()
        {
            var connection = new Connection(this.configuration, this.Database, this.servicesBuilder) { UserId = this.user.Id };
            return connection.CreateWorkspace();
        }
        public IWorkspace CreateSharedWorkspace() => this.Connection.CreateWorkspace();

        public Task Login(string userName)
        {
            using var transaction = this.Database.CreateTransaction();
            this.user = transaction.Extent<User>().ToArray().First(v => v.UserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase));
            transaction.Services.Get<IUserService>().User = this.user;

            this.Connection = new Connection(this.configuration, this.Database, this.servicesBuilder) { UserId = this.user.Id };

            this.Workspace = this.Connection.CreateWorkspace();

            return Task.CompletedTask;
        }
    }
}
