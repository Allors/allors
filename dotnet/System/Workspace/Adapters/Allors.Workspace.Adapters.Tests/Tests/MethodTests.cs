﻿// <copyright file="MethodTests.cs" company="Allors bvba">
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

            var organisation = (await workspace.PullAsync(pull)).GetCollection<Organization>()[0];

            Assert.False(organisation.JustDidIt.Value);

            var invokeResult = await workspace.InvokeAsync(organisation.JustDoIt);

            Assert.False(invokeResult.HasErrors);

            await workspace.PullAsync(new Pull { Object = organisation.Strategy });

            Assert.True(organisation.JustDidIt.Value);
            Assert.True(organisation.JustDidItDerived.Value);
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

            var invokeResult = await workspace.InvokeAsync(new[] { organisation1.JustDoIt, organisation2.JustDoIt });

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

            var invokeResult = await workspace.InvokeAsync(new[] { organisation1.JustDoIt, organisation2.JustDoIt }, new InvokeOptions { Isolated = true });

            Assert.False(invokeResult.HasErrors);

            await workspace.PullAsync(pull);

            Assert.True(organisation1.JustDidIt.Value);
            Assert.True(organisation1.JustDidItDerived.Value);

            Assert.True(organisation2.JustDidIt.Value);
            Assert.True(organisation2.JustDidItDerived.Value);
        }

    }
}
