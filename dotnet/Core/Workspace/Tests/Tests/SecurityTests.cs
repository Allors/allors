// <copyright file="Many2OneTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.Workspace
{
    using System.Threading.Tasks;
    using Allors.Workspace.Request;
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
            var m = this.M;

            foreach (var connection in this.Connections)
            {
                var pull = new PullRequest
                {
                    Extent = new Filter(this.M.C1),
                };

                var result = await connection.PullAsync(pull);

                foreach (var c1 in result.GetCollection(m.C1))
                {
                    foreach (var roleType in this.M.C1.RoleTypes)
                    {
                        Assert.True(c1.CanRead(roleType));
                        Assert.True(c1.CanWrite(roleType));
                    }
                }
            }
        }

        [Fact]
        public async void WithoutAccessControl()
        {
            await this.Login("noacl");
            var m = this.M;

            foreach (var connection in this.Connections)
            {
                var pull = new PullRequest
                {
                    Extent = new Filter(this.M.C1),
                };

                var result = await connection.PullAsync(pull);

                foreach (var c1 in result.GetCollection(m.C1))
                {
                    foreach (var roleType in this.M.C1.RoleTypes)
                    {
                        Assert.False(c1.CanRead(roleType));
                        Assert.False(c1.CanWrite(roleType));
                    }
                }
            }
        }

        [Fact]
        public async void WithoutPermissions()
        {
            await this.Login("noperm");
            var m = this.M;

            foreach (var connection in this.Connections)
            {
                var pull = new PullRequest
                {
                    Extent = new Filter(this.M.C1),
                };

                var result = await connection.PullAsync(pull);

                foreach (var c1 in result.GetCollection(m.C1))
                {
                    foreach (var roleType in this.M.C1.RoleTypes)
                    {
                        Assert.False(c1.CanRead(roleType));
                        Assert.False(c1.CanWrite(roleType));
                    }
                }
            }
        }

        [Fact]
        public async void DeniedPermissions()
        {
            var m = this.M;

            foreach (var connection in this.Connections)
            {
                var result = await connection.PullAsync(new PullRequest { Extent = new Filter(this.M.Denied) });

                foreach (var denied in result.GetCollection(m.Denied))
                {
                    foreach (var roleType in this.M.C1.RoleTypes)
                    {
                        Assert.False(denied.CanRead(roleType));
                        Assert.False(denied.CanWrite(roleType));
                    }
                }
            }
        }
    }
}
