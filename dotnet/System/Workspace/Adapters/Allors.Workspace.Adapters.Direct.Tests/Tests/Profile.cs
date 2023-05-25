// <copyright file="Profile.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using Allors.Workspace.Adapters.Tests;
using Allors.Workspace.Meta.Static;

namespace Allors.Workspace.Adapters.Direct.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Allors.Database;
    using Allors.Database.Adapters.Memory;
    using Allors.Database.Configuration;
    using Allors.Database.Domain;
    using Allors.Database.Services;
    using Allors.Workspace;
    using Allors.Workspace.Adapters;
    using Allors.Workspace.Meta;
    using Configuration = Allors.Workspace.Adapters.Direct.Configuration;
    using Connection = Direct.Connection;
    using IWorkspaceServices = Allors.Workspace.IWorkspaceServices;
    using Person = Allors.Workspace.Domain.Person;
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
                new DefaultDatabaseServices(fixture.Engine),
                new Allors.Database.Adapters.Memory.Configuration
                {
                    ObjectFactory = new ObjectFactory(fixture.M, typeof(Allors.Database.Domain.Person)),
                });

            this.Database.Init();

            var config = new Config();
            new Setup(this.Database, config).Apply();

            using var transaction = this.Database.CreateTransaction();

            var administrator = transaction.Build<Allors.Database.Domain.Person>(v => v.UserName = "administrator");
            new UserGroups(transaction).Administrators.AddMember(administrator);
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
            this.user = new Users(transaction).Extent().ToArray().First(v => v.UserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase));
            transaction.Services.Get<IUserService>().User = this.user;

            this.Connection = new Connection(this.configuration, this.Database, this.servicesBuilder) { UserId = this.user.Id };

            this.Workspace = this.Connection.CreateWorkspace();
            
            return Task.CompletedTask;
        }
    }
}
