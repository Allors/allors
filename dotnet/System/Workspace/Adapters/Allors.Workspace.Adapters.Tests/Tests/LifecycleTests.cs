// <copyright file="Many2OneTests.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Tests
{
    using System.Threading.Tasks;
    using Allors.Workspace.Domain;
    using Xunit;
    using Allors.Workspace.Data;
    using System;

    public abstract class LifecycleTests : Test
    {
        protected LifecycleTests(Fixture fixture) : base(fixture)
        {

        }

        public override async System.Threading.Tasks.Task InitializeAsync()
        {
            await base.InitializeAsync();
            await this.Login("administrator");
        }

        [Fact]
        public async void PullSameSessionNotPushedException()
        {
            var workspace = this.Workspace;

            var c1 = workspace.Create<C1>();
            Assert.NotNull(c1);

            bool hasErrors;

            try
            {
                var result = await workspace.PullAsync(new Pull { Object = c1.Strategy });
                hasErrors = false;
            }
            catch (Exception)
            {
                hasErrors = true;
            }

            Assert.True(hasErrors);
        }

        [Fact]
        public async void PullNotPushedException()
        {
            var workspace = this.Workspace;

            var c1 = workspace.Create<C1>();
            Assert.NotNull(c1);

            bool hasErrors;

            try
            {
                var result = await workspace.PullAsync(new Pull { Object = c1.Strategy });
                hasErrors = false;
            }
            catch (Exception)
            {
                hasErrors = true;
            }

            Assert.True(hasErrors);
        }
    }
}
