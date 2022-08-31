// <copyright file="MethodTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.Workspace
{
    using System.Linq;
    using Allors.Workspace;
    using Allors.Workspace.Request;
    using Allors.Workspace.Response;
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

            var connection = this.Connection;
            var m = this.M;

            var pull = new[] { new Pull { Extent = new Filter(m.Organization) } };

            var organization = (await connection.PullAsync(pull)).GetCollection(m.Organization)[0];

            Assert.False((bool)organization.GetUnitRole(m.Organization.JustDidIt));

            var invokeResult = await connection.InvokeAsync(new Method(organization, m.Organization.JustDoIt));

            Assert.False(invokeResult.HasErrors);

            var result = await connection.PullAsync(new Pull { Object = organization });
            organization = result.GetObject(m.Organization);

            Assert.True((bool)organization.GetUnitRole(m.Organization.JustDidIt));
            Assert.True((bool)organization.GetUnitRole(m.Organization.JustDidItDerived));
        }

        [Fact]
        public async void CallMultiple()
        {
            await this.Login("administrator");

            var connection = this.Connection;
            var m = this.M;

            var pull = new[] { new Pull { Extent = new Filter(m.Organization) } };

            var organization1 = (await connection.PullAsync(pull)).GetCollection(m.Organization)[0];
            var organization2 = (await connection.PullAsync(pull)).GetCollection(m.Organization).Skip(1).First();

            Assert.False((bool)organization1.GetUnitRole(m.Organization.JustDidIt));

            var invokeResult = await connection.InvokeAsync(new[] { new Method(organization1, m.Organization.JustDoIt), new Method(organization2, m.Organization.JustDoIt) });

            Assert.False(invokeResult.HasErrors);

            var result = await connection.PullAsync(pull);

            organization1 = result.GetCollection(m.Organization)[0];
            organization2 = result.GetCollection(m.Organization).Skip(1).First();

            Assert.True((bool)organization1.GetUnitRole(m.Organization.JustDidIt));
            Assert.True((bool)organization1.GetUnitRole(m.Organization.JustDidItDerived));

            Assert.True((bool)organization2.GetUnitRole(m.Organization.JustDidIt));
            Assert.True((bool)organization2.GetUnitRole(m.Organization.JustDidItDerived));
        }

        [Fact]
        public async void CallMultipleIsolated()
        {
            await this.Login("administrator");

            var connection = this.Connection;
            var m = this.M;

            var pull = new[] { new Pull { Extent = new Filter(m.Organization) } };

            var organization1 = (await connection.PullAsync(pull)).GetCollection(m.Organization)[0];
            var organization2 = (await connection.PullAsync(pull)).GetCollection(m.Organization).Skip(1).First();

            Assert.False((bool)organization1.GetUnitRole(m.Organization.JustDidIt));

            var invokeResult = await connection.InvokeAsync(new[] { new Method(organization1, m.Organization.JustDoIt), new Method(organization2, m.Organization.JustDoIt) }, new InvokeOptions { Isolated = true });

            Assert.False(invokeResult.HasErrors);

            var result = await connection.PullAsync(pull);

            organization1 = result.GetCollection(m.Organization)[0];
            organization2 = result.GetCollection(m.Organization).Skip(1).First();

            Assert.True((bool)organization1.GetUnitRole(m.Organization.JustDidIt));
            Assert.True((bool)organization1.GetUnitRole(m.Organization.JustDidItDerived));

            Assert.True((bool)organization2.GetUnitRole(m.Organization.JustDidIt));
            Assert.True((bool)organization2.GetUnitRole(m.Organization.JustDidItDerived));
        }
    }
}
