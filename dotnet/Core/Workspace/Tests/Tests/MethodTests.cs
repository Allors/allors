// <copyright file="MethodTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.Workspace
{
    using System.Linq;
    using Allors.Workspace;
    using Allors.Workspace.Data;
    using Allors.Workspace.Domain;
    using Xunit;

    public abstract class MethodTests : Test
    {
        protected MethodTests(Fixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async void CallSingle()
        {
            await this.Login("administrator");

            var session = this.Workspace.CreateSession();

            var pull = new[] { new Pull { Extent = new Filter(this.M.Organization) } };

            var organization = (await session.PullAsync(pull)).GetCollection<Organization>()[0];

            Assert.False(organization.JustDidIt);

            var invokeResult = await session.InvokeAsync(organization.JustDoIt);

            Assert.False(invokeResult.HasErrors);

            await session.PullAsync(new Pull { Object = organization });

            Assert.True(organization.JustDidIt);
            Assert.True(organization.JustDidItDerived);
        }

        [Fact]
        public async void CallMultiple()
        {
            await this.Login("administrator");

            var session = this.Workspace.CreateSession();

            var pull = new[] { new Pull { Extent = new Filter(this.M.Organization) } };

            var organization1 = (await session.PullAsync(pull)).GetCollection<Organization>()[0];
            var organization2 = (await session.PullAsync(pull)).GetCollection<Organization>().Skip(1).First();

            Assert.False(organization1.JustDidIt);

            var invokeResult = await session.InvokeAsync(new[] { organization1.JustDoIt, organization2.JustDoIt });

            Assert.False(invokeResult.HasErrors);

            await session.PullAsync(pull);

            Assert.True(organization1.JustDidIt);
            Assert.True(organization1.JustDidItDerived);

            Assert.True(organization2.JustDidIt);
            Assert.True(organization2.JustDidItDerived);
        }

        [Fact]
        public async void CallMultipleIsolated()
        {
            await this.Login("administrator");

            var session = this.Workspace.CreateSession();

            var pull = new[] { new Pull { Extent = new Filter(this.M.Organization) } };

            var organization1 = (await session.PullAsync(pull)).GetCollection<Organization>()[0];
            var organization2 = (await session.PullAsync(pull)).GetCollection<Organization>().Skip(1).First();

            Assert.False(organization1.JustDidIt);

            var invokeResult = await session.InvokeAsync(new[] { organization1.JustDoIt, organization2.JustDoIt }, new InvokeOptions { Isolated = true });

            Assert.False(invokeResult.HasErrors);

            await session.PullAsync(pull);

            Assert.True(organization1.JustDidIt);
            Assert.True(organization1.JustDidItDerived);

            Assert.True(organization2.JustDidIt);
            Assert.True(organization2.JustDidItDerived);
        }

    }
}
