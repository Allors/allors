// <copyright file="Many2OneTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Tests
{
    using System.Threading.Tasks;
    using Allors;
    using Allors.Workspace.Data;
    using Allors.Workspace.Domain;
    using Xunit;

    public abstract class SecurityTests : Test
    {
        protected SecurityTests(Fixture fixture) : base(fixture)
        {
        }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            await this.Login("administrator");
        }

        [Fact]
        public async void WithGrant()
        {
            await this.Login("administrator");

            var workspace = this.Workspace;

            var pull = new Pull
            {
                Extent = new Filter(this.M.C1)
            };

            var result = await workspace.PullAsync(pull);

            var c1s = result.GetCollection<C1>("C1s");
            foreach (var c1 in result.GetCollection<C1>())
            {
                foreach (var roleType in this.M.C1.RoleTypes)
                {
                    Assert.True(c1.Strategy.Role(roleType).CanRead);
                    Assert.True(c1.Strategy.Role(roleType).CanWrite);
                }
            }
        }

        [Fact]
        public async void WithoutAccessControl()
        {
            await this.Login("noacl");

            var workspace = this.Workspace;

            var pull = new Pull
            {
                Extent = new Filter(this.M.C1)
            };

            var result = await workspace.PullAsync(pull);

            foreach (var c1 in result.GetCollection<C1>())
            {
                foreach (var roleType in this.M.C1.RoleTypes)
                {
                    Assert.False(c1.Strategy.Role(roleType).CanRead);
                    Assert.False(c1.Strategy.Role(roleType).CanWrite);
                }
            }
        }

        [Fact]
        public async void WithoutPermissions()
        {
            await this.Login("noperm");

            var workspace = this.Workspace;

            var pull = new Pull
            {
                Extent = new Filter(this.M.C1)
            };

            var result = await workspace.PullAsync(pull);

            foreach (var c1 in result.GetCollection<C1>())
            {
                foreach (var roleType in this.M.C1.RoleTypes)
                {
                    Assert.False(c1.Strategy.Role(roleType).CanRead);
                    Assert.False(c1.Strategy.Role(roleType).CanWrite);
                }
            }
        }

        [Fact]
        public async void DeniedPermissions()
        {
            var workspace = this.Workspace;

            var result = await workspace.PullAsync(new Pull { Extent = new Filter(this.M.Denied) });

            foreach (var denied in result.GetCollection<Denied>())
            {
                foreach (var roleType in this.M.C1.RoleTypes)
                {
                    Assert.False(denied.Strategy.Role(roleType).CanRead);
                    Assert.False(denied.Strategy.Role(roleType).CanWrite);
                }
            }
        }
    }
}
