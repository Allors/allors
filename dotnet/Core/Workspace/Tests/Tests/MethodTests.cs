// <copyright file="MethodTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.Workspace
{
    using System.Linq;
    using Allors.Workspace;
    using Allors.Workspace.Data;
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

            var session = this.Connection;

            var pull = new[] { new Pull { Extent = new Filter(this.M.Organization) } };

            var organization = (await session.PullAsync(pull)).GetCollection(M.Organization)[0];

            Assert.False((bool)organization.GetUnitRole(M.Organization.JustDidIt));

            var invokeResult = await session.InvokeAsync(new Method(organization, M.Organization.JustDoIt));

            Assert.False(invokeResult.HasErrors);

            var result = await session.PullAsync(new Pull { Object = organization });
            organization = result.GetObject(M.Organization);

            Assert.True((bool)organization.GetUnitRole(M.Organization.JustDidIt));
            Assert.True((bool)organization.GetUnitRole(M.Organization.JustDidItDerived));
        }

        [Fact]
        public async void CallMultiple()
        {
            await this.Login("administrator");

            var session = this.Connection;

            var pull = new[] { new Pull { Extent = new Filter(this.M.Organization) } };

            var organization1 = (await session.PullAsync(pull)).GetCollection(M.Organization)[0];
            var organization2 = (await session.PullAsync(pull)).GetCollection(M.Organization).Skip(1).First();

            Assert.False((bool)organization1.GetUnitRole(M.Organization.JustDidIt));

            var invokeResult = await session.InvokeAsync(new[] { new Method(organization1, M.Organization.JustDoIt), new Method(organization2, M.Organization.JustDoIt) });

            Assert.False(invokeResult.HasErrors);

            var result = await session.PullAsync(pull);

            organization1 = result.GetCollection(M.Organization)[0];
            organization2 = result.GetCollection(M.Organization).Skip(1).First();

            Assert.True((bool)organization1.GetUnitRole(M.Organization.JustDidIt));
            Assert.True((bool)organization1.GetUnitRole(M.Organization.JustDidItDerived));

            Assert.True((bool)organization2.GetUnitRole(M.Organization.JustDidIt));
            Assert.True((bool)organization2.GetUnitRole(M.Organization.JustDidItDerived));
        }

        [Fact]
        public async void CallMultipleIsolated()
        {
            await this.Login("administrator");

            var session = this.Connection;

            var pull = new[] { new Pull { Extent = new Filter(this.M.Organization) } };

            var organization1 = (await session.PullAsync(pull)).GetCollection(M.Organization)[0];
            var organization2 = (await session.PullAsync(pull)).GetCollection(M.Organization).Skip(1).First();

            Assert.False((bool)organization1.GetUnitRole(M.Organization.JustDidIt));

            var invokeResult = await session.InvokeAsync(new[] { new Method(organization1, M.Organization.JustDoIt), new Method(organization2, M.Organization.JustDoIt) }, new InvokeOptions { Isolated = true });

            Assert.False(invokeResult.HasErrors);

            var result = await session.PullAsync(pull);

            organization1 = result.GetCollection(M.Organization)[0];
            organization2 = result.GetCollection(M.Organization).Skip(1).First();

            Assert.True((bool)organization1.GetUnitRole(M.Organization.JustDidIt));
            Assert.True((bool)organization1.GetUnitRole(M.Organization.JustDidItDerived));

            Assert.True((bool)organization2.GetUnitRole(M.Organization.JustDidIt));
            Assert.True((bool)organization2.GetUnitRole(M.Organization.JustDidItDerived));
        }

    }
}
