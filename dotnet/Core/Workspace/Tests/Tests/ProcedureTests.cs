// <copyright file="ObjectTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.Workspace
{
    using System;
    using System.Collections.Generic;
    using Allors.Workspace.Data;
    using Allors.Workspace.Domain;
    using Xunit;

    public abstract class ProcedureTests : Test
    {
        protected ProcedureTests(Fixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async void TestUnitSamplesWithNulls()
        {
            await this.Login("administrator");
            var session = this.Workspace.CreateWorkspace();

            var procedure = new Procedure("TestUnitSamples")
            {
                Values = new Dictionary<string, string> { { "step", "0" } }
            };

            var result = await session.CallAsync(procedure);

            Assert.False(result.HasErrors);

            var unitSample = result.GetObject("unitSample");

            Assert.False(unitSample.ExistRole(M.UnitSample.AllorsBinary));
            Assert.False(unitSample.ExistRole(M.UnitSample.AllorsBoolean));
            Assert.False(unitSample.ExistRole(M.UnitSample.AllorsDateTime));
            Assert.False(unitSample.ExistRole(M.UnitSample.AllorsDecimal));
            Assert.False(unitSample.ExistRole(M.UnitSample.AllorsDouble));
            Assert.False(unitSample.ExistRole(M.UnitSample.AllorsInteger));
            Assert.False(unitSample.ExistRole(M.UnitSample.AllorsString));
            Assert.False(unitSample.ExistRole(M.UnitSample.AllorsUnique));
        }

        [Fact]
        public async void TestUnitSamplesWithValues()
        {
            await this.Login("administrator");
            var session = this.Workspace.CreateWorkspace();

            var procedure = new Procedure("TestUnitSamples")
            {
                Values = new Dictionary<string, string> { { "step", "1" } }
            };

            var result = await session.CallAsync(procedure);

            Assert.False(result.HasErrors);

            var unitSample = result.GetObject("unitSample");

            Assert.True(unitSample.ExistRole(M.UnitSample.AllorsBinary));
            Assert.True(unitSample.ExistRole(M.UnitSample.AllorsBoolean));
            Assert.True(unitSample.ExistRole(M.UnitSample.AllorsDateTime));
            Assert.True(unitSample.ExistRole(M.UnitSample.AllorsDecimal));
            Assert.True(unitSample.ExistRole(M.UnitSample.AllorsDouble));
            Assert.True(unitSample.ExistRole(M.UnitSample.AllorsInteger));
            Assert.True(unitSample.ExistRole(M.UnitSample.AllorsString));
            Assert.True(unitSample.ExistRole(M.UnitSample.AllorsUnique));

            Assert.Equal(new byte[] { 1, 2, 3 }, unitSample.GetUnitRole(M.UnitSample.AllorsBinary));
            Assert.True((bool)unitSample.GetUnitRole(M.UnitSample.AllorsBoolean));
            Assert.Equal(new DateTime(1973, 3, 27, 0, 0, 0, DateTimeKind.Utc), unitSample.GetUnitRole(M.UnitSample.AllorsDateTime));
            Assert.Equal(12.34m, unitSample.GetUnitRole(M.UnitSample.AllorsDecimal));
            Assert.Equal(123d, unitSample.GetUnitRole(M.UnitSample.AllorsDouble));
            Assert.Equal(1000, unitSample.GetUnitRole(M.UnitSample.AllorsInteger));
            Assert.Equal("a string", unitSample.GetUnitRole(M.UnitSample.AllorsString));
            Assert.Equal(new Guid("2946CF37-71BE-4681-8FE6-D0024D59BEFF"), unitSample.GetUnitRole(M.UnitSample.AllorsUnique));
        }

        [Fact]
        public async void NonExistingProcedure()
        {
            await this.Login("administrator");

            var session = this.Workspace.CreateWorkspace();

            var procedure = new Procedure("ThisIsWrong")
            {
                Values = new Dictionary<string, string> { { "step", "0" } }
            };

            var result = await session.CallAsync(procedure);

            Assert.True(result.HasErrors);
        }
    }
}
