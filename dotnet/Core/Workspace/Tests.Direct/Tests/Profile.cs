// <copyright file="Profile.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.Workspace.Direct
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Allors.Database.Adapters.Memory;
    using Allors.Database.Configuration;
    using Allors.Database.Domain;
    using Allors.Workspace;
    using Allors.Workspace.Adapters;
    using Allors.Workspace.Meta;
    using Person = Allors.Workspace.Domain.Person;
    using User = Allors.Database.Domain.User;

    public class Profile : IProfile
    {
        private User user;
        private readonly M metaPopulation;
        private readonly ReflectionObjectFactory objectFactory;

        public Database Database { get; }

        IConnection IProfile.Connection => this.Connection;

        public Allors.Workspace.Adapters.Direct.Connection Connection { get; private set; }

        public M M => (M)this.Connection.MetaPopulation;

        public Profile(Fixture fixture)
        {

            this.metaPopulation = new MetaBuilder().Build();
            this.objectFactory = new ReflectionObjectFactory(this.metaPopulation, typeof(Person));

            this.Database = new Database(
                new DefaultDatabaseServices(fixture.Engine),
                new Allors.Database.Adapters.Memory.Configuration
                {
                    ObjectFactory = new Allors.Database.ObjectFactory(fixture.M, typeof(Allors.Database.Domain.Person)),
                });

            this.Database.Init();

            this.Connection = new Allors.Workspace.Adapters.Direct.Connection(this.Database, "Default", this.metaPopulation, this.objectFactory);

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

        public IConnection CreateExclusiveWorkspaceConnection() => new Allors.Workspace.Adapters.Direct.Connection(this.Database, "Default", this.metaPopulation, this.objectFactory) { UserId = this.user.Id };

        public IConnection CreateWorkspaceConnection() => new Allors.Workspace.Adapters.Direct.Connection(this.Database, "Default", this.metaPopulation, this.objectFactory) { UserId = this.user.Id };

        public Task Login(string userName)
        {
            using var transaction = this.Database.CreateTransaction();
            this.user = new Users(transaction).Extent().ToArray().First(v => v.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));
            transaction.Services.Get<IUserService>().User = this.user;

            this.Connection.UserId = this.user.Id;

            return Task.CompletedTask;
        }
    }
}
