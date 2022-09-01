// <copyright file="ObjectTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.Workspace
{
    using System;
    using System.Collections.Generic;
    using Allors.Workspace.Request;
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
            var m = this.M;

            foreach (var connection in this.Connections)
            {
                var procedure = new ProcedureCall("TestUnitSamples")
                {
                    Values = new Dictionary<string, string> { { "step", "0" } }
                };

                var result = await connection.CallAsync(procedure);

                Assert.False(result.HasErrors);

                var unitSample = result.GetObject("unitSample");

                Assert.False(unitSample.ExistRole(m.UnitSample.AllorsBinary));
                Assert.False(unitSample.ExistRole(m.UnitSample.AllorsBoolean));
                Assert.False(unitSample.ExistRole(m.UnitSample.AllorsDateTime));
                Assert.False(unitSample.ExistRole(m.UnitSample.AllorsDecimal));
                Assert.False(unitSample.ExistRole(m.UnitSample.AllorsDouble));
                Assert.False(unitSample.ExistRole(m.UnitSample.AllorsInteger));
                Assert.False(unitSample.ExistRole(m.UnitSample.AllorsString));
                Assert.False(unitSample.ExistRole(m.UnitSample.AllorsUnique));

            }
        }

        [Fact]
        public async void TestUnitSamplesWithValues()
        {
            await this.Login("administrator");
            var m = this.M;

            foreach (var connection in this.Connections)
            {
                var procedure = new ProcedureCall("TestUnitSamples")
                {
                    Values = new Dictionary<string, string> { { "step", "1" } }
                };

                var result = await connection.CallAsync(procedure);

                Assert.False(result.HasErrors);

                var unitSample = result.GetObject("unitSample");

                Assert.True(unitSample.ExistRole(m.UnitSample.AllorsBinary));
                Assert.True(unitSample.ExistRole(m.UnitSample.AllorsBoolean));
                Assert.True(unitSample.ExistRole(m.UnitSample.AllorsDateTime));
                Assert.True(unitSample.ExistRole(m.UnitSample.AllorsDecimal));
                Assert.True(unitSample.ExistRole(m.UnitSample.AllorsDouble));
                Assert.True(unitSample.ExistRole(m.UnitSample.AllorsInteger));
                Assert.True(unitSample.ExistRole(m.UnitSample.AllorsString));
                Assert.True(unitSample.ExistRole(m.UnitSample.AllorsUnique));

                Assert.Equal(new byte[] { 1, 2, 3 }, unitSample.GetUnitRole(m.UnitSample.AllorsBinary));
                Assert.True((bool)unitSample.GetUnitRole(m.UnitSample.AllorsBoolean));
                Assert.Equal(new DateTime(1973, 3, 27, 0, 0, 0, DateTimeKind.Utc), unitSample.GetUnitRole(m.UnitSample.AllorsDateTime));
                Assert.Equal(12.34m, unitSample.GetUnitRole(m.UnitSample.AllorsDecimal));
                Assert.Equal(123d, unitSample.GetUnitRole(m.UnitSample.AllorsDouble));
                Assert.Equal(1000, unitSample.GetUnitRole(m.UnitSample.AllorsInteger));
                Assert.Equal("a string", unitSample.GetUnitRole(m.UnitSample.AllorsString));
                Assert.Equal(new Guid("2946CF37-71BE-4681-8FE6-D0024D59BEFF"), unitSample.GetUnitRole(m.UnitSample.AllorsUnique));
            }
        }

        [Fact]
        public async void NonExistingProcedure()
        {
            await this.Login("administrator");
            var m = this.M;

            foreach (var connection in this.Connections)
            {
                var procedure = new ProcedureCall("ThisIsWrong")
                {
                    Values = new Dictionary<string, string> { { "step", "0" } }
                };

                var result = await connection.CallAsync(procedure);

                Assert.True(result.HasErrors);
            }
        }
    }
}
