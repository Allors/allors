// <copyright file="MethodTests.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Tests
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

            var workspace = this.Workspace;

            var pull = new[] { new Pull { Extent = new Filter(this.M.Organization) } };

            var organization = (await workspace.PullAsync(pull)).GetCollection<Organization>()[0];

            Assert.False(organization.JustDidIt.Value);

            var invokeResult = await workspace.InvokeAsync(organization.JustDoIt);

            Assert.False(invokeResult.HasErrors);

            await workspace.PullAsync(new Pull { Object = organization.Strategy });

            Assert.True(organization.JustDidIt.Value);
            Assert.True(organization.JustDidItDerived.Value);
        }

        [Fact]
        public async void CallMultiple()
        {
            await this.Login("administrator");

            var workspace = this.Workspace;

            var pull = new[] { new Pull { Extent = new Filter(this.M.Organization) } };

            var organisation1 = (await workspace.PullAsync(pull)).GetCollection<Organization>()[0];
            var organisation2 = (await workspace.PullAsync(pull)).GetCollection<Organization>().Skip(1).First();

            Assert.False(organisation1.JustDidIt.Value);

            var invokeResult = await workspace.InvokeAsync([organisation1.JustDoIt, organisation2.JustDoIt]);

            Assert.False(invokeResult.HasErrors);

            await workspace.PullAsync(pull);

            Assert.True(organisation1.JustDidIt.Value);
            Assert.True(organisation1.JustDidItDerived.Value);

            Assert.True(organisation2.JustDidIt.Value);
            Assert.True(organisation2.JustDidItDerived.Value);
        }

        [Fact]
        public async void CallMultipleIsolated()
        {
            await this.Login("administrator");

            var workspace = this.Workspace;

            var pull = new[] { new Pull { Extent = new Filter(this.M.Organization) } };

            var organisation1 = (await workspace.PullAsync(pull)).GetCollection<Organization>()[0];
            var organisation2 = (await workspace.PullAsync(pull)).GetCollection<Organization>().Skip(1).First();

            Assert.False(organisation1.JustDidIt.Value);

            var invokeResult = await workspace.InvokeAsync([organisation1.JustDoIt, organisation2.JustDoIt], new InvokeOptions { Isolated = true });

            Assert.False(invokeResult.HasErrors);

            await workspace.PullAsync(pull);

            Assert.True(organisation1.JustDidIt.Value);
            Assert.True(organisation1.JustDidItDerived.Value);

            Assert.True(organisation2.JustDidIt.Value);
            Assert.True(organisation2.JustDidItDerived.Value);
        }

    }
}
