// <copyright file="ServicesTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Allors.Workspace;
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

            var singleSessionContext = new SingleWorkspaceContext(this, "Single shared");
            var multipleSessionContext = new MultipleWorkspaceContext(this, "Multiple shared");

            this.contextFactories = new Func<Context>[]
            {
                () => singleSessionContext,
                //() => new SingleSessionContext(this, "Single"),
                //() => multipleSessionContext,
                () => new MultipleWorkspaceContext(this, "Multiple"),
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
                var (workspace1, workspace2) = ctx;

                var c1x_1 = await ctx.Create<C1>(workspace1, mode1);
                var c1y_2 = await ctx.Create<C1>(workspace2, mode2);

                c1x_1.ShouldNotBeNull(ctx, mode1, mode2);
                c1y_2.ShouldNotBeNull(ctx, mode1, mode2);

                var pushResult = await workspace2.PushAsync();
                Assert.False(pushResult.HasErrors);

                var result = await workspace1.PullAsync(new Pull { Object = c1y_2.Strategy });

                var c1y_1 = result.Objects.Values.First().Cast<C1>();

                c1y_1.ShouldNotBeNull(ctx, mode1, mode2);

                if (!c1x_1.C1C1Many2One.CanWrite)
                {
                    await workspace1.PullAsync(new Pull { Object = c1x_1.Strategy });
                }

                c1x_1.C1C1Many2One.Value = c1y_1;

                c1x_1.C1C1Many2One.Value.ShouldEqual(c1y_1, ctx, mode1, mode2);
                c1y_1.C1sWhereC1C1Many2One.Value.ShouldContain(c1x_1, ctx, mode1, mode2);

                pushResult = await workspace1.PushAsync();
                Assert.False(pushResult.HasErrors);

                pushResult = await workspace2.PushAsync();
                Assert.False(pushResult.HasErrors);
            }

            {
                var mode1 = DatabaseMode.NoPush;
                var mode2 = DatabaseMode.Push;

                var ctx = contextFactory();
                var (workspace1, workspace2) = ctx;

                var c1x_1 = await ctx.Create<C1>(workspace1, mode1);
                var c1y_2 = await ctx.Create<C1>(workspace2, mode2);

                c1x_1.ShouldNotBeNull(ctx, mode1, mode2);
                c1y_2.ShouldNotBeNull(ctx, mode1, mode2);

                var pushResult = await workspace2.PushAsync();
                Assert.False(pushResult.HasErrors);

                var result = await workspace1.PullAsync(new Pull { Object = c1y_2.Strategy });

                var c1y_1 = result.Objects.Values.First().Cast<C1>();

                c1y_1.ShouldNotBeNull(ctx, mode1, mode2);

                if (!c1x_1.C1C1Many2One.CanWrite)
                {
                    await workspace1.PullAsync(new Pull { Object = c1x_1.Strategy });
                }

                c1x_1.C1C1Many2One.Value = c1y_1;

                c1x_1.C1C1Many2One.Value.ShouldEqual(c1y_1, ctx, mode1, mode2);
                c1y_1.C1sWhereC1C1Many2One.Value.ShouldContain(c1x_1, ctx, mode1, mode2);

                pushResult = await workspace1.PushAsync();
                Assert.False(pushResult.HasErrors);

                pushResult = await workspace2.PushAsync();
                Assert.False(pushResult.HasErrors);
            }
        }
    }
}
