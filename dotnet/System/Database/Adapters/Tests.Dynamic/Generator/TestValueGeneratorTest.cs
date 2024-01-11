// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestValueGeneratorTest.cs" company="Allors bv">
//   Copyright 2002-2012 Allors bv.
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

using System.Linq;
using Xunit;

public class TestValueGeneratorTest
{
    private readonly TestValueGenerator testValueGenerator = new();

    [Fact]
    [Trait("Category", "Dynamic")]
    public void GenerateBoolean()
    {
        var value = this.testValueGenerator.GenerateBoolean();
        var differentValueFound = false;

        for (var i = 0; i < 100; i++)
        {
            var newValue = this.testValueGenerator.GenerateBoolean();
            if (newValue != value)
            {
                differentValueFound = true;
                break;
            }
        }

        Assert.True(differentValueFound);
    }

    [Fact]
    [Trait("Category", "Dynamic")]
    public void GenerateDateTime()
    {
        var value1 = this.testValueGenerator.GenerateDateTime();
        var value2 = this.testValueGenerator.GenerateDateTime();

        Assert.NotEqual(value1, value2);
    }

    [Fact]
    [Trait("Category", "Dynamic")]
    public void GenerateDecimal()
    {
        var value1 = this.testValueGenerator.GenerateDecimal();
        var value2 = this.testValueGenerator.GenerateDecimal();

        Assert.NotEqual(value1, value2);
    }

    [Fact]
    [Trait("Category", "Dynamic")]
    public void GenerateFloat()
    {
        var value1 = this.testValueGenerator.GenerateFloat();
        var value2 = this.testValueGenerator.GenerateFloat();

        Assert.NotEqual(value1, value2);
    }

    [Fact]
    [Trait("Category", "Dynamic")]
    public void GenerateInteger()
    {
        var value1 = this.testValueGenerator.GenerateInteger();
        var value2 = this.testValueGenerator.GenerateInteger();

        Assert.NotEqual(value1, value2);
    }

    [Fact]
    [Trait("Category", "Dynamic")]
    public void GeneratePercentage()
    {
        var value1 = this.testValueGenerator.GeneratePercentage();
        var value2 = this.testValueGenerator.GeneratePercentage();

        Assert.NotEqual(value1, value2);
    }

    [Fact]
    [Trait("Category", "Dynamic")]
    public void GenerateString()
    {
        var value1 = this.testValueGenerator.GenerateString(0);
        var value2 = this.testValueGenerator.GenerateString(0);

        Assert.Empty(value1);
        Assert.Empty(value2);
        Assert.Equal(value1, value2);

        value1 = this.testValueGenerator.GenerateString(1);
        value2 = this.testValueGenerator.GenerateString(1);

        Assert.Single(value1);
        Assert.Single(value2);
        Assert.NotEqual(value1, value2);

        value1 = this.testValueGenerator.GenerateString(100);
        value2 = this.testValueGenerator.GenerateString(100);

        Assert.Equal(100, value1.Count());
        Assert.Equal(100, value2.Count());
        Assert.NotEqual(value1, value2);
    }

    [Fact]
    [Trait("Category", "Dynamic")]
    public void GenerateUnique()
    {
        var value1 = this.testValueGenerator.GenerateUnique();
        var value2 = this.testValueGenerator.GenerateUnique();

        Assert.NotEqual(value1, value2);
    }
}
