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

    public abstract class OneToOneTests : Test
    {
        protected OneToOneTests(Fixture fixture) : base(fixture)
        {
        }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            await this.Login("administrator");
        }

        [Fact]
        public async void SetRole()
        {
            {
                var workspace = this.Profile.Workspace;

                var c1A = workspace.Create<C1>();
                var c1B = workspace.Create<C1>();
                var c1C = workspace.Create<C1>();
                var c1D = workspace.Create<C1>();

                c1A.C1C1One2One = c1B;
                c1B.C1C1One2One = c1C;
                c1C.C1C1One2One = c1D;

                // Role
                Assert.Equal(c1B, c1A.C1C1One2One);
                Assert.Equal(c1C, c1B.C1C1One2One);
                Assert.Equal(c1D, c1C.C1C1One2One);
                Assert.Null(c1D.C1C1One2One);

                // Association
                Assert.Null(c1A.C1WhereC1C1One2One);
                Assert.Equal(c1A, c1B.C1WhereC1C1One2One);
                Assert.Equal(c1B, c1C.C1WhereC1C1One2One);
                Assert.Equal(c1C, c1D.C1WhereC1C1One2One);

                c1B.C1C1One2One = c1B;

                // Role
                Assert.Null(c1A.C1C1One2One);
                Assert.Equal(c1B, c1B.C1C1One2One);
                Assert.Equal(c1D, c1C.C1C1One2One);
                Assert.Null(c1D.C1C1One2One);

                // Association
                Assert.Null(c1A.C1WhereC1C1One2One);
                Assert.Equal(c1B, c1B.C1WhereC1C1One2One);
                Assert.Null(c1C.C1WhereC1C1One2One);
                Assert.Equal(c1C, c1D.C1WhereC1C1One2One);

                workspace.Reset();

                // Role
                Assert.Null(c1A.C1C1One2One);
                Assert.Null(c1B.C1C1One2One);
                Assert.Null(c1C.C1C1One2One);
                Assert.Null(c1D.C1C1One2One);

                // Association
                Assert.Null(c1A.C1WhereC1C1One2One);
                Assert.Null(c1B.C1WhereC1C1One2One);
                Assert.Null(c1C.C1WhereC1C1One2One);
                Assert.Null(c1D.C1WhereC1C1One2One);
            }

            {
                var workspace = this.Profile.Workspace;

                var result = await workspace.PullAsync(new Pull { Extent = new Filter(this.M.C1) });
                var c1s = result.GetCollection<C1>();
                var c1A = c1s.Single(v => v.Name.Value == "c1A");
                var c1B = c1s.Single(v => v.Name.Value == "c1B");
                var c1C = c1s.Single(v => v.Name.Value == "c1C");
                var c1D = c1s.Single(v => v.Name.Value == "c1D");

                // Role
                Assert.Equal(c1B, c1A.C1C1One2One);
                Assert.Equal(c1C, c1B.C1C1One2One);
                Assert.Equal(c1D, c1C.C1C1One2One);
                Assert.Null(c1D.C1C1One2One);

                // Association
                Assert.Null(c1A.C1WhereC1C1One2One);
                Assert.Equal(c1A, c1B.C1WhereC1C1One2One);
                Assert.Equal(c1B, c1C.C1WhereC1C1One2One);
                Assert.Equal(c1C, c1D.C1WhereC1C1One2One);

                c1B.C1C1One2One = c1B;

                // Role
                Assert.Null(c1A.C1C1One2One);
                Assert.Equal(c1B, c1B.C1C1One2One);
                Assert.Equal(c1D, c1C.C1C1One2One);
                Assert.Null(c1D.C1C1One2One);

                // Association
                Assert.Null(c1A.C1WhereC1C1One2One);
                Assert.Equal(c1B, c1B.C1WhereC1C1One2One);
                Assert.Null(c1C.C1WhereC1C1One2One);
                Assert.Equal(c1C, c1D.C1WhereC1C1One2One);

                workspace.Reset();

                // Role
                Assert.Equal(c1B, c1A.C1C1One2One);
                Assert.Equal(c1C, c1B.C1C1One2One);
                Assert.Equal(c1D, c1C.C1C1One2One);
                Assert.Null(c1D.C1C1One2One);

                // Association
                Assert.Null(c1A.C1WhereC1C1One2One);
                Assert.Equal(c1A, c1B.C1WhereC1C1One2One);
                Assert.Equal(c1B, c1C.C1WhereC1C1One2One);
                Assert.Equal(c1C, c1D.C1WhereC1C1One2One);
            }
        }

        //[Fact]
        //public async void RemoveRole()
        //{
        //    foreach (DatabaseMode mode1 in Enum.GetValues(typeof(DatabaseMode)))
        //    {
        //        foreach (DatabaseMode mode2 in Enum.GetValues(typeof(DatabaseMode)))
        //        {
        //            foreach (var contextFactory in this.contextFactories)
        //            {
        //                var ctx = contextFactory();
        //                var (workspace1, workspace2) = ctx;

        //                var c1x_1 = await ctx.Create<C1>(workspace1, mode1);
        //                var c1y_2 = await ctx.Create<C1>(workspace2, mode2);

        //                await workspace2.PushAsync();
        //                var result = await workspace1.PullAsync(new Pull { Object = c1y_2 });

        //                var c1y_1 = (C1)result.Objects.Values.First();

        //                c1y_1.ShouldNotBeNull(ctx, mode1, mode2);

        //                if (!c1x_1.CanWriteC1C1One2One)
        //                {
        //                    await workspace1.PullAsync(new Pull { Object = c1x_1 });
        //                }

        //                c1x_1.C1C1One2One = c1y_1;

        //                c1x_1.C1C1One2One.ShouldEqual(c1y_1, ctx, mode1, mode2);
        //                c1y_1.C1WhereC1C1One2One.ShouldEqual(c1x_1, ctx);

        //                c1x_1.RemoveC1C1One2One();

        //                c1x_1.C1C1One2One.ShouldNotEqual(c1y_1, ctx, mode1, mode2);
        //                c1y_1.C1WhereC1C1One2One.ShouldNotEqual(c1x_1, ctx);
        //            }
        //        }
        //    }
        //}
    }
}
