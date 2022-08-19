// <copyright file="ServicesTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.Workspace
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Allors.Workspace.Data;
    using Allors.Workspace.Domain;
    using Xunit;

    public abstract class SandboxTests : Test
    {
        private Func<Context>[] contextFactories;

        protected SandboxTests(Fixture fixture) : base(fixture)
        {
        }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            await this.Login("administrator");

            var singleSessionContext = new SingleSessionContext(this, "Single shared");
            var multipleSessionContext = new MultipleSessionContext(this, "Multiple shared");

            this.contextFactories = new Func<Context>[]
            {
                () => singleSessionContext,
                //() => new SingleSessionContext(this, "Single"),
                //() => multipleSessionContext,
                () => new MultipleSessionContext(this, "Multiple"),
            };
        }

        [Fact]
        public async void Test()
        {
            var contextFactory = this.contextFactories[0];

            {
                var mode1 = DatabaseMode.NoPush;
                var mode2 = DatabaseMode.NoPush;

                var ctx = contextFactory();
                var (session1, session2) = ctx;
            }
        }
    }
}
