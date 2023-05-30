// <copyright file="Many2OneTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Tests
{
    using System.Threading.Tasks;
    using Allors.Workspace.Domain;
    using Allors.Workspace;
    using Xunit;
    using Allors.Workspace.Data;
    using System;
    using System.Linq;

    public abstract class ManyToOneTests : Test
    {
        private Func<Context>[] contextFactories;

        protected ManyToOneTests(Fixture fixture) : base(fixture)
        {

        }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            await this.Login("administrator");
            
            var singleWorkspaceContext = new SingleWorkspaceContext(this, "Single Shared Workspace");
            var multipleWorkspaceContext = new MultipleWorkspaceContext(this, "Multiple Shared Workspace");

            this.contextFactories = new Func<Context>[]
            {
                () => singleWorkspaceContext,
                () => new SingleWorkspaceContext(this, "Single Workspace"),
                () => multipleWorkspaceContext,
                () => new MultipleWorkspaceContext(this, "Multiple Workspace"),
            };
        }

        [Fact]
        public async void SetRole()
        {
            foreach (DatabaseMode mode1 in Enum.GetValues(typeof(DatabaseMode)))
            {
                foreach (DatabaseMode mode2 in Enum.GetValues(typeof(DatabaseMode)))
                {
                    foreach (var contextFactory in this.contextFactories)
                    {
                        var ctx = contextFactory();
                        var (workspace1, workspace2) = ctx;

                        var c1x_1 = await ctx.Create<C1>(workspace1, mode1);
                        var c1y_2 = await ctx.Create<C1>(workspace2, mode2);

                        await workspace2.PushAsync();
                        var result = await workspace1.PullAsync(new Pull { Object = c1y_2.Strategy });

                        var c1y_1 = result.Objects.Values.First().Cast<C1>();

                        c1y_1.ShouldNotBeNull(ctx, mode1, mode2);

                        if (!c1x_1.C1C1Many2One.CanWrite)
                        {
                            await workspace1.PullAsync(new Pull { Object = c1x_1.Strategy });
                        }

                        c1x_1.C1C1Many2One.Value = c1y_1;

                        c1x_1.C1C1Many2One.Value.ShouldEqual(c1y_1, ctx, mode1, mode2);
                        c1y_1.C1sWhereC1C1Many2One.ShouldContain(c1x_1, ctx, mode1, mode2);

                        await workspace1.PushAsync();
                        await workspace2.PushAsync();
                    }
                }
            }
        }

        [Fact]
        public async void RemoveRole()
        {
            foreach (DatabaseMode mode1 in Enum.GetValues(typeof(DatabaseMode)))
            {
                foreach (DatabaseMode mode2 in Enum.GetValues(typeof(DatabaseMode)))
                {
                    foreach (var contextFactory in this.contextFactories)
                    {
                        var ctx = contextFactory();
                        var (workspace1, workspace2) = ctx;

                        var c1x_1 = await ctx.Create<C1>(workspace1, mode1);
                        var c1y_2 = await ctx.Create<C1>(workspace2, mode2);

                        await workspace2.PushAsync();
                        var result = await workspace1.PullAsync(new Pull { Object = c1y_2.Strategy });

                        var c1y_1 = result.Objects.Values.First().Cast<C1>();

                        c1y_1.ShouldNotBeNull(ctx, mode1, mode2);

                        if (!c1x_1.C1C1Many2One.CanWrite)
                        {
                            await workspace1.PullAsync(new Pull { Object = c1x_1.Strategy });
                        }

                        c1x_1.C1C1Many2One.Value = c1y_1;
                        c1x_1.C1C1Many2One.Value.ShouldEqual(c1y_1, ctx, mode1, mode2);
                        c1y_1.C1sWhereC1C1Many2One.ShouldContain(c1x_1, ctx, mode1, mode2);

                        c1x_1.C1C1Many2One.Value = null;
                        c1x_1.C1C1Many2One.Value.ShouldNotEqual(c1y_1, ctx, mode1, mode2);
                        c1y_1.C1sWhereC1C1Many2One.ShouldNotContain(c1x_1, ctx, mode1, mode2);
                    }
                }
            }
        }
    }
}
