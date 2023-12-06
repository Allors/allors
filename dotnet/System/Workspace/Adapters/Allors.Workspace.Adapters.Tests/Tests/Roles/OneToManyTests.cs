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
    using System.Linq;

    public abstract class OneToManyTests : Test
    {
        protected OneToManyTests(Fixture fixture) : base(fixture)
        {

        }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            await this.Login("administrator");
        }

        [Fact]
        public async void AddRole()
        {
            /*   [Before]           [Add]           [After]
            *
            *  c1A      c1A     c1A      c1A     c1A      c1A
            *
            *                                             
            *  c1B ---- c1B     c1B      c1B     c1B ---- c1B
            *                         
            *                           
            *  c1C ---- c1C     c1C ---- c1C     c1C ---- c1C
            *       \                                 \   
            *        \                                 \
            *  c1D      c1D     c1D      c1D     c1D      c1D
            */
            {
                var workspace = this.Profile.Workspace;

                var c1A = workspace.Create<C1>();
                var c1B = workspace.Create<C1>();
                var c1C = workspace.Create<C1>();
                var c1D = workspace.Create<C1>();

                c1B.C1C1One2Manies.Add(c1B);
                c1C.C1C1One2Manies.Add(c1C);
                c1C.C1C1One2Manies.Add(c1D);

                // Role
                Assert.Empty(c1A.C1C1One2Manies.Value);
                c1B.C1C1One2Manies.Value.ShouldContainSingle(c1B);
                c1C.C1C1One2Manies.Value.ShouldHaveSameElements(new[] { c1C, c1D });
                Assert.Empty(c1D.C1C1One2Manies.Value);

                // Association
                Assert.Null(c1A.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1B, c1B.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1C, c1C.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1C, c1D.C1WhereC1C1One2Many.Value);

                c1C.C1C1One2Manies.Add(c1C);

                // Role
                Assert.Empty(c1A.C1C1One2Manies.Value);
                c1B.C1C1One2Manies.Value.ShouldContainSingle(c1B);
                c1C.C1C1One2Manies.Value.ShouldHaveSameElements(new[] { c1C, c1D });
                Assert.Empty(c1D.C1C1One2Manies.Value);

                // Association
                Assert.Null(c1A.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1B, c1B.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1C, c1C.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1C, c1D.C1WhereC1C1One2Many.Value);

                c1C.C1C1One2Manies.Add(c1C);

                // Role
                Assert.Empty(c1A.C1C1One2Manies.Value);
                c1B.C1C1One2Manies.Value.ShouldContainSingle(c1B);
                c1C.C1C1One2Manies.Value.ShouldHaveSameElements(new[] { c1C, c1D });
                Assert.Empty(c1D.C1C1One2Manies.Value);

                // Association
                Assert.Null(c1A.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1B, c1B.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1C, c1C.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1C, c1D.C1WhereC1C1One2Many.Value);

                workspace.Reset();

                // Role
                Assert.Empty(c1A.C1C1One2Manies.Value);
                Assert.Empty(c1B.C1C1One2Manies.Value);
                Assert.Empty(c1C.C1C1One2Manies.Value);
                Assert.Empty(c1D.C1C1One2Manies.Value);

                // Association
                Assert.Null(c1A.C1WhereC1C1One2Many.Value);
                Assert.Null(c1B.C1WhereC1C1One2Many.Value);
                Assert.Null(c1C.C1WhereC1C1One2Many.Value);
                Assert.Null(c1D.C1WhereC1C1One2Many.Value);
            }

            /*   [Before]           [Add]           [After]
            *
            *  c1A      c1A     c1A      c1A     c1A      c1A
            *
            *                                             
            *  c1B ---- c1B     c1B      c1B     c1B ---- c1B
            *                        \                \
            *                         \                \
            *  c1C ---- c1C     c1C      c1C     c1C      c1C
            *       \                                 \   
            *        \                                 \
            *  c1D      c1D     c1D      c1D     c1D      c1D
            */
            {
                var workspace = this.Profile.Workspace;

                var result = await workspace.PullAsync(new Pull { Extent = new Filter(this.M.C1) });
                var c1s = result.GetCollection<C1>();
                var c1A = c1s.Single(v => v.Name.Value == "c1A");
                var c1B = c1s.Single(v => v.Name.Value == "c1B");
                var c1C = c1s.Single(v => v.Name.Value == "c1C");
                var c1D = c1s.Single(v => v.Name.Value == "c1D");

                // Role
                Assert.Empty(c1A.C1C1One2Manies.Value);
                c1B.C1C1One2Manies.Value.ShouldContainSingle(c1B);
                c1C.C1C1One2Manies.Value.ShouldHaveSameElements(new[] { c1C, c1D });
                Assert.Empty(c1D.C1C1One2Manies.Value);

                // Association
                Assert.Null(c1A.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1B, c1B.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1C, c1C.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1C, c1D.C1WhereC1C1One2Many.Value);

                c1B.C1C1One2Manies.Add(c1C);

                // Role
                Assert.Empty(c1A.C1C1One2Manies.Value);
                c1B.C1C1One2Manies.Value.ShouldHaveSameElements(new[] { c1B, c1C });
                c1C.C1C1One2Manies.Value.ShouldContainSingle(c1D);
                Assert.Empty(c1D.C1C1One2Manies.Value);

                // Association
                Assert.Null(c1A.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1B, c1B.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1B, c1C.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1C, c1D.C1WhereC1C1One2Many.Value);

                c1B.C1C1One2Manies.Add(c1C);

                // Role
                Assert.Empty(c1A.C1C1One2Manies.Value);
                c1B.C1C1One2Manies.Value.ShouldHaveSameElements(new[] { c1B, c1C });
                c1C.C1C1One2Manies.Value.ShouldContainSingle(c1D);
                Assert.Empty(c1D.C1C1One2Manies.Value);

                // Association
                Assert.Null(c1A.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1B, c1B.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1B, c1C.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1C, c1D.C1WhereC1C1One2Many.Value);

                workspace.Reset();

                // Role
                Assert.Empty(c1A.C1C1One2Manies.Value);
                c1B.C1C1One2Manies.Value.ShouldContainSingle(c1B);
                c1C.C1C1One2Manies.Value.ShouldHaveSameElements(new[] { c1C, c1D });
                Assert.Empty(c1D.C1C1One2Manies.Value);

                // Association
                Assert.Null(c1A.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1B, c1B.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1C, c1C.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1C, c1D.C1WhereC1C1One2Many.Value);
            }
        }

        [Fact]
        public async void AddRoleWithNulls()
        {
            var workspace = this.Profile.Workspace;

            var c1A = workspace.Create<C1>();
            var c1B = workspace.Create<C1>();
            var c1C = workspace.Create<C1>();
            var c1D = workspace.Create<C1>();

            c1A.C1C1One2Manies.Add(null);

            Assert.Empty(c1A.C1C1One2Manies.Value);

            c1A.C1C1One2Manies.Value = new[]{c1B, null};

            c1A.C1C1One2Manies.Value.ShouldContainSingle(c1B);

            c1A.C1C1One2Manies.Value = new[] { null , c1C};

            c1A.C1C1One2Manies.Value.ShouldContainSingle(c1C);

            c1A.C1C1One2Manies.Value = new C1[] { null, null};

            Assert.Empty(c1A.C1C1One2Manies.Value);
        }

        [Fact]
        public async void RemoveRole()
        {
            /*   [Before]         [Remove]           [After]
            *
            *  c1A      c1A     c1A      c1A     c1A      c1A
            *
            *                                             
            *  c1B ---- c1B     c1B      c1B     c1B ---- c1B
            *                         x
            *                        /   
            *  c1C ---- c1C     c1C      c1C     c1C ---- c1C
            *       \                                 \   
            *        \                                 \
            *  c1D      c1D     c1D      c1D     c1D      c1D
            */
            {
                var workspace = this.Profile.Workspace;

                var c1A = workspace.Create<C1>();
                var c1B = workspace.Create<C1>();
                var c1C = workspace.Create<C1>();
                var c1D = workspace.Create<C1>();

                c1B.C1C1One2Manies.Add(c1B);
                c1C.C1C1One2Manies.Add(c1C);
                c1C.C1C1One2Manies.Add(c1D);

                // Role
                Assert.Empty(c1A.C1C1One2Manies.Value);
                c1B.C1C1One2Manies.Value.ShouldContainSingle(c1B);
                c1C.C1C1One2Manies.Value.ShouldHaveSameElements(new[] { c1C, c1D });
                Assert.Empty(c1D.C1C1One2Manies.Value);

                // Association
                Assert.Null(c1A.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1B, c1B.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1C, c1C.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1C, c1D.C1WhereC1C1One2Many.Value);

                c1C.C1C1One2Manies.Remove(c1B);

                // Role
                Assert.Empty(c1A.C1C1One2Manies.Value);
                c1B.C1C1One2Manies.Value.ShouldContainSingle(c1B);
                c1C.C1C1One2Manies.Value.ShouldHaveSameElements(new[] { c1C, c1D });
                Assert.Empty(c1D.C1C1One2Manies.Value);

                // Association
                Assert.Null(c1A.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1B, c1B.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1C, c1C.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1C, c1D.C1WhereC1C1One2Many.Value);

                c1C.C1C1One2Manies.Remove(c1B);

                // Role
                Assert.Empty(c1A.C1C1One2Manies.Value);
                c1B.C1C1One2Manies.Value.ShouldContainSingle(c1B);
                c1C.C1C1One2Manies.Value.ShouldHaveSameElements(new[] { c1C, c1D });
                Assert.Empty(c1D.C1C1One2Manies.Value);

                // Association
                Assert.Null(c1A.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1B, c1B.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1C, c1C.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1C, c1D.C1WhereC1C1One2Many.Value);

                workspace.Reset();

                // Role
                Assert.Empty(c1A.C1C1One2Manies.Value);
                Assert.Empty(c1B.C1C1One2Manies.Value);
                Assert.Empty(c1C.C1C1One2Manies.Value);
                Assert.Empty(c1D.C1C1One2Manies.Value);

                // Association
                Assert.Null(c1A.C1WhereC1C1One2Many.Value);
                Assert.Null(c1B.C1WhereC1C1One2Many.Value);
                Assert.Null(c1C.C1WhereC1C1One2Many.Value);
                Assert.Null(c1D.C1WhereC1C1One2Many.Value);
            }

            /*   [Before]          [Remove]           [After]
            *
            *  c1A      c1A     c1A      c1A     c1A      c1A
            *
            *                                             
            *  c1B ---- c1B     c1B      c1B     c1B ---- c1B
            *                        
            *                        
            *  c1C ---- c1C     c1C -x-- c1C     c1C      c1C
            *       \                                 \   
            *        \                                 \
            *  c1D      c1D     c1D      c1D     c1D      c1D
            */
            {
                var workspace = this.Profile.Workspace;

                var result = await workspace.PullAsync(new Pull { Extent = new Filter(this.M.C1) });
                var c1s = result.GetCollection<C1>();
                var c1A = c1s.Single(v => v.Name.Value == "c1A");
                var c1B = c1s.Single(v => v.Name.Value == "c1B");
                var c1C = c1s.Single(v => v.Name.Value == "c1C");
                var c1D = c1s.Single(v => v.Name.Value == "c1D");

                // Role
                Assert.Empty(c1A.C1C1One2Manies.Value);
                c1B.C1C1One2Manies.Value.ShouldContainSingle(c1B);
                c1C.C1C1One2Manies.Value.ShouldHaveSameElements(new[] { c1C, c1D });
                Assert.Empty(c1D.C1C1One2Manies.Value);

                // Association
                Assert.Null(c1A.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1B, c1B.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1C, c1C.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1C, c1D.C1WhereC1C1One2Many.Value);

                c1C.C1C1One2Manies.Remove(c1C);

                // Role
                Assert.Empty(c1A.C1C1One2Manies.Value);
                c1B.C1C1One2Manies.Value.ShouldContainSingle(c1B);
                c1C.C1C1One2Manies.Value.ShouldContainSingle(c1D);
                Assert.Empty(c1D.C1C1One2Manies.Value);

                // Association
                Assert.Null(c1A.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1B, c1B.C1WhereC1C1One2Many.Value);
                Assert.Null(c1C.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1C, c1D.C1WhereC1C1One2Many.Value);

                c1C.C1C1One2Manies.Remove(c1C);

                // Role
                Assert.Empty(c1A.C1C1One2Manies.Value);
                c1B.C1C1One2Manies.Value.ShouldContainSingle(c1B);
                c1C.C1C1One2Manies.Value.ShouldContainSingle(c1D);
                Assert.Empty(c1D.C1C1One2Manies.Value);

                // Association
                Assert.Null(c1A.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1B, c1B.C1WhereC1C1One2Many.Value);
                Assert.Null(c1C.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1C, c1D.C1WhereC1C1One2Many.Value);

                workspace.Reset();

                // Role
                Assert.Empty(c1A.C1C1One2Manies.Value);
                c1B.C1C1One2Manies.Value.ShouldContainSingle(c1B);
                c1C.C1C1One2Manies.Value.ShouldHaveSameElements(new[] { c1C, c1D });
                Assert.Empty(c1D.C1C1One2Manies.Value);

                // Association
                Assert.Null(c1A.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1B, c1B.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1C, c1C.C1WhereC1C1One2Many.Value);
                Assert.Equal(c1C, c1D.C1WhereC1C1One2Many.Value);
            }
        }


        //[Fact]
        //public async void SetRoleToNull()
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
        //                var result = await workspace1.PullAsync(new Pull { Object = c1y_2.Strategy });

        //                var c1y_1 = result.Objects.Values.First().Cast<C1>();

        //                c1y_1.ShouldNotBeNull(ctx, mode1, mode2);

        //                if (!c1x_1.C1C1One2Manies.CanWrite)
        //                {
        //                    await workspace1.PullAsync(new Pull { Object = c1x_1.Strategy });
        //                }

        //                c1x_1.C1C1One2Manies.Add(null);

        //                Assert.Empty(c1x_1.C1C1One2Manies.Value);
        //                c1y_1.C1WhereC1C1One2Many.Value.ShouldEqual(null, ctx, mode1, mode2);

        //                if (!c1x_1.C1C1One2Manies.CanWrite)
        //                {
        //                    await workspace1.PullAsync(new Pull { Object = c1x_1.Strategy });
        //                }

        //                c1x_1.C1C1One2Manies.Add(c1y_1);

        //                c1x_1.C1C1One2Manies.Value.ShouldContain(c1y_1, ctx, mode1, mode2);
        //                c1y_1.C1WhereC1C1One2Many.Value.ShouldEqual(c1x_1, ctx, mode1, mode2);
        //            }
        //        }
        //    }
        //}

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
        //                var result = await workspace1.PullAsync(new Pull { Object = c1y_2.Strategy });

        //                var c1y_1 = result.Objects.Values.First().Cast<C1>();

        //                c1y_1.ShouldNotBeNull(ctx, mode1, mode2);

        //                if (!c1x_1.C1C1One2Manies.CanWrite)
        //                {
        //                    await workspace1.PullAsync(new Pull { Object = c1x_1.Strategy });
        //                }

        //                c1x_1.C1C1One2Manies.Add(c1y_1);

        //                c1x_1.C1C1One2Manies.Value.ShouldContain(c1y_1, ctx, mode1, mode2);
        //                c1y_1.C1WhereC1C1One2Many.Value.ShouldEqual(c1x_1, ctx, mode1, mode2);

        //                c1x_1.C1C1One2Manies.Remove(c1y_1);

        //                c1x_1.C1C1One2Manies.Value.ShouldNotContain(c1y_1, ctx, mode1, mode2);
        //                c1y_1.C1WhereC1C1One2Many.Value.ShouldNotEqual(c1x_1, ctx, mode1, mode2);
        //            }
        //        }
        //    }
        //}

        //[Fact]
        //public async void RemoveNullRole()
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
        //                var result = await workspace1.PullAsync(new Pull { Object = c1y_2.Strategy });

        //                var c1y_1 = result.Objects.Values.First().Cast<C1>();

        //                c1y_1.ShouldNotBeNull(ctx, mode1, mode2);

        //                if (!c1x_1.C1C1One2Manies.CanWrite)
        //                {
        //                    await workspace1.PullAsync(new Pull { Object = c1x_1.Strategy });
        //                }

        //                c1x_1.C1C1One2Manies.Add(c1y_1);

        //                if (!c1x_1.C1C1One2Manies.CanWrite)
        //                {
        //                    await workspace1.PullAsync(new Pull { Object = c1x_1.Strategy });
        //                }

        //                c1x_1.C1C1One2Manies.Remove(null);

        //                c1x_1.C1C1One2Manies.Value.ShouldContain(c1y_1, ctx, mode1, mode2);
        //                c1y_1.C1WhereC1C1One2Many.Value.ShouldEqual(c1x_1, ctx, mode1, mode2);

        //                c1x_1.C1C1One2Manies.Value.ShouldContain(c1y_1, ctx, mode1, mode2);
        //                c1y_1.C1WhereC1C1One2Many.Value.ShouldEqual(c1x_1, ctx, mode1, mode2);

        //                if (!c1x_1.C1C1One2Manies.CanWrite)
        //                {
        //                    await workspace1.PullAsync(new Pull { Object = c1x_1.Strategy });
        //                }

        //                c1x_1.C1C1One2Manies.Remove(c1y_1);

        //                c1x_1.C1C1One2Manies.Value.ShouldNotContain(c1y_1, ctx, mode1, mode2);
        //                c1y_1.C1WhereC1C1One2Many.Value.ShouldNotEqual(c1x_1, ctx, mode1, mode2);
        //            }
        //        }
        //    }
        //}
    }
}
