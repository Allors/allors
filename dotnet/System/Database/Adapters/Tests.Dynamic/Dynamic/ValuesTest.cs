// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValuesTest.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Database.Adapters;

using System;
using System.Linq;
using Meta;
using Xunit;

public abstract class ValuesTest : Test
{
    protected TestValues testValues = new();

    protected virtual int[] BinarySizes =>
        new[]
        {
            0, 1, 2, 8000 - 1, 8000, // SqlClient
            8000 + 1, 2 ^ (16 - 1), 2 ^ 16, // MySqlClient
            2 ^ (16 + 1), 2 ^ (32 - 1), 2 ^ 32, // MySqlClient
            2 ^ (32 + 1)
        };

    protected virtual int[] StringSizes =>
        new[]
        {
            0, 1, 2, 4000 - 1, 4000, // SqlClient
            4000 + 1, 8000 - 1, 8000, // SqlClient
            8000 + 1, 2 ^ (16 - 1), 2 ^ 16, // MySqlClient
            2 ^ (16 + 1), 2 ^ (32 - 1), 2 ^ 32, // MySqlClient
            2 ^ (32 + 1)
        };

    [Fact]
    [Trait("Category", "Dynamic")]
    public void AllorsBinary()
    {
        bool[] transactionFlags = {false, true};

        for (var transactionFlagIndex = 0; transactionFlagIndex < transactionFlags.Count(); transactionFlagIndex++)
        {
            var transactionFlag = transactionFlags[transactionFlagIndex];

            // Set
            for (var binarySizeIndex = 0; binarySizeIndex < this.BinarySizes.Count(); binarySizeIndex++)
            {
                var binarySize = this.BinarySizes[binarySizeIndex];

                for (var testTypeIndex = 0; testTypeIndex < this.GetTestTypes().Length; testTypeIndex++)
                {
                    var testType = this.GetTestTypes()[testTypeIndex];
                    var allorsObject = this.GetTransaction().Build(testType);
                    if (transactionFlag)
                    {
                        this.GetTransaction().Commit();
                    }

                    var testRoleTypes = this.GetBinaryRoles(testType);
                    for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
                    {
                        var testRoleType = testRoleTypes[testRoleTypeIndex];
                        var value = this.ValueGenerator.GenerateBinary(binarySize);

                        if (binarySize < testRoleType.RoleType.Size)
                        {
                            allorsObject.Strategy.SetUnitRole(testRoleType.RoleType, value);
                            if (transactionFlag)
                            {
                                this.GetTransaction().Commit();
                            }

                            Assert.Equal(value, allorsObject.Strategy.GetUnitRole(testRoleType.RoleType));
                            Assert.Equal(value, allorsObject.Strategy.GetUnitRole(testRoleType.RoleType));
                        }
                    }
                }
            }
        }
    }

    [Fact]
    [Trait("Category", "Dynamic")]
    public void AllorsBoolean()
    {
        bool[] transactionFlags = {false, true};
        var values = this.testValues.Booleans;

        for (var transactionFlagIndex = 0; transactionFlagIndex < transactionFlags.Count(); transactionFlagIndex++)
        {
            var transactionFlag = transactionFlags[transactionFlagIndex];

            // Set
            for (var valueIndex = 0; valueIndex < values.Count(); valueIndex++)
            {
                var value = values[valueIndex];
                for (var testTypeIndex = 0; testTypeIndex < this.GetTestTypes().Length; testTypeIndex++)
                {
                    var testType = this.GetTestTypes()[testTypeIndex];
                    var allorsObject = this.GetTransaction().Build(testType);
                    if (transactionFlag)
                    {
                        this.GetTransaction().Commit();
                    }

                    var testRoleTypes = this.GetBooleanRoles(testType);
                    for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
                    {
                        var testRoleType = testRoleTypes[testRoleTypeIndex];
                        allorsObject.Strategy.SetUnitRole(testRoleType.RoleType, value);
                        if (transactionFlag)
                        {
                            this.GetTransaction().Commit();
                        }

                        Assert.Equal(value, (bool)allorsObject.Strategy.GetUnitRole(testRoleType.RoleType));
                        Assert.Equal(value, (bool)allorsObject.Strategy.GetUnitRole(testRoleType.RoleType));
                    }
                }
            }

            // Initial empty
            for (var testTypeIndex = 0; testTypeIndex < this.GetTestTypes().Length; testTypeIndex++)
            {
                var testType = this.GetTestTypes()[testTypeIndex];
                var allorsObject = this.GetTransaction().Build(testType);
                if (transactionFlag)
                {
                    this.GetTransaction().Commit();
                }

                var testRoleTypes = this.GetBooleanRoles(testType);
                for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
                {
                    var testRoleType = testRoleTypes[testRoleTypeIndex];

                    Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                    Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));

                    if (transactionFlag)
                    {
                        this.GetTransaction().Commit();
                    }

                    Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                    Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                }
            }

            // Remove
            for (var valueIndex = 0; valueIndex < values.Count(); valueIndex++)
            {
                var value = values[valueIndex];
                for (var testTypeIndex = 0; testTypeIndex < this.GetTestTypes().Length; testTypeIndex++)
                {
                    var testType = this.GetTestTypes()[testTypeIndex];
                    var allorsObject = this.GetTransaction().Build(testType);

                    var testRoleTypes = this.GetBooleanRoles(testType);
                    for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
                    {
                        var testRoleType = testRoleTypes[testRoleTypeIndex];
                        allorsObject.Strategy.SetUnitRole(testRoleType.RoleType, value);
                        if (transactionFlag)
                        {
                            this.GetTransaction().Commit();
                        }

                        Assert.True(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.True(allorsObject.Strategy.ExistRole(testRoleType.RoleType));

                        allorsObject.Strategy.RemoveRole(testRoleType.RoleType);
                        if (transactionFlag)
                        {
                            this.GetTransaction().Commit();
                        }

                        if (transactionFlag)
                        {
                            this.GetTransaction().Commit();
                        }

                        Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));

                        Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                    }
                }
            }
        }

        if (this.IsRollbackSupported())
        {
            // Set
            for (var valueIndex = 0; valueIndex < values.Count(); valueIndex++)
            {
                var value = values[valueIndex];
                for (var testTypeIndex = 0; testTypeIndex < this.GetTestTypes().Length; testTypeIndex++)
                {
                    var testType = this.GetTestTypes()[testTypeIndex];
                    var allorsObject = this.GetTransaction().Build(testType);
                    this.GetTransaction().Commit();

                    var testRoleTypes = this.GetBooleanRoles(testType);
                    for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
                    {
                        var testRoleType = testRoleTypes[testRoleTypeIndex];
                        allorsObject.Strategy.SetUnitRole(testRoleType.RoleType, value);

                        this.GetTransaction().Rollback();

                        Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                    }
                }
            }

            // Commit Set Rollback
            for (var valueIndex = 0; valueIndex < values.Count(); valueIndex++)
            {
                var value = values[valueIndex];
                for (var testTypeIndex = 0; testTypeIndex < this.GetTestTypes().Length; testTypeIndex++)
                {
                    var testType = this.GetTestTypes()[testTypeIndex];
                    var allorsObject = this.GetTransaction().Build(testType);

                    var testRoleTypes = this.GetBooleanRoles(testType);
                    for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
                    {
                        var testRoleType = testRoleTypes[testRoleTypeIndex];
                        allorsObject.Strategy.SetUnitRole(testRoleType.RoleType, value);

                        this.GetTransaction().Commit();

                        var value2 = !value;

                        allorsObject.Strategy.SetUnitRole(testRoleType.RoleType, value2);

                        this.GetTransaction().Rollback();

                        Assert.True(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.True(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.Equal(value, (bool)allorsObject.Strategy.GetUnitRole(testRoleType.RoleType));
                        Assert.Equal(value, (bool)allorsObject.Strategy.GetUnitRole(testRoleType.RoleType));
                    }
                }
            }

            // Commit Remove Rollback
            for (var valueIndex = 0; valueIndex < values.Count(); valueIndex++)
            {
                var value = values[valueIndex];
                for (var testTypeIndex = 0; testTypeIndex < this.GetTestTypes().Length; testTypeIndex++)
                {
                    var testType = this.GetTestTypes()[testTypeIndex];
                    var allorsObject = this.GetTransaction().Build(testType);

                    var testRoleTypes = this.GetBooleanRoles(testType);
                    for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
                    {
                        var testRoleType = testRoleTypes[testRoleTypeIndex];
                        allorsObject.Strategy.SetUnitRole(testRoleType.RoleType, value);

                        this.GetTransaction().Commit();

                        allorsObject.Strategy.RemoveRole(testRoleType.RoleType);

                        this.GetTransaction().Rollback();

                        Assert.True(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.True(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.Equal(value, (bool)allorsObject.Strategy.GetUnitRole(testRoleType.RoleType));
                        Assert.Equal(value, (bool)allorsObject.Strategy.GetUnitRole(testRoleType.RoleType));
                    }
                }
            }
        }
    }

    [Fact]
    [Trait("Category", "Dynamic")]
    public void AllorsDateTime()
    {
        bool[] transactionFlags = {false, true};
        var values = new DateTime[this.testValues.DateTimes.Count()];
        for (var i = 0; i < values.Count(); i++)
        {
            values[i] = this.testValues.DateTimes[i];
        }

        for (var transactionFlagIndex = 0; transactionFlagIndex < transactionFlags.Count(); transactionFlagIndex++)
        {
            var transactionFlag = transactionFlags[transactionFlagIndex];

            // Set
            for (var valueIndex = 0; valueIndex < values.Count(); valueIndex++)
            {
                var value = values[valueIndex];
                for (var testTypeIndex = 0; testTypeIndex < this.GetTestTypes().Length; testTypeIndex++)
                {
                    var testType = this.GetTestTypes()[testTypeIndex];
                    var allorsObject = this.GetTransaction().Build(testType);
                    if (transactionFlag)
                    {
                        this.GetTransaction().Commit();
                    }

                    var testRoleTypes = this.GetDateTimeRoles(testType);
                    for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
                    {
                        var testRoleType = testRoleTypes[testRoleTypeIndex];
                        allorsObject.Strategy.SetUnitRole(testRoleType.RoleType, value);
                        if (transactionFlag)
                        {
                            this.GetTransaction().Commit();
                        }

                        var dateTime = (DateTime)allorsObject.Strategy.GetUnitRole(testRoleType.RoleType);
                        if (!dateTime.Equals(value))
                        {
                            Console.WriteLine(dateTime);
                        }

                        Assert.InRange((DateTime)allorsObject.Strategy.GetUnitRole(testRoleType.RoleType), value.AddMilliseconds(-1),
                            value.AddMilliseconds(1));
                        Assert.InRange((DateTime)allorsObject.Strategy.GetUnitRole(testRoleType.RoleType), value.AddMilliseconds(-1),
                            value.AddMilliseconds(1));
                    }
                }
            }

            // Initial empty
            for (var testTypeIndex = 0; testTypeIndex < this.GetTestTypes().Length; testTypeIndex++)
            {
                var testType = this.GetTestTypes()[testTypeIndex];
                var allorsObject = this.GetTransaction().Build(testType);
                if (transactionFlag)
                {
                    this.GetTransaction().Commit();
                }

                var testRoleTypes = this.GetDateTimeRoles(testType);
                for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
                {
                    var testRoleType = testRoleTypes[testRoleTypeIndex];

                    Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                    Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));

                    if (transactionFlag)
                    {
                        this.GetTransaction().Commit();
                    }

                    Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                    Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                }
            }

            // Remove
            for (var valueIndex = 0; valueIndex < values.Count(); valueIndex++)
            {
                object value = values[valueIndex];
                for (var testTypeIndex = 0; testTypeIndex < this.GetTestTypes().Length; testTypeIndex++)
                {
                    var testType = this.GetTestTypes()[testTypeIndex];
                    var allorsObject = this.GetTransaction().Build(testType);

                    var testRoleTypes = this.GetDateTimeRoles(testType);
                    for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
                    {
                        var testRoleType = testRoleTypes[testRoleTypeIndex];
                        allorsObject.Strategy.SetUnitRole(testRoleType.RoleType, value);
                        if (transactionFlag)
                        {
                            this.GetTransaction().Commit();
                        }

                        Assert.True(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.True(allorsObject.Strategy.ExistRole(testRoleType.RoleType));

                        allorsObject.Strategy.RemoveRole(testRoleType.RoleType);
                        if (transactionFlag)
                        {
                            this.GetTransaction().Commit();
                        }

                        if (transactionFlag)
                        {
                            this.GetTransaction().Commit();
                        }

                        Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));

                        Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                    }
                }
            }
        }

        if (this.IsRollbackSupported())
        {
            // Set
            for (var valueIndex = 0; valueIndex < values.Count(); valueIndex++)
            {
                object value = values[valueIndex];
                for (var testTypeIndex = 0; testTypeIndex < this.GetTestTypes().Length; testTypeIndex++)
                {
                    var testType = this.GetTestTypes()[testTypeIndex];
                    var allorsObject = this.GetTransaction().Build(testType);
                    this.GetTransaction().Commit();

                    var testRoleTypes = this.GetDateTimeRoles(testType);
                    for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
                    {
                        var testRoleType = testRoleTypes[testRoleTypeIndex];
                        allorsObject.Strategy.SetUnitRole(testRoleType.RoleType, value);

                        this.GetTransaction().Rollback();

                        Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                    }
                }
            }

            // Commit Set Rollback
            for (var valueIndex = 0; valueIndex < values.Count(); valueIndex++)
            {
                var value = values[valueIndex];
                for (var testTypeIndex = 0; testTypeIndex < this.GetTestTypes().Length; testTypeIndex++)
                {
                    var testType = this.GetTestTypes()[testTypeIndex];
                    var allorsObject = this.GetTransaction().Build(testType);

                    var testRoleTypes = this.GetDateTimeRoles(testType);
                    for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
                    {
                        var testRoleType = testRoleTypes[testRoleTypeIndex];
                        allorsObject.Strategy.SetUnitRole(testRoleType.RoleType, value);

                        this.GetTransaction().Commit();

                        var value2 = this.ValueGenerator.GenerateDateTime();

                        allorsObject.Strategy.SetUnitRole(testRoleType.RoleType, value2);

                        this.GetTransaction().Rollback();

                        Assert.True(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.True(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.InRange((DateTime)allorsObject.Strategy.GetUnitRole(testRoleType.RoleType), value.AddMilliseconds(-1),
                            value.AddMilliseconds(1));
                        Assert.InRange((DateTime)allorsObject.Strategy.GetUnitRole(testRoleType.RoleType), value.AddMilliseconds(-1),
                            value.AddMilliseconds(1));
                    }
                }
            }

            // Commit Remove Rollback
            for (var valueIndex = 0; valueIndex < values.Count(); valueIndex++)
            {
                var value = values[valueIndex];
                for (var testTypeIndex = 0; testTypeIndex < this.GetTestTypes().Length; testTypeIndex++)
                {
                    var testType = this.GetTestTypes()[testTypeIndex];
                    var allorsObject = this.GetTransaction().Build(testType);

                    var testRoleTypes = this.GetDateTimeRoles(testType);
                    for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
                    {
                        var testRoleType = testRoleTypes[testRoleTypeIndex];
                        allorsObject.Strategy.SetUnitRole(testRoleType.RoleType, value);

                        this.GetTransaction().Commit();

                        allorsObject.Strategy.RemoveRole(testRoleType.RoleType);

                        this.GetTransaction().Rollback();

                        Assert.True(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.True(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.InRange((DateTime)allorsObject.Strategy.GetUnitRole(testRoleType.RoleType), value.AddMilliseconds(-1),
                            value.AddMilliseconds(1));
                        Assert.InRange((DateTime)allorsObject.Strategy.GetUnitRole(testRoleType.RoleType), value.AddMilliseconds(-1),
                            value.AddMilliseconds(1));
                    }
                }
            }
        }
    }

    [Fact]
    [Trait("Category", "Dynamic")]
    public void AllorsDecimal()
    {
        bool[] transactionFlags = {false, true};
        var values = new object[this.testValues.Decimals.Count()];
        for (var i = 0; i < values.Count(); i++)
        {
            values[i] = this.testValues.Decimals[i];
        }

        for (var transactionFlagIndex = 0; transactionFlagIndex < transactionFlags.Count(); transactionFlagIndex++)
        {
            var transactionFlag = transactionFlags[transactionFlagIndex];

            // Set
            for (var valueIndex = 0; valueIndex < values.Count(); valueIndex++)
            {
                var value = values[valueIndex];
                for (var testTypeIndex = 0; testTypeIndex < this.GetTestTypes().Length; testTypeIndex++)
                {
                    var testType = this.GetTestTypes()[testTypeIndex];
                    var allorsObject = this.GetTransaction().Build(testType);
                    if (transactionFlag)
                    {
                        this.GetTransaction().Commit();
                    }

                    var testRoleTypes = this.GetDecimalRoles(testType);
                    for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
                    {
                        var testRoleType = testRoleTypes[testRoleTypeIndex];
                        allorsObject.Strategy.SetUnitRole(testRoleType.RoleType, value);
                        if (transactionFlag)
                        {
                            this.GetTransaction().Commit();
                        }

                        Assert.Equal(value, (decimal)allorsObject.Strategy.GetUnitRole(testRoleType.RoleType));
                        Assert.Equal(value, (decimal)allorsObject.Strategy.GetUnitRole(testRoleType.RoleType));
                    }
                }
            }

            // Initial empty
            for (var testTypeIndex = 0; testTypeIndex < this.GetTestTypes().Length; testTypeIndex++)
            {
                var testType = this.GetTestTypes()[testTypeIndex];
                var allorsObject = this.GetTransaction().Build(testType);
                if (transactionFlag)
                {
                    this.GetTransaction().Commit();
                }

                var testRoleTypes = this.GetDecimalRoles(testType);
                for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
                {
                    var testRoleType = testRoleTypes[testRoleTypeIndex];

                    Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                    Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));

                    if (transactionFlag)
                    {
                        this.GetTransaction().Commit();
                    }

                    Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                    Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                }
            }

            // Remove
            for (var valueIndex = 0; valueIndex < values.Count(); valueIndex++)
            {
                var value = values[valueIndex];
                for (var testTypeIndex = 0; testTypeIndex < this.GetTestTypes().Length; testTypeIndex++)
                {
                    var testType = this.GetTestTypes()[testTypeIndex];
                    var allorsObject = this.GetTransaction().Build(testType);

                    var testRoleTypes = this.GetDecimalRoles(testType);
                    for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
                    {
                        var testRoleType = testRoleTypes[testRoleTypeIndex];
                        allorsObject.Strategy.SetUnitRole(testRoleType.RoleType, value);
                        if (transactionFlag)
                        {
                            this.GetTransaction().Commit();
                        }

                        Assert.True(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.True(allorsObject.Strategy.ExistRole(testRoleType.RoleType));

                        allorsObject.Strategy.RemoveRole(testRoleType.RoleType);
                        if (transactionFlag)
                        {
                            this.GetTransaction().Commit();
                        }

                        if (transactionFlag)
                        {
                            this.GetTransaction().Commit();
                        }

                        Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));

                        Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                    }
                }
            }
        }

        if (this.IsRollbackSupported())
        {
            // Set
            for (var valueIndex = 0; valueIndex < values.Count(); valueIndex++)
            {
                var value = values[valueIndex];
                for (var testTypeIndex = 0; testTypeIndex < this.GetTestTypes().Length; testTypeIndex++)
                {
                    var testType = this.GetTestTypes()[testTypeIndex];
                    var allorsObject = this.GetTransaction().Build(testType);
                    this.GetTransaction().Commit();

                    var testRoleTypes = this.GetDecimalRoles(testType);
                    for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
                    {
                        var testRoleType = testRoleTypes[testRoleTypeIndex];
                        allorsObject.Strategy.SetUnitRole(testRoleType.RoleType, value);

                        this.GetTransaction().Rollback();

                        Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                    }
                }
            }

            // Commit Set Rollback
            for (var valueIndex = 0; valueIndex < values.Count(); valueIndex++)
            {
                var value = values[valueIndex];
                for (var testTypeIndex = 0; testTypeIndex < this.GetTestTypes().Length; testTypeIndex++)
                {
                    var testType = this.GetTestTypes()[testTypeIndex];
                    var allorsObject = this.GetTransaction().Build(testType);

                    var testRoleTypes = this.GetDecimalRoles(testType);
                    for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
                    {
                        var testRoleType = testRoleTypes[testRoleTypeIndex];
                        allorsObject.Strategy.SetUnitRole(testRoleType.RoleType, value);

                        this.GetTransaction().Commit();

                        object value2 = this.ValueGenerator.GenerateDecimal();

                        allorsObject.Strategy.SetUnitRole(testRoleType.RoleType, value2);

                        this.GetTransaction().Rollback();

                        Assert.True(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.True(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.Equal(value, (decimal)allorsObject.Strategy.GetUnitRole(testRoleType.RoleType));
                        Assert.Equal(value, (decimal)allorsObject.Strategy.GetUnitRole(testRoleType.RoleType));
                    }
                }
            }

            // Commit Remove Rollback
            for (var valueIndex = 0; valueIndex < values.Count(); valueIndex++)
            {
                var value = values[valueIndex];
                for (var testTypeIndex = 0; testTypeIndex < this.GetTestTypes().Length; testTypeIndex++)
                {
                    var testType = this.GetTestTypes()[testTypeIndex];
                    var allorsObject = this.GetTransaction().Build(testType);

                    var testRoleTypes = this.GetDecimalRoles(testType);
                    for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
                    {
                        var testRoleType = testRoleTypes[testRoleTypeIndex];
                        allorsObject.Strategy.SetUnitRole(testRoleType.RoleType, value);

                        this.GetTransaction().Commit();

                        allorsObject.Strategy.RemoveRole(testRoleType.RoleType);

                        this.GetTransaction().Rollback();

                        Assert.True(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.True(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.Equal(value, (decimal)allorsObject.Strategy.GetUnitRole(testRoleType.RoleType));
                        Assert.Equal(value, (decimal)allorsObject.Strategy.GetUnitRole(testRoleType.RoleType));
                    }
                }
            }
        }
    }

    [Fact]
    [Trait("Category", "Dynamic")]
    public void AllorsDouble()
    {
        bool[] transactionFlags = {false, true};
        var values = this.testValues.Floats;

        for (var transactionFlagIndex = 0; transactionFlagIndex < transactionFlags.Count(); transactionFlagIndex++)
        {
            var transactionFlag = transactionFlags[transactionFlagIndex];

            // Set
            for (var valueIndex = 0; valueIndex < values.Count(); valueIndex++)
            {
                var value = values[valueIndex];
                for (var testTypeIndex = 0; testTypeIndex < this.GetTestTypes().Length; testTypeIndex++)
                {
                    var testType = this.GetTestTypes()[testTypeIndex];
                    var allorsObject = this.GetTransaction().Build(testType);
                    if (transactionFlag)
                    {
                        this.GetTransaction().Commit();
                    }

                    var testRoleTypes = this.GetFloatRoles(testType);
                    for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
                    {
                        var testRoleType = testRoleTypes[testRoleTypeIndex];
                        allorsObject.Strategy.SetUnitRole(testRoleType.RoleType, value);
                        if (transactionFlag)
                        {
                            this.GetTransaction().Commit();
                        }

                        Assert.Equal(value, (double)allorsObject.Strategy.GetUnitRole(testRoleType.RoleType));
                        Assert.Equal(value, (double)allorsObject.Strategy.GetUnitRole(testRoleType.RoleType));
                    }
                }
            }

            // Initial empty
            for (var testTypeIndex = 0; testTypeIndex < this.GetTestTypes().Length; testTypeIndex++)
            {
                var testType = this.GetTestTypes()[testTypeIndex];
                var allorsObject = this.GetTransaction().Build(testType);
                if (transactionFlag)
                {
                    this.GetTransaction().Commit();
                }

                var testRoleTypes = this.GetFloatRoles(testType);
                for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
                {
                    var testRoleType = testRoleTypes[testRoleTypeIndex];

                    Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                    Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));

                    if (transactionFlag)
                    {
                        this.GetTransaction().Commit();
                    }

                    Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                    Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                }
            }

            // Remove
            for (var valueIndex = 0; valueIndex < values.Count(); valueIndex++)
            {
                var value = values[valueIndex];
                for (var testTypeIndex = 0; testTypeIndex < this.GetTestTypes().Length; testTypeIndex++)
                {
                    var testType = this.GetTestTypes()[testTypeIndex];
                    var allorsObject = this.GetTransaction().Build(testType);

                    var testRoleTypes = this.GetFloatRoles(testType);
                    for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
                    {
                        var testRoleType = testRoleTypes[testRoleTypeIndex];
                        allorsObject.Strategy.SetUnitRole(testRoleType.RoleType, value);
                        if (transactionFlag)
                        {
                            this.GetTransaction().Commit();
                        }

                        Assert.True(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.True(allorsObject.Strategy.ExistRole(testRoleType.RoleType));

                        allorsObject.Strategy.RemoveRole(testRoleType.RoleType);
                        if (transactionFlag)
                        {
                            this.GetTransaction().Commit();
                        }

                        if (transactionFlag)
                        {
                            this.GetTransaction().Commit();
                        }

                        Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));

                        Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                    }
                }
            }
        }

        if (this.IsRollbackSupported())
        {
            // Set
            for (var valueIndex = 0; valueIndex < values.Count(); valueIndex++)
            {
                var value = values[valueIndex];
                for (var testTypeIndex = 0; testTypeIndex < this.GetTestTypes().Length; testTypeIndex++)
                {
                    var testType = this.GetTestTypes()[testTypeIndex];
                    var allorsObject = this.GetTransaction().Build(testType);
                    this.GetTransaction().Commit();

                    var testRoleTypes = this.GetFloatRoles(testType);
                    for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
                    {
                        var testRoleType = testRoleTypes[testRoleTypeIndex];
                        allorsObject.Strategy.SetUnitRole(testRoleType.RoleType, value);

                        this.GetTransaction().Rollback();

                        Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                    }
                }
            }

            // Commit Set Rollback
            for (var valueIndex = 0; valueIndex < values.Count(); valueIndex++)
            {
                var value = values[valueIndex];
                for (var testTypeIndex = 0; testTypeIndex < this.GetTestTypes().Length; testTypeIndex++)
                {
                    var testType = this.GetTestTypes()[testTypeIndex];
                    var allorsObject = this.GetTransaction().Build(testType);

                    var testRoleTypes = this.GetFloatRoles(testType);
                    for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
                    {
                        var testRoleType = testRoleTypes[testRoleTypeIndex];
                        allorsObject.Strategy.SetUnitRole(testRoleType.RoleType, value);

                        this.GetTransaction().Commit();

                        var value2 = this.ValueGenerator.GenerateFloat();

                        allorsObject.Strategy.SetUnitRole(testRoleType.RoleType, value2);

                        this.GetTransaction().Rollback();

                        Assert.True(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.True(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.Equal(value, (double)allorsObject.Strategy.GetUnitRole(testRoleType.RoleType));
                        Assert.Equal(value, (double)allorsObject.Strategy.GetUnitRole(testRoleType.RoleType));
                    }
                }
            }

            // Commit Remove Rollback
            for (var valueIndex = 0; valueIndex < values.Count(); valueIndex++)
            {
                var value = values[valueIndex];
                for (var testTypeIndex = 0; testTypeIndex < this.GetTestTypes().Length; testTypeIndex++)
                {
                    var testType = this.GetTestTypes()[testTypeIndex];
                    var allorsObject = this.GetTransaction().Build(testType);

                    var testRoleTypes = this.GetFloatRoles(testType);
                    for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
                    {
                        var testRoleType = testRoleTypes[testRoleTypeIndex];
                        allorsObject.Strategy.SetUnitRole(testRoleType.RoleType, value);

                        this.GetTransaction().Commit();

                        allorsObject.Strategy.RemoveRole(testRoleType.RoleType);

                        this.GetTransaction().Rollback();

                        Assert.True(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.True(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.Equal(value, (double)allorsObject.Strategy.GetUnitRole(testRoleType.RoleType));
                        Assert.Equal(value, (double)allorsObject.Strategy.GetUnitRole(testRoleType.RoleType));
                    }
                }
            }
        }
    }

    [Fact]
    [Trait("Category", "Dynamic")]
    public void AllorsInteger()
    {
        bool[] transactionFlags = {false, true};
        var values = this.testValues.Integers;

        for (var transactionFlagIndex = 0; transactionFlagIndex < transactionFlags.Count(); transactionFlagIndex++)
        {
            var transactionFlag = transactionFlags[transactionFlagIndex];

            // Set
            for (var valueIndex = 0; valueIndex < values.Count(); valueIndex++)
            {
                var value = values[valueIndex];

                var testTypes = this.GetTestTypes();
                for (var testTypeIndex = 0; testTypeIndex < testTypes.Count(); testTypeIndex++)
                {
                    var testType = testTypes[testTypeIndex];
                    var allorsObject = this.GetTransaction().Build(testType);
                    if (transactionFlag)
                    {
                        this.GetTransaction().Commit();
                    }

                    var testRoleTypes = this.GetIntegerRoles(testType);
                    for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
                    {
                        var testRoleType = testRoleTypes[testRoleTypeIndex];
                        allorsObject.Strategy.SetUnitRole(testRoleType.RoleType, value);
                        if (transactionFlag)
                        {
                            this.GetTransaction().Commit();
                        }

                        Assert.Equal(value, (int)allorsObject.Strategy.GetUnitRole(testRoleType.RoleType));
                        Assert.Equal(value, (int)allorsObject.Strategy.GetUnitRole(testRoleType.RoleType));
                    }
                }
            }

            // Initial empty
            for (var testTypeIndex = 0; testTypeIndex < this.GetTestTypes().Length; testTypeIndex++)
            {
                var testType = this.GetTestTypes()[testTypeIndex];
                var allorsObject = this.GetTransaction().Build(testType);
                if (transactionFlag)
                {
                    this.GetTransaction().Commit();
                }

                var testRoleTypes = this.GetIntegerRoles(testType);
                for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
                {
                    var testRoleType = testRoleTypes[testRoleTypeIndex];

                    Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                    Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));

                    if (transactionFlag)
                    {
                        this.GetTransaction().Commit();
                    }

                    Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                    Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                }
            }

            // Remove
            for (var valueIndex = 0; valueIndex < values.Count(); valueIndex++)
            {
                var value = values[valueIndex];
                for (var testTypeIndex = 0; testTypeIndex < this.GetTestTypes().Length; testTypeIndex++)
                {
                    var testType = this.GetTestTypes()[testTypeIndex];
                    var allorsObject = this.GetTransaction().Build(testType);

                    var testRoleTypes = this.GetIntegerRoles(testType);
                    for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
                    {
                        var testRoleType = testRoleTypes[testRoleTypeIndex];
                        allorsObject.Strategy.SetUnitRole(testRoleType.RoleType, value);
                        if (transactionFlag)
                        {
                            this.GetTransaction().Commit();
                        }

                        Assert.True(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.True(allorsObject.Strategy.ExistRole(testRoleType.RoleType));

                        allorsObject.Strategy.RemoveRole(testRoleType.RoleType);
                        if (transactionFlag)
                        {
                            this.GetTransaction().Commit();
                        }

                        if (transactionFlag)
                        {
                            this.GetTransaction().Commit();
                        }

                        Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));

                        Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                    }
                }
            }
        }

        if (this.IsRollbackSupported())
        {
            // Set
            for (var valueIndex = 0; valueIndex < values.Count(); valueIndex++)
            {
                var value = values[valueIndex];
                for (var testTypeIndex = 0; testTypeIndex < this.GetTestTypes().Length; testTypeIndex++)
                {
                    var testType = this.GetTestTypes()[testTypeIndex];
                    var allorsObject = this.GetTransaction().Build(testType);
                    this.GetTransaction().Commit();

                    var testRoleTypes = this.GetIntegerRoles(testType);
                    for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
                    {
                        var testRoleType = testRoleTypes[testRoleTypeIndex];
                        allorsObject.Strategy.SetUnitRole(testRoleType.RoleType, value);

                        this.GetTransaction().Rollback();

                        Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                    }
                }
            }

            // Commit Set Rollback
            for (var valueIndex = 0; valueIndex < values.Count(); valueIndex++)
            {
                var value = values[valueIndex];
                for (var testTypeIndex = 0; testTypeIndex < this.GetTestTypes().Length; testTypeIndex++)
                {
                    var testType = this.GetTestTypes()[testTypeIndex];
                    var allorsObject = this.GetTransaction().Build(testType);

                    var testRoleTypes = this.GetIntegerRoles(testType);
                    for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
                    {
                        var testRoleType = testRoleTypes[testRoleTypeIndex];
                        allorsObject.Strategy.SetUnitRole(testRoleType.RoleType, value);

                        this.GetTransaction().Commit();

                        var value2 = this.ValueGenerator.GenerateInteger();

                        allorsObject.Strategy.SetUnitRole(testRoleType.RoleType, value2);

                        this.GetTransaction().Rollback();

                        Assert.True(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.True(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.Equal(value, (int)allorsObject.Strategy.GetUnitRole(testRoleType.RoleType));
                        Assert.Equal(value, (int)allorsObject.Strategy.GetUnitRole(testRoleType.RoleType));
                    }
                }
            }

            // Commit Remove Rollback
            for (var valueIndex = 0; valueIndex < values.Count(); valueIndex++)
            {
                var value = values[valueIndex];
                for (var testTypeIndex = 0; testTypeIndex < this.GetTestTypes().Length; testTypeIndex++)
                {
                    var testType = this.GetTestTypes()[testTypeIndex];
                    var allorsObject = this.GetTransaction().Build(testType);

                    var testRoleTypes = this.GetIntegerRoles(testType);
                    for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
                    {
                        var testRoleType = testRoleTypes[testRoleTypeIndex];
                        allorsObject.Strategy.SetUnitRole(testRoleType.RoleType, value);

                        this.GetTransaction().Commit();

                        allorsObject.Strategy.RemoveRole(testRoleType.RoleType);

                        this.GetTransaction().Rollback();

                        Assert.True(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.True(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.Equal(value, (int)allorsObject.Strategy.GetUnitRole(testRoleType.RoleType));
                        Assert.Equal(value, (int)allorsObject.Strategy.GetUnitRole(testRoleType.RoleType));
                    }
                }
            }
        }
    }

    [Fact]
    [Trait("Category", "Dynamic")]
    public void AllorsString()
    {
        bool[] transactionFlags = {false, true};

        for (var transactionFlagIndex = 0; transactionFlagIndex < transactionFlags.Count(); transactionFlagIndex++)
        {
            var transactionFlag = transactionFlags[transactionFlagIndex];

            // Set
            for (var stringSizeIndex = 0; stringSizeIndex < this.StringSizes.Count(); stringSizeIndex++)
            {
                var stringSize = this.StringSizes[stringSizeIndex];

                for (var testTypeIndex = 0; testTypeIndex < this.GetTestTypes().Length; testTypeIndex++)
                {
                    var testType = this.GetTestTypes()[testTypeIndex];
                    var allorsObject = this.GetTransaction().Build(testType);
                    if (transactionFlag)
                    {
                        this.GetTransaction().Commit();
                    }

                    var testRoleTypes = this.GetStringRoles(testType);
                    for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
                    {
                        var testRoleType = testRoleTypes[testRoleTypeIndex];
                        var value = this.ValueGenerator.GenerateString(stringSize);

                        if (stringSize < testRoleType.RoleType.Size)
                        {
                            allorsObject.Strategy.SetUnitRole(testRoleType.RoleType, value);
                            if (transactionFlag)
                            {
                                this.GetTransaction().Commit();
                            }

                            Assert.Equal(value, allorsObject.Strategy.GetUnitRole(testRoleType.RoleType));
                            Assert.Equal(value, allorsObject.Strategy.GetUnitRole(testRoleType.RoleType));
                        }
                    }
                }
            }
        }
    }

    [Fact]
    [Trait("Category", "Dynamic")]
    public void AllorsUnique()
    {
        bool[] transactionFlags = {false, true};
        var values = this.testValues.Uniques;

        for (var transactionFlagIndex = 0; transactionFlagIndex < transactionFlags.Count(); transactionFlagIndex++)
        {
            var transactionFlag = transactionFlags[transactionFlagIndex];

            // Set
            for (var valueIndex = 0; valueIndex < values.Count(); valueIndex++)
            {
                var value = values[valueIndex];

                var testTypes = this.GetTestTypes();
                for (var testTypeIndex = 0; testTypeIndex < testTypes.Count(); testTypeIndex++)
                {
                    var testType = testTypes[testTypeIndex];
                    var allorsObject = this.GetTransaction().Build(testType);
                    if (transactionFlag)
                    {
                        this.GetTransaction().Commit();
                    }

                    var testRoleTypes = this.GetUniqueRoles(testType);
                    for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
                    {
                        var testRoleType = testRoleTypes[testRoleTypeIndex];
                        allorsObject.Strategy.SetUnitRole(testRoleType.RoleType, value);
                        if (transactionFlag)
                        {
                            this.GetTransaction().Commit();
                        }

                        Assert.Equal(value, (Guid)allorsObject.Strategy.GetUnitRole(testRoleType.RoleType));
                        Assert.Equal(value, (Guid)allorsObject.Strategy.GetUnitRole(testRoleType.RoleType));
                    }
                }
            }

            // Initial empty
            for (var testTypeIndex = 0; testTypeIndex < this.GetTestTypes().Length; testTypeIndex++)
            {
                var testType = this.GetTestTypes()[testTypeIndex];
                var allorsObject = this.GetTransaction().Build(testType);
                if (transactionFlag)
                {
                    this.GetTransaction().Commit();
                }

                var testRoleTypes = this.GetUniqueRoles(testType);
                for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
                {
                    var testRoleType = testRoleTypes[testRoleTypeIndex];

                    Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                    Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));

                    if (transactionFlag)
                    {
                        this.GetTransaction().Commit();
                    }

                    Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                    Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                }
            }

            // Remove
            for (var valueIndex = 0; valueIndex < values.Count(); valueIndex++)
            {
                var value = values[valueIndex];
                for (var testTypeIndex = 0; testTypeIndex < this.GetTestTypes().Length; testTypeIndex++)
                {
                    var testType = this.GetTestTypes()[testTypeIndex];
                    var allorsObject = this.GetTransaction().Build(testType);

                    var testRoleTypes = this.GetUniqueRoles(testType);
                    for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
                    {
                        var testRoleType = testRoleTypes[testRoleTypeIndex];
                        allorsObject.Strategy.SetUnitRole(testRoleType.RoleType, value);
                        if (transactionFlag)
                        {
                            this.GetTransaction().Commit();
                        }

                        Assert.True(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.True(allorsObject.Strategy.ExistRole(testRoleType.RoleType));

                        allorsObject.Strategy.RemoveRole(testRoleType.RoleType);
                        if (transactionFlag)
                        {
                            this.GetTransaction().Commit();
                        }

                        if (transactionFlag)
                        {
                            this.GetTransaction().Commit();
                        }

                        Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));

                        Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                    }
                }
            }
        }

        if (this.IsRollbackSupported())
        {
            // Set
            for (var valueIndex = 0; valueIndex < values.Count(); valueIndex++)
            {
                var value = values[valueIndex];
                for (var testTypeIndex = 0; testTypeIndex < this.GetTestTypes().Length; testTypeIndex++)
                {
                    var testType = this.GetTestTypes()[testTypeIndex];
                    var allorsObject = this.GetTransaction().Build(testType);
                    this.GetTransaction().Commit();

                    var testRoleTypes = this.GetUniqueRoles(testType);
                    for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
                    {
                        var testRoleType = testRoleTypes[testRoleTypeIndex];
                        allorsObject.Strategy.SetUnitRole(testRoleType.RoleType, value);

                        this.GetTransaction().Rollback();

                        Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.False(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                    }
                }
            }

            // Commit Set Rollback
            for (var valueIndex = 0; valueIndex < values.Count(); valueIndex++)
            {
                var value = values[valueIndex];
                for (var testTypeIndex = 0; testTypeIndex < this.GetTestTypes().Length; testTypeIndex++)
                {
                    var testType = this.GetTestTypes()[testTypeIndex];
                    var allorsObject = this.GetTransaction().Build(testType);

                    var testRoleTypes = this.GetUniqueRoles(testType);
                    for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
                    {
                        var testRoleType = testRoleTypes[testRoleTypeIndex];
                        allorsObject.Strategy.SetUnitRole(testRoleType.RoleType, value);

                        this.GetTransaction().Commit();

                        var value2 = this.ValueGenerator.GenerateUnique();

                        allorsObject.Strategy.SetUnitRole(testRoleType.RoleType, value2);

                        this.GetTransaction().Rollback();

                        Assert.True(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.True(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.Equal(value, (Guid)allorsObject.Strategy.GetUnitRole(testRoleType.RoleType));
                        Assert.Equal(value, (Guid)allorsObject.Strategy.GetUnitRole(testRoleType.RoleType));
                    }
                }
            }

            // Commit Remove Rollback
            for (var valueIndex = 0; valueIndex < values.Count(); valueIndex++)
            {
                var value = values[valueIndex];
                for (var testTypeIndex = 0; testTypeIndex < this.GetTestTypes().Length; testTypeIndex++)
                {
                    var testType = this.GetTestTypes()[testTypeIndex];
                    var allorsObject = this.GetTransaction().Build(testType);

                    var testRoleTypes = this.GetUniqueRoles(testType);
                    for (var testRoleTypeIndex = 0; testRoleTypeIndex < testRoleTypes.Count(); testRoleTypeIndex++)
                    {
                        var testRoleType = testRoleTypes[testRoleTypeIndex];
                        allorsObject.Strategy.SetUnitRole(testRoleType.RoleType, value);

                        this.GetTransaction().Commit();

                        allorsObject.Strategy.RemoveRole(testRoleType.RoleType);

                        this.GetTransaction().Rollback();

                        Assert.True(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.True(allorsObject.Strategy.ExistRole(testRoleType.RoleType));
                        Assert.Equal(value, (Guid)allorsObject.Strategy.GetUnitRole(testRoleType.RoleType));
                        Assert.Equal(value, (Guid)allorsObject.Strategy.GetUnitRole(testRoleType.RoleType));
                    }
                }
            }
        }
    }

    private IClass[] GetTestTypes() => this.GetMetaPopulation().Classes.ToArray();
}
