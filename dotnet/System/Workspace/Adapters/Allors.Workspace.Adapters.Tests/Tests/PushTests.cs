// <copyright file="SaveTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Tests
{
    using System.Linq;
    using Allors.Workspace;
    using Allors.Workspace.Data;
    using Allors.Workspace.Domain;
    using Xunit;
    using Version = Allors.Version;

    public abstract class PushTests : Test
    {
        protected PushTests(Fixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async void PushNewObject()
        {
            await this.Login("administrator");

            var workspace = this.Workspace;

            var newObject = workspace.Create<C1>();

            var result = await workspace.PushAsync();
            Assert.False(result.HasErrors);

            foreach (var roleType in this.M.C1.RoleTypes)
            {
                Assert.True(newObject.Strategy.CanRead(roleType));
                Assert.False(newObject.Strategy.ExistRole(roleType));
            }

            foreach (var associationType in this.M.C1.AssociationTypes)
            {
                if (associationType.IsOne)
                {
                    var association = newObject.Strategy.GetCompositeAssociation(associationType);
                    Assert.Null(association);
                }
                else
                {
                    var association = newObject.Strategy.GetCompositesAssociation(associationType);
                    Assert.Empty(association);
                }
            }
        }

        [Fact]
        public async void PushAndPullNewObject()
        {
            await this.Login("administrator");

            var workspace = this.Workspace;

            var newObject = workspace.Create<C1>();

            var result = await workspace.PushAsync();
            Assert.False(result.HasErrors);

            await workspace.PullAsync(new Pull { Object = newObject.Strategy });

            foreach (var roleType in this.M.C1.RoleTypes)
            {
                Assert.True(newObject.Strategy.CanRead(roleType));
                Assert.False(newObject.Strategy.ExistRole(roleType));
            }

            foreach (var associationType in this.M.C1.AssociationTypes)
            {
                if (associationType.IsOne)
                {
                    var association = newObject.Strategy.GetCompositeAssociation(associationType);
                    Assert.Null(association);
                }
                else
                {
                    var association = newObject.Strategy.GetCompositesAssociation(associationType);
                    Assert.Empty(association);
                }
            }
        }

        [Fact]
        public async void PushNewObjectWithChangedRoles()
        {
            await this.Login("administrator");

            var workspace = this.Workspace;

            var newObject = workspace.Create<C1>();
            newObject.C1AllorsString.Value = "A new object";

            var result = await workspace.PushAsync();
            Assert.False(result.HasErrors);

            await workspace.PullAsync(new Pull { Object = newObject.Strategy });

            Assert.Equal("A new object", newObject.C1AllorsString.Value);
        }

        [Fact]
        public async void PushExistingObjectWithChangedRoles()
        {
            await this.Login("administrator");

            var workspace = this.Workspace;

            var pull = new Pull
            {
                Extent = new Filter(this.M.C1)
                {
                    Predicate = new Equals(this.M.C1.Name) { Value = "c1A" }
                }
            };

            var result = await workspace.PullAsync(pull);
            var c1a = result.GetCollection<C1>()[0];

            c1a.C1AllorsString.Value = "X";

            Assert.Equal("X", c1a.C1AllorsString.Value);

            await workspace.PushAsync();
            await workspace.PullAsync(pull);

            Assert.Equal("X", c1a.C1AllorsString.Value);
        }

        [Fact]
        public async void PushShouldUpdateId()
        {
            await this.Login("administrator");

            var workspace = this.Workspace;

            var person = workspace.Create<Person>();
            person.FirstName.Value = "Johny";
            person.LastName.Value = "Doey";

            Assert.True(person.Id < 0);

            Assert.False((await workspace.PushAsync()).HasErrors);

            Assert.True(person.Id > 0);
        }

        [Fact]
        public async void PushShouldNotUpdateVersion()
        {
            await this.Login("administrator");

            var workspace = this.Workspace;

            var person = workspace.Create<Person>();
            person.FirstName.Value = "Johny";
            person.LastName.Value = "Doey";

            Assert.Equal(Version.WorkspaceInitial.Value, person.Strategy.Version);

            Assert.False((await workspace.PushAsync()).HasErrors);

            Assert.Equal(Version.WorkspaceInitial.Value, person.Strategy.Version);
        }

        [Fact]
        public async void PushShouldDerive()
        {
            await this.Login("administrator");

            var workspace = this.Workspace;

            var person = workspace.Create<Person>();
            person.FirstName.Value = "Johny";
            person.LastName.Value = "Doey";

            Assert.False((await workspace.PushAsync()).HasErrors);

            var pull = new Pull
            {
                Object = person.Strategy
            };

            Assert.False((await workspace.PullAsync(pull)).HasErrors);

            Assert.Equal("Johny Doey", person.DomainFullName.Value);
        }

        [Fact]
        public async void PushTwice()
        {
            await this.Login("administrator");

            var workspace = this.Workspace;

            var c1x = workspace.Create<C1>();
            var c1y = workspace.Create<C1>();
            c1x.C1C1Many2One.Value = c1y;

            var pushResult = await workspace.PushAsync();
            Assert.False(pushResult.HasErrors);

            pushResult = await workspace.PushAsync();
            Assert.False(pushResult.HasErrors);
        }
    }
}
