// <copyright file="Many2OneTests.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
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
        protected UnitTests(Fixture fixture) : base(fixture)
        {
        }

        public override async System.Threading.Tasks.Task InitializeAsync()
        {
            await base.InitializeAsync();
            await this.Login("administrator");
        }

        [Fact]
        public async void SetRole()
        {
            var workspace = this.Profile.Workspace;

            var c1 = workspace.Create<C1>();

            Assert.NotNull(c1);

            if (!c1.C1C1One2One.CanWrite)
            {
                await workspace.PullAsync(new Pull { Object = c1.Strategy });
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
                await workspace.PullAsync(new Pull { Object = c1.Strategy });
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

        [Fact]
        public async void SetRoleNull()
        {
            var workspace = this.Profile.Workspace;

            var c1 = workspace.Create<C1>();

            Assert.NotNull(c1);

            if (!c1.C1C1One2One.CanWrite)
            {
                await workspace.PullAsync(new Pull { Object = c1.Strategy });
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
                await workspace.PullAsync(new Pull { Object = c1.Strategy });
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

            if (!c1.C1C1One2One.CanWrite)
            {
                await workspace.PullAsync(new Pull { Object = c1.Strategy });
            }

            c1.C1AllorsBinary.Value = new byte[] { 1, 2 };
            c1.C1AllorsBoolean.Value = true;
            c1.C1AllorsDateTime.Value = new DateTime(1973, 3, 27, 12, 1, 2, 3, DateTimeKind.Utc);
            c1.C1AllorsDecimal.Value = 10.10m;
            c1.C1AllorsDouble.Value = 11.11d;
            c1.C1AllorsInteger.Value = 12;
            c1.C1AllorsString.Value = "a string";
            c1.C1AllorsUnique.Value = new Guid("0208BB9B-E87B-4CED-8DEC-516E6778CD66");


            if (!c1.C1C1One2One.CanWrite)
            {
                await workspace.PullAsync(new Pull { Object = c1.Strategy });
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
                await workspace.PullAsync(new Pull { Object = c1.Strategy });
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

        [Fact]
        public async void RemoveRole()
        {
            var workspace = this.Profile.Workspace;

            var c1 = workspace.Create<C1>();

            Assert.NotNull(c1);

            if (!c1.C1C1One2One.CanWrite)
            {
                await workspace.PullAsync(new Pull { Object = c1.Strategy });
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
                await workspace.PullAsync(new Pull { Object = c1.Strategy });
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

            if (!c1.C1C1One2One.CanWrite)
            {
                await workspace.PullAsync(new Pull { Object = c1.Strategy });
            }

            c1.C1AllorsBinary.Value = new byte[] { 1, 2 };
            c1.C1AllorsBoolean.Value = true;
            c1.C1AllorsDateTime.Value = new DateTime(1973, 3, 27, 12, 1, 2, 3, DateTimeKind.Utc);
            c1.C1AllorsDecimal.Value = 10.10m;
            c1.C1AllorsDouble.Value = 11.11d;
            c1.C1AllorsInteger.Value = 12;
            c1.C1AllorsString.Value = "a string";
            c1.C1AllorsUnique.Value = new Guid("0208BB9B-E87B-4CED-8DEC-516E6778CD66");

            if (!c1.C1C1One2One.CanWrite)
            {
                await workspace.PullAsync(new Pull { Object = c1.Strategy });
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
                await workspace.PullAsync(new Pull { Object = c1.Strategy });
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
