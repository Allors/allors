// <copyright file="RangesFromTests.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Ranges.Long;

using System;
using System.Linq;
using Xunit;
using Range = Allors.Shared.Ranges.ValueRange<long>;

public class RangeImportTests
{
    [Fact]
    public void ImportEmpty()
    {
        var x = Range.Import(Array.Empty<long>());

        Assert.Equal(Array.Empty<long>(), x);
    }

    [Fact]
    public void ImportSingle()
    {
        var x = Range.Import([1L]);

        Assert.Equal(new[] { 1L }, x);
    }

    [Fact]
    public void ImportOrderedPair()
    {
        var x = Range.Import([1L, 2L]);

        Assert.Equal(new[] { 1L, 2L }, x);
    }

    [Fact]
    public void ImportUnorderedPair()
    {
        var x = Range.Import([2L, 1L]);

        Assert.Equal(new[] { 1L, 2L }, x);
    }

    [Fact]
    public void ImportDistinctIterator()
    {
        var distinctIterator = Array.Empty<long>().Distinct();

        var x = Range.Import(distinctIterator);

        Assert.True(x.IsEmpty);
        Assert.Null(x.Save());
    }
}
