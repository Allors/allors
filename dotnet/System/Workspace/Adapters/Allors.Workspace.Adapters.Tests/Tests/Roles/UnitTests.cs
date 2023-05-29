// <copyright file="Many2OneTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Tests
{
    using System;
    using System.Threading.Tasks;
    using Allors.Workspace.Domain;
    using Xunit;
    using Allors.Workspace.Data;
    using Allors.Workspace;

    public abstract class UnitTests : Test
    {
        private Func<Context>[] contextFactories;

        protected UnitTests(Fixture fixture) : base(fixture)
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
            foreach (DatabaseMode mode in Enum.GetValues(typeof(DatabaseMode)))
            {
                foreach (var contextFactory in this.contextFactories)
                {
                    var ctx = contextFactory();
                    var (workspace1, _) = ctx;

                    var c1 = await ctx.Create<C1>(workspace1, mode);

                    Assert.NotNull(c1);

                    if (!c1.CanWriteC1C1One2One)
                    {
                        await workspace1.PullAsync(new Pull { Object = c1.Strategy });
                    }

                    c1.C1AllorsBinary.Value = new byte[] { 1, 2 };
                    c1.C1AllorsBoolean.Value = true;
                    c1.C1AllorsDateTime.Value = new DateTime(1973, 3, 27, 12, 1, 2, 3, DateTimeKind.Utc);
                    c1.C1AllorsDecimal.Value = 10.10m;
                    c1.C1AllorsDouble.Value = 11.11d;
                    c1.C1AllorsInteger.Value = 12;
                    c1.C1AllorsString.Value = "a string";
                    c1.C1AllorsUnique.Value = new Guid("0208BB9B-E87B-4CED-8DEC-516E6778CD66");

                    Assert.Equal(new byte[] { 1, 2 }, c1.C1AllorsBinary.Value);
                    Assert.True(c1.C1AllorsBoolean.Value);
                    Assert.Equal(new DateTime(1973, 3, 27, 12, 1, 2, 3, DateTimeKind.Utc), c1.C1AllorsDateTime.Value);
                    Assert.Equal(10.10m, c1.C1AllorsDecimal.Value);
                    Assert.Equal(11.11d, c1.C1AllorsDouble.Value);
                    Assert.Equal(12, c1.C1AllorsInteger.Value);
                    Assert.Equal("a string", c1.C1AllorsString.Value);
                    Assert.Equal(new Guid("0208BB9B-E87B-4CED-8DEC-516E6778CD66"), c1.C1AllorsUnique.Value);

                    if (c1.Strategy.Id > 0)
                    {
                        await workspace1.PullAsync(new Pull { Object = c1.Strategy });
                    }

                    Assert.Equal(new byte[] { 1, 2 }, c1.C1AllorsBinary.Value);
                    Assert.True(c1.C1AllorsBoolean.Value);
                    Assert.Equal(new DateTime(1973, 3, 27, 12, 1, 2, 3, DateTimeKind.Utc), c1.C1AllorsDateTime.Value);
                    Assert.Equal(10.10m, c1.C1AllorsDecimal.Value);
                    Assert.Equal(11.11d, c1.C1AllorsDouble.Value);
                    Assert.Equal(12, c1.C1AllorsInteger.Value);
                    Assert.Equal("a string", c1.C1AllorsString.Value);
                    Assert.Equal(new Guid("0208BB9B-E87B-4CED-8DEC-516E6778CD66"), c1.C1AllorsUnique.Value);
                }
            }
        }

        [Fact]
        public async void SetRoleNull()
        {
            foreach (DatabaseMode mode in Enum.GetValues(typeof(DatabaseMode)))
            {
                foreach (var contextFactory in this.contextFactories)
                {
                    var ctx = contextFactory();
                    var (workspace1, _) = ctx;

                    var c1 = await ctx.Create<C1>(workspace1, mode);

                    Assert.NotNull(c1);

                    if (!c1.CanWriteC1C1One2One)
                    {
                        await workspace1.PullAsync(new Pull { Object = c1.Strategy });
                    }

                    c1.C1AllorsBinary.Value = null;
                    c1.C1AllorsBoolean.Value = null;
                    c1.C1AllorsDateTime.Value = null;
                    c1.C1AllorsDecimal.Value = null;
                    c1.C1AllorsDouble.Value = null;
                    c1.C1AllorsInteger.Value = null;
                    c1.C1AllorsString.Value = null;
                    c1.C1AllorsUnique.Value = null;

                    Assert.False(c1.C1AllorsBinary.Exist);
                    Assert.False(c1.C1AllorsBoolean.Exist);
                    Assert.False(c1.C1AllorsDateTime.Exist);
                    Assert.False(c1.C1AllorsDecimal.Exist);
                    Assert.False(c1.C1AllorsDouble.Exist);
                    Assert.False(c1.C1AllorsInteger.Exist);
                    Assert.False(c1.C1AllorsString.Exist);
                    Assert.False(c1.C1AllorsUnique.Exist);

                    Assert.Null(c1.C1AllorsBinary.Value);
                    Assert.Null(c1.C1AllorsBoolean.Value);
                    Assert.Null(c1.C1AllorsDateTime.Value);
                    Assert.Null(c1.C1AllorsDecimal.Value);
                    Assert.Null(c1.C1AllorsDouble.Value);
                    Assert.Null(c1.C1AllorsInteger.Value);
                    Assert.Null(c1.C1AllorsString.Value);
                    Assert.Null(c1.C1AllorsUnique.Value);

                    if (c1.Strategy.Id > 0)
                    {
                        await workspace1.PullAsync(new Pull { Object = c1.Strategy });
                    }

                    Assert.False(c1.C1AllorsBinary.Exist);
                    Assert.False(c1.C1AllorsBoolean.Exist);
                    Assert.False(c1.C1AllorsDateTime.Exist);
                    Assert.False(c1.C1AllorsDecimal.Exist);
                    Assert.False(c1.C1AllorsDouble.Exist);
                    Assert.False(c1.C1AllorsInteger.Exist);
                    Assert.False(c1.C1AllorsString.Exist);
                    Assert.False(c1.C1AllorsUnique.Exist);

                    Assert.Null(c1.C1AllorsBinary.Value);
                    Assert.Null(c1.C1AllorsBoolean.Value);
                    Assert.Null(c1.C1AllorsDateTime.Value);
                    Assert.Null(c1.C1AllorsDecimal.Value);
                    Assert.Null(c1.C1AllorsDouble.Value);
                    Assert.Null(c1.C1AllorsInteger.Value);
                    Assert.Null(c1.C1AllorsString.Value);
                    Assert.Null(c1.C1AllorsUnique.Value);

                    if (!c1.CanWriteC1C1One2One)
                    {
                        await workspace1.PullAsync(new Pull { Object = c1.Strategy });
                    }

                    c1.C1AllorsBinary.Value = new byte[] { 1, 2 };
                    c1.C1AllorsBoolean.Value = true;
                    c1.C1AllorsDateTime.Value = new DateTime(1973, 3, 27, 12, 1, 2, 3, DateTimeKind.Utc);
                    c1.C1AllorsDecimal.Value = 10.10m;
                    c1.C1AllorsDouble.Value = 11.11d;
                    c1.C1AllorsInteger.Value = 12;
                    c1.C1AllorsString.Value = "a string";
                    c1.C1AllorsUnique.Value = new Guid("0208BB9B-E87B-4CED-8DEC-516E6778CD66");


                    if (!c1.CanWriteC1C1One2One)
                    {
                        await workspace1.PullAsync(new Pull { Object = c1.Strategy });
                    }

                    c1.C1AllorsBinary.Value = null;
                    c1.C1AllorsBoolean.Value = null;
                    c1.C1AllorsDateTime.Value = null;
                    c1.C1AllorsDecimal.Value = null;
                    c1.C1AllorsDouble.Value = null;
                    c1.C1AllorsInteger.Value = null;
                    c1.C1AllorsString.Value = null;
                    c1.C1AllorsUnique.Value = null;

                    Assert.False(c1.C1AllorsBinary.Exist);
                    Assert.False(c1.C1AllorsBoolean.Exist);
                    Assert.False(c1.C1AllorsDateTime.Exist);
                    Assert.False(c1.C1AllorsDecimal.Exist);
                    Assert.False(c1.C1AllorsDouble.Exist);
                    Assert.False(c1.C1AllorsInteger.Exist);
                    Assert.False(c1.C1AllorsString.Exist);
                    Assert.False(c1.C1AllorsUnique.Exist);

                    Assert.Null(c1.C1AllorsBinary.Value);
                    Assert.Null(c1.C1AllorsBoolean.Value);
                    Assert.Null(c1.C1AllorsDateTime.Value);
                    Assert.Null(c1.C1AllorsDecimal.Value);
                    Assert.Null(c1.C1AllorsDouble.Value);
                    Assert.Null(c1.C1AllorsInteger.Value);
                    Assert.Null(c1.C1AllorsString.Value);
                    Assert.Null(c1.C1AllorsUnique.Value);

                    if (c1.Strategy.Id > 0)
                    {
                        await workspace1.PullAsync(new Pull { Object = c1.Strategy });
                    }

                    Assert.False(c1.C1AllorsBinary.Exist);
                    Assert.False(c1.C1AllorsBoolean.Exist);
                    Assert.False(c1.C1AllorsDateTime.Exist);
                    Assert.False(c1.C1AllorsDecimal.Exist);
                    Assert.False(c1.C1AllorsDouble.Exist);
                    Assert.False(c1.C1AllorsInteger.Exist);
                    Assert.False(c1.C1AllorsString.Exist);
                    Assert.False(c1.C1AllorsUnique.Exist);

                    Assert.Null(c1.C1AllorsBinary.Value);
                    Assert.Null(c1.C1AllorsBoolean.Value);
                    Assert.Null(c1.C1AllorsDateTime.Value);
                    Assert.Null(c1.C1AllorsDecimal.Value);
                    Assert.Null(c1.C1AllorsDouble.Value);
                    Assert.Null(c1.C1AllorsInteger.Value);
                    Assert.Null(c1.C1AllorsString.Value);
                    Assert.Null(c1.C1AllorsUnique.Value);
                }
            }
        }

        [Fact]
        public async void RemoveRole()
        {
            foreach (DatabaseMode mode in Enum.GetValues(typeof(DatabaseMode)))
            {
                foreach (var contextFactory in this.contextFactories)
                {
                    var ctx = contextFactory();
                    var (workspace1, _) = ctx;

                    var c1 = await ctx.Create<C1>(workspace1, mode);

                    Assert.NotNull(c1);

                    if (!c1.CanWriteC1C1One2One)
                    {
                        await workspace1.PullAsync(new Pull { Object = c1.Strategy });
                    }

                    c1.C1AllorsBinary.Value = null;
                    c1.C1AllorsBoolean.Value = null;
                    c1.C1AllorsDateTime.Value = null;
                    c1.C1AllorsDecimal.Value = null;
                    c1.C1AllorsDouble.Value = null;
                    c1.C1AllorsInteger.Value = null;
                    c1.C1AllorsString.Value = null;
                    c1.C1AllorsUnique.Value = null;

                    Assert.False(c1.C1AllorsBinary.Exist);
                    Assert.False(c1.C1AllorsBoolean.Exist);
                    Assert.False(c1.C1AllorsDateTime.Exist);
                    Assert.False(c1.C1AllorsDecimal.Exist);
                    Assert.False(c1.C1AllorsDouble.Exist);
                    Assert.False(c1.C1AllorsInteger.Exist);
                    Assert.False(c1.C1AllorsString.Exist);
                    Assert.False(c1.C1AllorsUnique.Exist);

                    Assert.Null(c1.C1AllorsBinary.Value);
                    Assert.Null(c1.C1AllorsBoolean.Value);
                    Assert.Null(c1.C1AllorsDateTime.Value);
                    Assert.Null(c1.C1AllorsDecimal.Value);
                    Assert.Null(c1.C1AllorsDouble.Value);
                    Assert.Null(c1.C1AllorsInteger.Value);
                    Assert.Null(c1.C1AllorsString.Value);
                    Assert.Null(c1.C1AllorsUnique.Value);

                    if (c1.Strategy.Id > 0)
                    {
                        await workspace1.PullAsync(new Pull { Object = c1.Strategy });
                    }

                    Assert.False(c1.C1AllorsBinary.Exist);
                    Assert.False(c1.C1AllorsBoolean.Exist);
                    Assert.False(c1.C1AllorsDateTime.Exist);
                    Assert.False(c1.C1AllorsDecimal.Exist);
                    Assert.False(c1.C1AllorsDouble.Exist);
                    Assert.False(c1.C1AllorsInteger.Exist);
                    Assert.False(c1.C1AllorsString.Exist);
                    Assert.False(c1.C1AllorsUnique.Exist);

                    Assert.Null(c1.C1AllorsBinary.Value);
                    Assert.Null(c1.C1AllorsBoolean.Value);
                    Assert.Null(c1.C1AllorsDateTime.Value);
                    Assert.Null(c1.C1AllorsDecimal.Value);
                    Assert.Null(c1.C1AllorsDouble.Value);
                    Assert.Null(c1.C1AllorsInteger.Value);
                    Assert.Null(c1.C1AllorsString.Value);
                    Assert.Null(c1.C1AllorsUnique.Value);

                    if (!c1.CanWriteC1C1One2One)
                    {
                        await workspace1.PullAsync(new Pull { Object = c1.Strategy });
                    }

                    c1.C1AllorsBinary.Value = new byte[] { 1, 2 };
                    c1.C1AllorsBoolean.Value = true;
                    c1.C1AllorsDateTime.Value = new DateTime(1973, 3, 27, 12, 1, 2, 3, DateTimeKind.Utc);
                    c1.C1AllorsDecimal.Value = 10.10m;
                    c1.C1AllorsDouble.Value = 11.11d;
                    c1.C1AllorsInteger.Value = 12;
                    c1.C1AllorsString.Value = "a string";
                    c1.C1AllorsUnique.Value = new Guid("0208BB9B-E87B-4CED-8DEC-516E6778CD66");

                    if (!c1.CanWriteC1C1One2One)
                    {
                        await workspace1.PullAsync(new Pull { Object = c1.Strategy });
                    }

                    c1.C1AllorsBinary.Value = null;
                    c1.C1AllorsBoolean.Value = null;
                    c1.C1AllorsDateTime.Value = null;
                    c1.C1AllorsDecimal.Value = null;
                    c1.C1AllorsDouble.Value = null;
                    c1.C1AllorsInteger.Value = null;
                    c1.C1AllorsString.Value = null;
                    c1.C1AllorsUnique.Value = null;

                    Assert.False(c1.C1AllorsBinary.Exist);
                    Assert.False(c1.C1AllorsBoolean.Exist);
                    Assert.False(c1.C1AllorsDateTime.Exist);
                    Assert.False(c1.C1AllorsDecimal.Exist);
                    Assert.False(c1.C1AllorsDouble.Exist);
                    Assert.False(c1.C1AllorsInteger.Exist);
                    Assert.False(c1.C1AllorsString.Exist);
                    Assert.False(c1.C1AllorsUnique.Exist);

                    Assert.Null(c1.C1AllorsBinary.Value);
                    Assert.Null(c1.C1AllorsBoolean.Value);
                    Assert.Null(c1.C1AllorsDateTime.Value);
                    Assert.Null(c1.C1AllorsDecimal.Value);
                    Assert.Null(c1.C1AllorsDouble.Value);
                    Assert.Null(c1.C1AllorsInteger.Value);
                    Assert.Null(c1.C1AllorsString.Value);
                    Assert.Null(c1.C1AllorsUnique.Value);

                    if (c1.Strategy.Id > 0)
                    {
                        await workspace1.PullAsync(new Pull { Object = c1.Strategy });
                    }

                    Assert.False(c1.C1AllorsBinary.Exist);
                    Assert.False(c1.C1AllorsBoolean.Exist);
                    Assert.False(c1.C1AllorsDateTime.Exist);
                    Assert.False(c1.C1AllorsDecimal.Exist);
                    Assert.False(c1.C1AllorsDouble.Exist);
                    Assert.False(c1.C1AllorsInteger.Exist);
                    Assert.False(c1.C1AllorsString.Exist);
                    Assert.False(c1.C1AllorsUnique.Exist);

                    Assert.Null(c1.C1AllorsBinary.Value);
                    Assert.Null(c1.C1AllorsBoolean.Value);
                    Assert.Null(c1.C1AllorsDateTime.Value);
                    Assert.Null(c1.C1AllorsDecimal.Value);
                    Assert.Null(c1.C1AllorsDouble.Value);
                    Assert.Null(c1.C1AllorsInteger.Value);
                    Assert.Null(c1.C1AllorsString.Value);
                    Assert.Null(c1.C1AllorsUnique.Value);
                }
            }
        }
    }
}
