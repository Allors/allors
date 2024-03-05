﻿// <copyright file="ExtentTest.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters;

using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using Allors.Database.Domain;
using Allors.Database.Meta;
using Xunit;

public enum Zero2Four
{
    Zero = 0,
    One = 1,
    Two = 2,
    Three = 3,
    Four = 4,
}

public abstract class ExtentTest : IDisposable
{
    protected static readonly bool[] TrueFalse = [true, false];

    protected abstract IProfile Profile { get; }

    protected ITransaction Transaction => this.Profile.Transaction;

    protected Action[] Markers => this.Profile.Markers;

    protected Action[] Inits => this.Profile.Inits;

    protected virtual bool[] UseOperator => [false, true];

    public abstract void Dispose();

    [Fact]
    public void AndGreaterThanLessThan()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddAnd(v =>
            {
                v.AddGreaterThan(m.C1.C1AllorsInteger, 0);
                v.AddLessThan(m.C1.C1AllorsInteger, 2);
            });

            Assert.Single(extent);
            Assert.False(extent.Contains(this.c1A));
            Assert.True(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.False(extent.Contains(this.c2B));
            Assert.False(extent.Contains(this.c2C));
            Assert.False(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            // Interface
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddGreaterThan(m.I12.I12AllorsInteger, 0);
            extent.Filter.AddLessThan(m.I12.I12AllorsInteger, 2);

            Assert.Equal(2, extent.Count);
            Assert.False(extent.Contains(this.c1A));
            Assert.True(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.True(extent.Contains(this.c2B));
            Assert.False(extent.Contains(this.c2C));
            Assert.False(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            // Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddGreaterThan(m.S1234.S1234AllorsInteger, 0);
            extent.Filter.AddLessThan(m.S1234.S1234AllorsInteger, 2);

            Assert.Equal(4, extent.Count);
            Assert.False(extent.Contains(this.c1A));
            Assert.True(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.True(extent.Contains(this.c2B));
            Assert.False(extent.Contains(this.c2C));
            Assert.False(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.True(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.True(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));
        }
    }

    [Fact]
    public void AndLessThan()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class
            var extent = this.Transaction.Extent(m.C1.Composite);
            var all = extent.Filter.AddAnd(v =>
            {
                v.AddLessThan(m.C1.C1AllorsInteger, 2);
            });

            Assert.Single(extent);
            Assert.False(extent.Contains(this.c1A));
            Assert.True(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.False(extent.Contains(this.c2B));
            Assert.False(extent.Contains(this.c2C));
            Assert.False(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            // Interface
            (extent = this.Transaction.Extent(m.I12.Composite)).Filter.AddAnd(v => v.AddLessThan(m.I12.I12AllorsInteger, 2));

            Assert.Equal(2, extent.Count);
            Assert.False(extent.Contains(this.c1A));
            Assert.True(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.True(extent.Contains(this.c2B));
            Assert.False(extent.Contains(this.c2C));
            Assert.False(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            // Super Interface
            (extent = this.Transaction.Extent(m.S1234.Composite)).Filter.AddAnd(v => v.AddLessThan(m.S1234.S1234AllorsInteger, 2));

            Assert.Equal(4, extent.Count);
            Assert.False(extent.Contains(this.c1A));
            Assert.True(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.True(extent.Contains(this.c2B));
            Assert.False(extent.Contains(this.c2C));
            Assert.False(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.True(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.True(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));
        }
    }

    [Fact]
    public void AssociationMany2ManyIn()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            foreach (var useEnumerable in TrueFalse)
            {
                foreach (var useOperator in this.UseOperator)
                {
                    // Empty
                    var inExtent = this.Transaction.Extent(m.C1.Composite);
                    inExtent.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                    if (useOperator)
                    {
                        var inExtentA = this.Transaction.Extent(m.C1.Composite);
                        inExtentA.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                        var inExtentB = this.Transaction.Extent(m.C1.Composite);
                        inExtentB.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                        inExtent = this.Transaction.Union(inExtentA, inExtentB);
                    }

                    var extent = this.Transaction.Extent(m.C2.Composite);
                    if (useEnumerable)
                    {
                        var enumerable = (IEnumerable<IObject>)(Extent<IObject>)inExtent;
                        extent.Filter.AddIn(m.C2.C1sWhereC1C2many2many, enumerable);
                    }
                    else
                    {
                        extent.Filter.AddIn(m.C2.C1sWhereC1C2many2many, inExtent);
                    }

                    Assert.Empty(extent);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full
                    inExtent = this.Transaction.Extent(m.C1.Composite);
                    if (useOperator)
                    {
                        var inExtentA = this.Transaction.Extent(m.C1.Composite);
                        var inExtentB = this.Transaction.Extent(m.C1.Composite);
                        inExtent = this.Transaction.Union(inExtentA, inExtentB);
                    }

                    extent = this.Transaction.Extent(m.C2.Composite);
                    if (useEnumerable)
                    {
                        var enumerable = (IEnumerable<IObject>)(Extent<IObject>)inExtent;
                        extent.Filter.AddIn(m.C2.C1sWhereC1C2many2many, enumerable);
                    }
                    else
                    {
                        extent.Filter.AddIn(m.C2.C1sWhereC1C2many2many, inExtent);
                    }

                    Assert.Equal(3, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, true, true, true);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Filtered
                    inExtent = this.Transaction.Extent(m.C1.Composite);
                    inExtent.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                    if (useOperator)
                    {
                        var inExtentA = this.Transaction.Extent(m.C1.Composite);
                        inExtentA.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                        var inExtentB = this.Transaction.Extent(m.C1.Composite);
                        inExtentB.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                        inExtent = this.Transaction.Union(inExtentA, inExtentB);
                    }

                    extent = this.Transaction.Extent(m.C2.Composite);
                    if (useEnumerable)
                    {
                        var enumerable = (IEnumerable<IObject>)(Extent<IObject>)inExtent;
                        extent.Filter.AddIn(m.C2.C1sWhereC1C2many2many, enumerable);
                    }
                    else
                    {
                        extent.Filter.AddIn(m.C2.C1sWhereC1C2many2many, inExtent);
                    }

                    Assert.Single(extent);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, true, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // In Extent over Interface
                    // Empty
                    inExtent = this.Transaction.Extent(m.I12.Composite);
                    inExtent.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    if (useOperator)
                    {
                        var inExtentA = this.Transaction.Extent(m.I12.Composite);
                        inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                        var inExtentB = this.Transaction.Extent(m.I12.Composite);
                        inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                        inExtent = this.Transaction.Union(inExtentA, inExtentB);
                    }

                    extent = this.Transaction.Extent(m.C2.Composite);
                    if (useEnumerable)
                    {
                        var enumerable = (IEnumerable<IObject>)(Extent<IObject>)inExtent;
                        extent.Filter.AddIn(m.C2.C1sWhereC1C2many2many, enumerable);
                    }
                    else
                    {
                        extent.Filter.AddIn(m.C2.C1sWhereC1C2many2many, inExtent);
                    }

                    Assert.Empty(extent);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full
                    inExtent = this.Transaction.Extent(m.I12.Composite);
                    if (useOperator)
                    {
                        var inExtentA = this.Transaction.Extent(m.I12.Composite);
                        var inExtentB = this.Transaction.Extent(m.I12.Composite);
                        inExtent = this.Transaction.Union(inExtentA, inExtentB);
                    }

                    extent = this.Transaction.Extent(m.C2.Composite);
                    if (useEnumerable)
                    {
                        var enumerable = (IEnumerable<IObject>)(Extent<IObject>)inExtent;
                        extent.Filter.AddIn(m.C2.C1sWhereC1C2many2many, enumerable);
                    }
                    else
                    {
                        extent.Filter.AddIn(m.C2.C1sWhereC1C2many2many, inExtent);
                    }

                    Assert.Equal(3, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, true, true, true);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Filtered
                    inExtent = this.Transaction.Extent(m.I12.Composite);
                    inExtent.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                    if (useOperator)
                    {
                        var inExtentA = this.Transaction.Extent(m.I12.Composite);
                        inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                        var inExtentB = this.Transaction.Extent(m.I12.Composite);
                        inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                        inExtent = this.Transaction.Union(inExtentA, inExtentB);
                    }

                    extent = this.Transaction.Extent(m.C2.Composite);
                    if (useEnumerable)
                    {
                        var enumerable = (IEnumerable<IObject>)(Extent<IObject>)inExtent;
                        extent.Filter.AddIn(m.C2.C1sWhereC1C2many2many, enumerable);
                    }
                    else
                    {
                        extent.Filter.AddIn(m.C2.C1sWhereC1C2many2many, inExtent);
                    }

                    Assert.Single(extent);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, true, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // RelationType from Class to Interface

                    // In Extent over Class
                    // Empty
                    inExtent = this.Transaction.Extent(m.C1.Composite);
                    inExtent.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                    if (useOperator)
                    {
                        var inExtentA = this.Transaction.Extent(m.C1.Composite);
                        inExtentA.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                        var inExtentB = this.Transaction.Extent(m.C1.Composite);
                        inExtentB.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                        inExtent = this.Transaction.Union(inExtentA, inExtentB);
                    }

                    extent = this.Transaction.Extent(m.C2.Composite);
                    if (useEnumerable)
                    {
                        var enumerable = (IEnumerable<IObject>)(Extent<IObject>)inExtent;
                        extent.Filter.AddIn(m.I12.C1sWhereC1I12many2many, enumerable);
                    }
                    else
                    {
                        extent.Filter.AddIn(m.I12.C1sWhereC1I12many2many, inExtent);
                    }

                    Assert.Empty(extent);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full
                    inExtent = this.Transaction.Extent(m.C1.Composite);
                    if (useOperator)
                    {
                        var inExtentA = this.Transaction.Extent(m.C1.Composite);
                        var inExtentB = this.Transaction.Extent(m.C1.Composite);
                        inExtent = this.Transaction.Union(inExtentA, inExtentB);
                    }

                    extent = this.Transaction.Extent(m.C2.Composite);
                    if (useEnumerable)
                    {
                        var enumerable = (IEnumerable<IObject>)(Extent<IObject>)inExtent;
                        extent.Filter.AddIn(m.I12.C1sWhereC1I12many2many, enumerable);
                    }
                    else
                    {
                        extent.Filter.AddIn(m.I12.C1sWhereC1I12many2many, inExtent);
                    }

                    Assert.Equal(3, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, true, true, true);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Filtered
                    inExtent = this.Transaction.Extent(m.C1.Composite);
                    inExtent.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                    if (useOperator)
                    {
                        var inExtentA = this.Transaction.Extent(m.C1.Composite);
                        inExtentA.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                        var inExtentB = this.Transaction.Extent(m.C1.Composite);
                        inExtentB.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                        inExtent = this.Transaction.Union(inExtentA, inExtentB);
                    }

                    extent = this.Transaction.Extent(m.C2.Composite);
                    if (useEnumerable)
                    {
                        var enumerable = (IEnumerable<IObject>)(Extent<IObject>)inExtent;
                        extent.Filter.AddIn(m.I12.C1sWhereC1I12many2many, enumerable);
                    }
                    else
                    {
                        extent.Filter.AddIn(m.I12.C1sWhereC1I12many2many, inExtent);
                    }

                    Assert.Single(extent);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, true, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // In Extent over Interface
                    // Empty
                    inExtent = this.Transaction.Extent(m.I12.Composite);
                    inExtent.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    if (useOperator)
                    {
                        var inExtentA = this.Transaction.Extent(m.I12.Composite);
                        inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                        var inExtentB = this.Transaction.Extent(m.I12.Composite);
                        inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                        inExtent = this.Transaction.Union(inExtentA, inExtentB);
                    }

                    extent = this.Transaction.Extent(m.C2.Composite);
                    if (useEnumerable)
                    {
                        var enumerable = (IEnumerable<IObject>)(Extent<IObject>)inExtent;
                        extent.Filter.AddIn(m.I12.C1sWhereC1I12many2many, enumerable);
                    }
                    else
                    {
                        extent.Filter.AddIn(m.I12.C1sWhereC1I12many2many, inExtent);
                    }

                    Assert.Empty(extent);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full
                    inExtent = this.Transaction.Extent(m.I12.Composite);
                    if (useOperator)
                    {
                        var inExtentA = this.Transaction.Extent(m.I12.Composite);
                        var inExtentB = this.Transaction.Extent(m.I12.Composite);
                        inExtent = this.Transaction.Union(inExtentA, inExtentB);
                    }

                    extent = this.Transaction.Extent(m.C2.Composite);
                    if (useEnumerable)
                    {
                        var enumerable = (IEnumerable<IObject>)(Extent<IObject>)inExtent;
                        extent.Filter.AddIn(m.I12.C1sWhereC1I12many2many, enumerable);
                    }
                    else
                    {
                        extent.Filter.AddIn(m.I12.C1sWhereC1I12many2many, inExtent);
                    }

                    Assert.Equal(3, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, true, true, true);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Filtered
                    inExtent = this.Transaction.Extent(m.I12.Composite);
                    inExtent.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                    if (useOperator)
                    {
                        var inExtentA = this.Transaction.Extent(m.I12.Composite);
                        inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                        var inExtentB = this.Transaction.Extent(m.I12.Composite);
                        inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                        inExtent = this.Transaction.Union(inExtentA, inExtentB);
                    }

                    extent = this.Transaction.Extent(m.C2.Composite);
                    if (useEnumerable)
                    {
                        var enumerable = (IEnumerable<IObject>)(Extent<IObject>)inExtent;
                        extent.Filter.AddIn(m.I12.C1sWhereC1I12many2many, enumerable);
                    }
                    else
                    {
                        extent.Filter.AddIn(m.I12.C1sWhereC1I12many2many, inExtent);
                    }

                    Assert.Single(extent);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, true, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // In Extent over Disjoint Interfaces
                    // Empty
                    inExtent = this.Transaction.Extent(m.I12.Composite);
                    inExtent.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    if (useOperator)
                    {
                        var inExtentA = this.Transaction.Extent(m.I12.Composite);
                        inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                        var inExtentB = this.Transaction.Extent(m.I12.Composite);
                        inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                        inExtent = this.Transaction.Union(inExtentA, inExtentB);
                    }

                    extent = this.Transaction.Extent(m.I34.Composite);
                    if (useEnumerable)
                    {
                        extent.Filter.AddIn(m.I34.I12sWhereI12I34many2many, (IEnumerable<IObject>)(Extent<IObject>)inExtent);
                    }
                    else
                    {
                        extent.Filter.AddIn(m.I34.I12sWhereI12I34many2many, inExtent);
                    }

                    Assert.Empty(extent);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, false, false, false);
                    this.AssertC4(extent, false, false, false, false);

                    // Full
                    inExtent = this.Transaction.Extent(m.I12.Composite);
                    if (useOperator)
                    {
                        var inExtentA = this.Transaction.Extent(m.I12.Composite);
                        var inExtentB = this.Transaction.Extent(m.I12.Composite);
                        inExtent = this.Transaction.Union(inExtentA, inExtentB);
                    }

                    extent = this.Transaction.Extent(m.I34.Composite);
                    if (useEnumerable)
                    {
                        var enumerable = (IEnumerable<IObject>)(Extent<IObject>)inExtent;
                        extent.Filter.AddIn(m.I34.I12sWhereI12I34many2many, enumerable);
                    }
                    else
                    {
                        extent.Filter.AddIn(m.I34.I12sWhereI12I34many2many, inExtent);
                    }

                    Assert.Equal(7, extent.Count);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, true, true, true);
                    this.AssertC4(extent, true, true, true, true);

                    // Filtered
                    inExtent = this.Transaction.Extent(m.I12.Composite);
                    inExtent.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                    if (useOperator)
                    {
                        var inExtentA = this.Transaction.Extent(m.I12.Composite);
                        inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                        var inExtentB = this.Transaction.Extent(m.I12.Composite);
                        inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                        inExtent = this.Transaction.Union(inExtentA, inExtentB);
                    }

                    extent = this.Transaction.Extent(m.I34.Composite);
                    if (useEnumerable)
                    {
                        var enumerable = (IEnumerable<IObject>)(Extent<IObject>)inExtent;
                        extent.Filter.AddIn(m.I34.I12sWhereI12I34many2many, enumerable);
                    }
                    else
                    {
                        extent.Filter.AddIn(m.I34.I12sWhereI12I34many2many, inExtent);
                    }

                    Assert.Single(extent);
                    this.AssertC1(extent, false, false, false, false);
                    this.AssertC2(extent, false, false, false, false);
                    this.AssertC3(extent, false, true, false, false);
                    this.AssertC4(extent, false, false, false, false);
                }
            }
        }
    }

    [Fact]
    public void AssociationMany2ManyContains()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class
            var extent = this.Transaction.Extent(m.C2.Composite);
            extent.Filter.AddContains(m.C2.C1sWhereC1C2many2many, this.c1C);

            Assert.Equal(2, extent.Count);
            Assert.False(extent.Contains(this.c1A));
            Assert.False(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.True(extent.Contains(this.c2B));
            Assert.True(extent.Contains(this.c2C));
            Assert.False(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            extent = this.Transaction.Extent(m.C2.Composite);
            extent.Filter.AddContains(m.C2.C1sWhereC1C2many2many, this.c1C);
            extent.Filter.AddContains(m.C2.C1sWhereC1C2many2many, this.c1D);

            Assert.Equal(2, extent.Count);
            Assert.False(extent.Contains(this.c1A));
            Assert.False(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.True(extent.Contains(this.c2B));
            Assert.True(extent.Contains(this.c2C));
            Assert.False(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            // Interface
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddContains(m.I12.C1sWhereC1I12many2many, this.c1C);

            Assert.Equal(2, extent.Count);
            Assert.False(extent.Contains(this.c1A));
            Assert.False(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.True(extent.Contains(this.c2B));
            Assert.True(extent.Contains(this.c2C));
            Assert.False(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            // Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddContains(m.S1234.S1234sWhereS1234many2many, this.c1B);

            Assert.Equal(2, extent.Count);
            Assert.True(extent.Contains(this.c1A));
            Assert.True(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.False(extent.Contains(this.c2B));
            Assert.False(extent.Contains(this.c2C));
            Assert.False(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));
        }
    }

    [Fact]
    public void AssociationMany2ManyExist()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class
            var extent = this.Transaction.Extent(m.C2.Composite);
            extent.Filter.AddExists(m.C2.C1sWhereC1C2many2many);

            Assert.Equal(3, extent.Count);
            Assert.False(extent.Contains(this.c1A));
            Assert.False(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.True(extent.Contains(this.c2B));
            Assert.True(extent.Contains(this.c2C));
            Assert.True(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            // Interface
            extent = this.Transaction.Extent(m.I2.Composite);
            extent.Filter.AddExists(m.I2.I1sWhereI1I2many2many);

            Assert.Equal(3, extent.Count);
            Assert.False(extent.Contains(this.c1A));
            Assert.False(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.True(extent.Contains(this.c2B));
            Assert.True(extent.Contains(this.c2C));
            Assert.True(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            // Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddExists(m.S1234.S1234sWhereS1234many2many);

            Assert.Equal(10, extent.Count);
            Assert.True(extent.Contains(this.c1A));
            Assert.True(extent.Contains(this.c1B));
            Assert.True(extent.Contains(this.c1C));
            Assert.True(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.True(extent.Contains(this.c2B));
            Assert.True(extent.Contains(this.c2C));
            Assert.True(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.True(extent.Contains(this.c3B));
            Assert.True(extent.Contains(this.c3C));
            Assert.True(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            // Class - Wrong RelationType
            extent = this.Transaction.Extent(m.C2.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddExists(m.C1.C1sWhereC1C1many2many);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.I12.Composite);

            exception = false;
            try
            {
                extent.Filter.AddExists(m.I34.I12sWhereI12I34many2many);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.S12.Composite);

            exception = false;
            try
            {
                extent.Filter.AddExists(m.S1234.S1234sWhereS1234many2many);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void AssociationMany2OneIn()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            this.Transaction.Commit();

            foreach (var useOperator in this.UseOperator)
            {
                // Extent over Class

                // RelationType from Class to Class

                // In Extent over Class
                // Empty
                var inExtent = this.Transaction.Extent(m.C1.Composite);
                inExtent.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    inExtentA.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtentB.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                var extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddIn(m.C2.C1sWhereC1C2many2one, inExtent);

                Assert.Empty(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full
                inExtent = this.Transaction.Extent(m.C1.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddIn(m.C2.C1sWhereC1C2many2one, inExtent);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, true, true, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Filtered
                inExtent = this.Transaction.Extent(m.C1.Composite);
                inExtent.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    inExtentA.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtentB.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddIn(m.C2.C1sWhereC1C2many2one, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // In Extent over Interface
                // Empty
                inExtent = this.Transaction.Extent(m.I12.Composite);
                inExtent.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddIn(m.C2.C1sWhereC1C2many2one, inExtent);

                Assert.Empty(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full
                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddIn(m.C2.C1sWhereC1C2many2one, inExtent);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, true, true, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Filtered
                inExtent = this.Transaction.Extent(m.I12.Composite);
                inExtent.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddIn(m.C2.C1sWhereC1C2many2one, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // RelationType from Class to Interface

                // In Extent over Class
                // Empty
                inExtent = this.Transaction.Extent(m.C1.Composite);
                inExtent.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    inExtentA.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtentB.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddIn(m.I12.C1sWhereC1I12many2one, inExtent);

                Assert.Empty(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full
                inExtent = this.Transaction.Extent(m.C1.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddIn(m.I12.C1sWhereC1I12many2one, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, true, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Filtered
                inExtent = this.Transaction.Extent(m.C1.Composite);
                inExtent.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    inExtentA.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtentB.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddIn(m.I12.C1sWhereC1I12many2one, inExtent);

                Assert.Empty(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // In Extent over Interface
                // Empty
                inExtent = this.Transaction.Extent(m.I12.Composite);
                inExtent.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddIn(m.I12.C1sWhereC1I12many2one, inExtent);

                Assert.Empty(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full
                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddIn(m.I12.C1sWhereC1I12many2one, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, true, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Filtered
                inExtent = this.Transaction.Extent(m.I12.Composite);
                inExtent.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddIn(m.I12.C1sWhereC1I12many2one, inExtent);

                Assert.Empty(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // RelationType from Interface to Interface

                // In Extent over Interface
                // Empty
                inExtent = this.Transaction.Extent(m.I12.Composite);
                inExtent.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.I34.Composite);
                extent.Filter.AddIn(m.I34.I12sWhereI12I34many2one, inExtent);

                Assert.Empty(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full
                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.I34.Composite);
                extent.Filter.AddIn(m.I34.I12sWhereI12I34many2one, inExtent);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, true, false, false);
                this.AssertC4(extent, false, true, false, false);

                // Filtered
                inExtent = this.Transaction.Extent(m.I12.Composite);
                inExtent.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.I34.Composite);
                extent.Filter.AddIn(m.I34.I12sWhereI12I34many2one, inExtent);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, true, false, false);
                this.AssertC4(extent, false, true, false, false);
            }
        }
    }

    [Fact]
    public void AssociationMany2OneContains()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddContains(m.C1.C1sWhereC1C1many2one, this.c1C);

            Assert.Single(extent);
            Assert.False(extent.Contains(this.c1A));
            Assert.False(extent.Contains(this.c1B));
            Assert.True(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.False(extent.Contains(this.c2B));
            Assert.False(extent.Contains(this.c2C));
            Assert.False(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            extent = this.Transaction.Extent(m.C2.Composite);
            extent.Filter.AddContains(m.C2.C1sWhereC1C2many2one, this.c1C);

            Assert.Single(extent);
            Assert.False(extent.Contains(this.c1A));
            Assert.False(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.False(extent.Contains(this.c2B));
            Assert.True(extent.Contains(this.c2C));
            Assert.False(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            extent = this.Transaction.Extent(m.C4.Composite);
            extent.Filter.AddContains(m.C4.C3sWhereC3C4many2one, this.c3C);

            Assert.Single(extent);
            Assert.False(extent.Contains(this.c1A));
            Assert.False(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.False(extent.Contains(this.c2B));
            Assert.False(extent.Contains(this.c2C));
            Assert.False(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.True(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            // Interface
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddContains(m.I12.C1sWhereC1I12many2one, this.c1C);

            Assert.Single(extent);
            Assert.False(extent.Contains(this.c1A));
            Assert.False(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.False(extent.Contains(this.c2B));
            Assert.True(extent.Contains(this.c2C));
            Assert.False(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            // TODO: wrong relation
        }
    }

    [Fact]
    public void AssociationOne2ManyIn()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            foreach (var useOperator in this.UseOperator)
            {
                // Extent over Class

                // RelationType from Class to Class

                // In Extent over Class
                // Empty
                var inExtent = this.Transaction.Extent(m.C1.Composite);
                inExtent.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    inExtentA.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtentB.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                var extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddIn(m.C2.C1WhereC1C2one2many, inExtent);

                Assert.Empty(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full
                inExtent = this.Transaction.Extent(m.C1.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddIn(m.C2.C1WhereC1C2one2many, inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Filtered
                inExtent = this.Transaction.Extent(m.C1.Composite);
                inExtent.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    inExtentA.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtentB.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddIn(m.C2.C1WhereC1C2one2many, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // In Extent over Interface
                // Empty
                inExtent = this.Transaction.Extent(m.I12.Composite);
                inExtent.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddIn(m.C2.C1WhereC1C2one2many, inExtent);

                Assert.Empty(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full
                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddIn(m.C2.C1WhereC1C2one2many, inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Filtered
                inExtent = this.Transaction.Extent(m.I12.Composite);
                inExtent.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddIn(m.C2.C1WhereC1C2one2many, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, true, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // RelationType from Class to Interface

                // In Extent over Class
                // Empty
                inExtent = this.Transaction.Extent(m.C1.Composite);
                inExtent.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    inExtentA.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtentB.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddIn(m.I12.C1WhereC1I12one2many, inExtent);

                Assert.Empty(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full
                inExtent = this.Transaction.Extent(m.C1.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddIn(m.I12.C1WhereC1I12one2many, inExtent);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Filtered
                inExtent = this.Transaction.Extent(m.C1.Composite);
                inExtent.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    inExtentA.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtentB.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddIn(m.I12.C1WhereC1I12one2many, inExtent);

                Assert.Empty(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // In Extent over Interface
                // Empty
                inExtent = this.Transaction.Extent(m.I12.Composite);
                inExtent.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddIn(m.I12.C1WhereC1I12one2many, inExtent);

                Assert.Empty(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full
                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddIn(m.I12.C1WhereC1I12one2many, inExtent);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Filtered
                inExtent = this.Transaction.Extent(m.I12.Composite);
                inExtent.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddIn(m.I12.C1WhereC1I12one2many, inExtent);

                Assert.Empty(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);
            }
        }
    }

    [Fact]
    public void AssociationOne2ManyEquals()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class
            var extent = this.Transaction.Extent(m.C2.Composite);
            extent.Filter.AddEquals(m.C2.C1WhereC1C2one2many, this.c1B);

            Assert.Single(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.C2.Composite);
            extent.Filter.AddEquals(m.C2.C1WhereC1C2one2many, this.c1C);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface
            extent = this.Transaction.Extent(m.I2.Composite);
            extent.Filter.AddEquals(m.I2.I1WhereI1I2one2many, this.c1B);

            Assert.Single(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.I2.Composite);
            extent.Filter.AddEquals(m.I2.I1WhereI1I2one2many, this.c1C);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddEquals(m.S1234.S1234WhereS1234one2many, this.c1B);

            Assert.Single(extent);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddEquals(m.S1234.S1234WhereS1234one2many, this.c3C);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Class - Wrong RelationType
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddEquals(m.C2.C3WhereC3C2one2many, this.c2A);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.I12.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.C2.C3WhereC3C2one2many, this.c2A);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.S12.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.C2.C3WhereC3C2one2many, this.c2A);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void AssociationOne2ManyExist()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class
            var extent = this.Transaction.Extent(m.C2.Composite);
            extent.Filter.AddExists(m.C2.C1WhereC1C2one2many);

            Assert.Equal(3, extent.Count);
            Assert.False(extent.Contains(this.c1A));
            Assert.False(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.True(extent.Contains(this.c2B));
            Assert.True(extent.Contains(this.c2C));
            Assert.True(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            // Interface
            extent = this.Transaction.Extent(m.I2.Composite);
            extent.Filter.AddExists(m.I2.I1WhereI1I2one2many);

            Assert.Equal(3, extent.Count);
            Assert.False(extent.Contains(this.c1A));
            Assert.False(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.True(extent.Contains(this.c2B));
            Assert.True(extent.Contains(this.c2C));
            Assert.True(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            // Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddExists(m.S1234.S1234WhereS1234one2many);

            Assert.Equal(3, extent.Count);
            Assert.False(extent.Contains(this.c1A));
            Assert.True(extent.Contains(this.c1B));
            Assert.True(extent.Contains(this.c1C));
            Assert.True(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.False(extent.Contains(this.c2B));
            Assert.False(extent.Contains(this.c2C));
            Assert.False(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            // Class - Wrong RelationType
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddExists(m.C2.C3WhereC3C2one2many);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.I12.Composite);

            exception = false;
            try
            {
                extent.Filter.AddExists(m.C2.C3WhereC3C2one2many);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.S12.Composite);

            exception = false;
            try
            {
                extent.Filter.AddExists(m.C2.C3WhereC3C2one2many);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void AssociationOne2ManyInstanceof()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class
            var extent = this.Transaction.Extent(m.C2.Composite);
            extent.Filter.AddInstanceOf(m.C2.C1WhereC1C2one2many, m.C1.Composite);

            Assert.Equal(3, extent.Count);
            Assert.False(extent.Contains(this.c1A));
            Assert.False(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.True(extent.Contains(this.c2B));
            Assert.True(extent.Contains(this.c2C));
            Assert.True(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            // Interface
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddInstanceOf(m.I12.C1WhereC1I12one2many, m.C1.Composite);

            Assert.Equal(3, extent.Count);
            Assert.False(extent.Contains(this.c1A));
            Assert.True(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.False(extent.Contains(this.c2B));
            Assert.True(extent.Contains(this.c2C));
            Assert.True(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            // Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddInstanceOf(m.S1234.S1234WhereS1234one2many, m.C1.Composite);

            Assert.Single(extent);
            Assert.False(extent.Contains(this.c1A));
            Assert.True(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.False(extent.Contains(this.c2B));
            Assert.False(extent.Contains(this.c2C));
            Assert.False(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            // TODO: wrong relation
        }
    }

    [Fact]
    public void AssociationOne2OneIn()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            foreach (var useOperator in this.UseOperator)
            {
                // Extent over Class
                // RelationType from Class to Class

                // In Extent over Class
                var inExtent = this.Transaction.Extent(m.C1.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                var extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddIn(m.C1.C1WhereC1C1one2one, inExtent);

                Assert.Equal(3, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.True(extent.Contains(this.c1C));
                Assert.True(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddIn(m.C2.C1WhereC1C2one2one, inExtent);

                Assert.Equal(3, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.True(extent.Contains(this.c2C));
                Assert.True(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                inExtent = this.Transaction.Extent(m.C3.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C3.Composite);
                    var inExtentB = this.Transaction.Extent(m.C3.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C4.Composite);
                extent.Filter.AddIn(m.C4.C3WhereC3C4one2one, inExtent);

                Assert.Equal(3, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.True(extent.Contains(this.c4B));
                Assert.True(extent.Contains(this.c4C));
                Assert.True(extent.Contains(this.c4D));

                // In Extent over Interface
                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddIn(m.C1.C1WhereC1C1one2one, inExtent);

                Assert.Equal(3, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.True(extent.Contains(this.c1C));
                Assert.True(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddIn(m.C2.C1WhereC1C2one2one, inExtent);

                Assert.Equal(3, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.True(extent.Contains(this.c2C));
                Assert.True(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                inExtent = this.Transaction.Extent(m.I34.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I34.Composite);
                    var inExtentB = this.Transaction.Extent(m.I34.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C4.Composite);
                extent.Filter.AddIn(m.C4.C3WhereC3C4one2one, inExtent);

                Assert.Equal(3, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.True(extent.Contains(this.c4B));
                Assert.True(extent.Contains(this.c4C));
                Assert.True(extent.Contains(this.c4D));

                // RelationType from Interface to Class

                // In Extent over Class
                inExtent = this.Transaction.Extent(m.C1.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddIn(m.C2.I12WhereI12C2one2one, inExtent);

                Assert.Equal(3, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.True(extent.Contains(this.c2C));
                Assert.True(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // In Extent over Interface
                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddIn(m.C2.I12WhereI12C2one2one, inExtent);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);
            }
        }
    }

    [Fact]
    public void AssociationOne2OneEquals()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddEquals(m.C1.C1WhereC1C1one2one, this.c1B);

            Assert.Single(extent);
            Assert.False(extent.Contains(this.c1A));
            Assert.True(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.False(extent.Contains(this.c2B));
            Assert.False(extent.Contains(this.c2C));
            Assert.False(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            extent = this.Transaction.Extent(m.C2.Composite);
            extent.Filter.AddEquals(m.C2.C1WhereC1C2one2one, this.c1B);

            Assert.Single(extent);
            Assert.False(extent.Contains(this.c1A));
            Assert.False(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.True(extent.Contains(this.c2B));
            Assert.False(extent.Contains(this.c2C));
            Assert.False(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            extent = this.Transaction.Extent(m.C4.Composite);
            extent.Filter.AddEquals(m.C4.C3WhereC3C4one2one, this.c3B);

            Assert.Single(extent);
            Assert.False(extent.Contains(this.c1A));
            Assert.False(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.False(extent.Contains(this.c2B));
            Assert.False(extent.Contains(this.c2C));
            Assert.False(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.True(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            // Interface
            extent = this.Transaction.Extent(m.I2.Composite);
            extent.Filter.AddEquals(m.I2.I1WhereI1I2one2one, this.c1B);

            Assert.Single(extent);
            Assert.False(extent.Contains(this.c1A));
            Assert.False(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.True(extent.Contains(this.c2B));
            Assert.False(extent.Contains(this.c2C));
            Assert.False(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            // Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddEquals(m.S1234.S1234WhereS1234one2one, this.c1C);

            Assert.Single(extent);
            Assert.False(extent.Contains(this.c1A));
            Assert.False(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.True(extent.Contains(this.c2B));
            Assert.False(extent.Contains(this.c2C));
            Assert.False(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            // Class - Wrong RelationType
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddEquals(m.C2.C3WhereC3C2one2one, this.c2A);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.I12.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.C2.C3WhereC3C2one2one, this.c2A);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.S12.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.C2.C3WhereC3C2one2one, this.c2A);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void AssociationOne2OneExist()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddExists(m.C1.C1WhereC1C1one2one);

            Assert.Equal(3, extent.Count);
            Assert.False(extent.Contains(this.c1A));
            Assert.True(extent.Contains(this.c1B));
            Assert.True(extent.Contains(this.c1C));
            Assert.True(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.False(extent.Contains(this.c2B));
            Assert.False(extent.Contains(this.c2C));
            Assert.False(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            extent = this.Transaction.Extent(m.C2.Composite);
            extent.Filter.AddExists(m.C2.C1WhereC1C2one2one);

            Assert.Equal(3, extent.Count);
            Assert.False(extent.Contains(this.c1A));
            Assert.False(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.True(extent.Contains(this.c2B));
            Assert.True(extent.Contains(this.c2C));
            Assert.True(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            extent = this.Transaction.Extent(m.C4.Composite);
            extent.Filter.AddExists(m.C4.C3WhereC3C4one2one);

            Assert.Equal(3, extent.Count);
            Assert.False(extent.Contains(this.c1A));
            Assert.False(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.False(extent.Contains(this.c2B));
            Assert.False(extent.Contains(this.c2C));
            Assert.False(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.True(extent.Contains(this.c4B));
            Assert.True(extent.Contains(this.c4C));
            Assert.True(extent.Contains(this.c4D));

            // Interface
            extent = this.Transaction.Extent(m.I2.Composite);
            extent.Filter.AddExists(m.I2.I1WhereI1I2one2one);

            Assert.Equal(3, extent.Count);
            Assert.False(extent.Contains(this.c1A));
            Assert.False(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.True(extent.Contains(this.c2B));
            Assert.True(extent.Contains(this.c2C));
            Assert.True(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            // Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddExists(m.S1234.S1234WhereS1234one2one);

            Assert.Equal(9, extent.Count);
            Assert.False(extent.Contains(this.c1A));
            Assert.True(extent.Contains(this.c1B));
            Assert.True(extent.Contains(this.c1C));
            Assert.True(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.True(extent.Contains(this.c2B));
            Assert.True(extent.Contains(this.c2C));
            Assert.True(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.True(extent.Contains(this.c3B));
            Assert.True(extent.Contains(this.c3C));
            Assert.True(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            // Class - Wrong RelationType
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddExists(m.C2.C3WhereC3C2one2one);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.I12.Composite);

            exception = false;
            try
            {
                extent.Filter.AddExists(m.C2.C3WhereC3C2one2one);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.S12.Composite);

            exception = false;
            try
            {
                extent.Filter.AddExists(m.C2.C3WhereC3C2one2one);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void AssociationOne2OneInstanceof()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class

            // Class
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddInstanceOf(m.C1.C1WhereC1C1one2one, m.C1.Composite);

            Assert.Equal(3, extent.Count);
            Assert.False(extent.Contains(this.c1A));
            Assert.True(extent.Contains(this.c1B));
            Assert.True(extent.Contains(this.c1C));
            Assert.True(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.False(extent.Contains(this.c2B));
            Assert.False(extent.Contains(this.c2C));
            Assert.False(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            extent = this.Transaction.Extent(m.C2.Composite);
            extent.Filter.AddInstanceOf(m.C2.C1WhereC1C2one2one, m.C1.Composite);

            Assert.Equal(3, extent.Count);
            Assert.False(extent.Contains(this.c1A));
            Assert.False(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.True(extent.Contains(this.c2B));
            Assert.True(extent.Contains(this.c2C));
            Assert.True(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            extent = this.Transaction.Extent(m.C4.Composite);
            extent.Filter.AddInstanceOf(m.C4.C3WhereC3C4one2one, m.C3.Composite);

            Assert.Equal(3, extent.Count);
            Assert.False(extent.Contains(this.c1A));
            Assert.False(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.False(extent.Contains(this.c2B));
            Assert.False(extent.Contains(this.c2C));
            Assert.False(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.True(extent.Contains(this.c4B));
            Assert.True(extent.Contains(this.c4C));
            Assert.True(extent.Contains(this.c4D));

            // Interface
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddInstanceOf(m.I12.C1WhereC1I12one2one, m.C1.Composite);

            Assert.Equal(3, extent.Count);
            Assert.False(extent.Contains(this.c1A));
            Assert.True(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.True(extent.Contains(this.c2B));
            Assert.True(extent.Contains(this.c2C));
            Assert.False(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            // Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddInstanceOf(m.S1234.S1234WhereS1234one2one, m.C1.Composite);

            Assert.Equal(3, extent.Count);
            Assert.False(extent.Contains(this.c1A));
            Assert.True(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.True(extent.Contains(this.c2B));
            Assert.False(extent.Contains(this.c2C));
            Assert.False(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.True(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            // Interface

            // Class
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddInstanceOf(m.C1.C1WhereC1C1one2one, m.I1.Composite);

            Assert.Equal(3, extent.Count);
            Assert.False(extent.Contains(this.c1A));
            Assert.True(extent.Contains(this.c1B));
            Assert.True(extent.Contains(this.c1C));
            Assert.True(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.False(extent.Contains(this.c2B));
            Assert.False(extent.Contains(this.c2C));
            Assert.False(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            extent = this.Transaction.Extent(m.C2.Composite);
            extent.Filter.AddInstanceOf(m.C2.C1WhereC1C2one2one, m.I1.Composite);

            Assert.Equal(3, extent.Count);
            Assert.False(extent.Contains(this.c1A));
            Assert.False(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.True(extent.Contains(this.c2B));
            Assert.True(extent.Contains(this.c2C));
            Assert.True(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            extent = this.Transaction.Extent(m.C4.Composite);
            extent.Filter.AddInstanceOf(m.C4.C3WhereC3C4one2one, m.I3.Composite);

            Assert.Equal(3, extent.Count);
            Assert.False(extent.Contains(this.c1A));
            Assert.False(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.False(extent.Contains(this.c2B));
            Assert.False(extent.Contains(this.c2C));
            Assert.False(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.True(extent.Contains(this.c4B));
            Assert.True(extent.Contains(this.c4C));
            Assert.True(extent.Contains(this.c4D));

            // Interface
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddInstanceOf(m.I12.C1WhereC1I12one2one, m.I1.Composite);

            Assert.Equal(3, extent.Count);
            Assert.False(extent.Contains(this.c1A));
            Assert.True(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.True(extent.Contains(this.c2B));
            Assert.True(extent.Contains(this.c2C));
            Assert.False(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            // Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddInstanceOf(m.S1234.S1234WhereS1234one2one, m.S1234.Composite);

            Assert.Equal(9, extent.Count);
            Assert.False(extent.Contains(this.c1A));
            Assert.True(extent.Contains(this.c1B));
            Assert.True(extent.Contains(this.c1C));
            Assert.True(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.True(extent.Contains(this.c2B));
            Assert.True(extent.Contains(this.c2C));
            Assert.True(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.True(extent.Contains(this.c3B));
            Assert.True(extent.Contains(this.c3C));
            Assert.True(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            // TODO: wrong relation
        }
    }

    [Fact]
    public virtual void Combination()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Like and any
            var extent = this.Transaction.Extent(m.C1.Composite);

            extent.Filter.AddLike(m.C1.C1AllorsString, "%nada%");

            var any1 = extent.Filter.AddOr();
            any1.AddGreaterThan(m.C1.C1AllorsInteger, 0);
            any1.AddLessThan(m.C1.C1AllorsInteger, 3);

            var any2 = extent.Filter.AddOr();
            any2.AddGreaterThan(m.C1.C1AllorsInteger, 0);
            any2.AddLessThan(m.C1.C1AllorsInteger, 3);

            extent.ToArray(typeof(C1));

            // Role + Value for Shared Interface
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddExists(m.C1.C1C1one2manies);

            extent.Filter.AddExists(m.I12.I12AllorsInteger);
            extent.Filter.AddNot().AddExists(m.I12.I12AllorsInteger);
            extent.Filter.AddEquals(m.I12.I12AllorsInteger, 0);
            extent.Filter.AddLessThan(m.I12.I12AllorsInteger, 0);
            extent.Filter.AddGreaterThan(m.I12.I12AllorsInteger, 0);
            extent.Filter.AddBetween(m.I12.I12AllorsInteger, 0, 1);

            Assert.Empty(extent);
            Assert.False(extent.Contains(this.c1A));
            Assert.False(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.False(extent.Contains(this.c2B));
            Assert.False(extent.Contains(this.c2C));
            Assert.False(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            // Role In + Except
            var firstExtent = this.Transaction.Extent(m.C2.Composite);
            firstExtent.Filter.AddLike(m.I12.I12AllorsString, "ᴀbra%");

            var secondExtent = this.Transaction.Extent(m.C2.Composite);
            secondExtent.Filter.AddLike(m.I12.I12AllorsString, "ᴀbracadabra");

            var inExtent = this.Transaction.Except(firstExtent, secondExtent);

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddIn(m.C1.C1C2one2manies, inExtent);

            Assert.Single(extent);
            Assert.False(extent.Contains(this.c1A));
            Assert.True(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.False(extent.Contains(this.c2B));
            Assert.False(extent.Contains(this.c2C));
            Assert.False(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            // AssociationType In + Except
            firstExtent = this.Transaction.Extent(m.C1.Composite);
            firstExtent.Filter.AddLike(m.I12.I12AllorsString, "ᴀbra%");

            secondExtent = this.Transaction.Extent(m.C1.Composite);
            secondExtent.Filter.AddLike(m.I12.I12AllorsString, "ᴀbracadabra");

            inExtent = this.Transaction.Except(firstExtent, secondExtent);

            extent = this.Transaction.Extent(m.C2.Composite);
            extent.Filter.AddIn(m.C2.C1WhereC1C2one2many, inExtent);

            Assert.Single(extent);
            Assert.False(extent.Contains(this.c1A));
            Assert.False(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.True(extent.Contains(this.c2B));
            Assert.False(extent.Contains(this.c2C));
            Assert.False(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));
        }
    }

    [Fact]
    public virtual void CombinationWithMultipleOperations()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Except + Union
            var firstExtent = this.Transaction.Extent(m.C1.Composite);
            firstExtent.Filter.AddNot().AddExists(m.C1.C1AllorsString);

            var secondExtent = this.Transaction.Extent(m.C1.Composite);
            secondExtent.Filter.AddLike(m.C1.C1AllorsString, "ᴀbracadabra");

            var unionExtent = this.Transaction.Union(firstExtent, secondExtent);
            var topExtent = this.Transaction.Extent(m.C1.Composite);

            var extent = this.Transaction.Except(topExtent, unionExtent);

            Assert.Single(extent);
            Assert.False(extent.Contains(this.c1A));
            Assert.True(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.False(extent.Contains(this.c2B));
            Assert.False(extent.Contains(this.c2C));
            Assert.False(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            // Except + Intersect
            firstExtent = this.Transaction.Extent(m.C1.Composite);
            firstExtent.Filter.AddExists(m.C1.C1AllorsString);

            secondExtent = this.Transaction.Extent(m.C1.Composite);
            secondExtent.Filter.AddLike(m.C1.C1AllorsString, "ᴀbracadabra");

            var intersectExtent = this.Transaction.Intersect(firstExtent, secondExtent);
            topExtent = this.Transaction.Extent(m.C1.Composite);

            extent = this.Transaction.Except(topExtent, intersectExtent);

            Assert.Equal(2, extent.Count);
            Assert.True(extent.Contains(this.c1A));
            Assert.True(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.False(extent.Contains(this.c2B));
            Assert.False(extent.Contains(this.c2C));
            Assert.False(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            // Intersect + Intersect + Intersect
            firstExtent = this.Transaction.Intersect(
                this.Transaction.Extent(m.C1.Composite),
                this.Transaction.Extent(m.C1.Composite));
            secondExtent = this.Transaction.Intersect(
                this.Transaction.Extent(m.C1.Composite),
                this.Transaction.Extent(m.C1.Composite));

            extent = this.Transaction.Intersect(firstExtent, secondExtent);

            Assert.Equal(4, extent.Count);
            Assert.True(extent.Contains(this.c1A));
            Assert.True(extent.Contains(this.c1B));
            Assert.True(extent.Contains(this.c1C));
            Assert.True(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.False(extent.Contains(this.c2B));
            Assert.False(extent.Contains(this.c2C));
            Assert.False(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));

            // Except + Intersect + Intersect
            firstExtent = this.Transaction.Intersect(
                this.Transaction.Extent(m.C1.Composite),
                this.Transaction.Extent(m.C1.Composite));
            secondExtent = this.Transaction.Intersect(
                this.Transaction.Extent(m.C1.Composite),
                this.Transaction.Extent(m.C1.Composite));

            extent = this.Transaction.Except(firstExtent, secondExtent);

            Assert.Empty(extent);
            Assert.False(extent.Contains(this.c1A));
            Assert.False(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.False(extent.Contains(this.c2A));
            Assert.False(extent.Contains(this.c2B));
            Assert.False(extent.Contains(this.c2C));
            Assert.False(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));
        }
    }

    [Fact]
    public void NoClass()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            var extent = this.Transaction.Extent(m.InterfaceWithoutClass.Composite);

            Assert.Empty(extent);
        }
    }

    [Fact]
    public void Equalz()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // class
            var extent = this.Transaction.Extent(m.C1.Composite);

            extent.Filter.AddEquals(this.c1A);

            Assert.Single(extent);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent.Filter.AddEquals(this.c1B);

            Assert.Empty(extent);

            // interface
            extent = this.Transaction.Extent(m.I1.Composite);

            extent.Filter.AddEquals(this.c1A);

            Assert.Single(extent);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent.Filter.AddEquals(this.c1B);

            Assert.Empty(extent);

            // shared interface
            extent = this.Transaction.Extent(m.I12.Composite);

            extent.Filter.AddEquals(this.c1A);

            Assert.Single(extent);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent.Filter.AddEquals(this.c1B);

            Assert.Empty(extent);
        }
    }

    [Fact]
    public void NotAndEquals()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // class
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot(v =>
            {
                v.AddAnd(w =>
                {
                    w.AddEquals(this.c1A);
                });
            });

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot(v =>
            {
                v.AddAnd(w =>
                {
                    w.AddEquals(this.c1A);
                    w.AddEquals(this.c1B);
                });
            });

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, true, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // interface
            extent = this.Transaction.Extent(m.I1.Composite);
            extent.Filter.AddNot(v =>
            {
                v.AddAnd(x =>
                {
                    x.AddEquals(this.c1A);
                });
            });

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.I1.Composite);
            extent.Filter.AddNot(v =>
            {
                v.AddAnd(w =>
                {
                    w.AddEquals(this.c1A);
                    w.AddEquals(this.c1B);
                });
            });

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, true, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // shared interface
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddNot(v =>
            {
                v.AddAnd(w =>
                {
                    w.AddEquals(this.c1A);
                });
            });

            Assert.Equal(7, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, true, true, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddNot(v =>
            {
                v.AddAnd(w =>
                {
                    w.AddEquals(this.c1A);
                    w.AddEquals(this.c1B);
                });
            });

            Assert.Equal(8, extent.Count);
            this.AssertC1(extent, true, true, true, true);
            this.AssertC2(extent, true, true, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);
        }
    }

    [Fact]
    public void OrEquals()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // class
            var extent = this.Transaction.Extent(m.C1.Composite);
            var or = extent.Filter.AddOr();
            or.AddEquals(this.c1A);

            Assert.Single(extent);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            or.AddEquals(this.c1B);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, true, true, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // interface
            extent = this.Transaction.Extent(m.I1.Composite);
            or = extent.Filter.AddOr();
            or.AddEquals(this.c1A);

            Assert.Single(extent);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            or.AddEquals(this.c1B);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, true, true, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // shared interface
            extent = this.Transaction.Extent(m.I12.Composite);
            or = extent.Filter.AddOr();
            or.AddEquals(this.c1A);

            Assert.Single(extent);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            or.AddEquals(this.c2B);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);
        }
    }

    [Fact]
    public void NotOrEquals()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // class
            var extent = this.Transaction.Extent(m.C1.Composite);
            var not = extent.Filter.AddNot();
            var or = not.AddOr();
            or.AddEquals(this.c1A);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            or.AddEquals(this.c1B);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // interface
            extent = this.Transaction.Extent(m.I1.Composite);
            not = extent.Filter.AddNot();
            or = not.AddOr();
            or.AddEquals(this.c1A);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            or.AddEquals(this.c1B);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // shared interface
            extent = this.Transaction.Extent(m.I12.Composite);
            not = extent.Filter.AddNot();
            or = not.AddOr();
            or.AddEquals(this.c1A);

            Assert.Equal(7, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, true, true, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            or.AddEquals(this.c2B);

            Assert.Equal(6, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, true, false, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);
        }
    }

    [Fact]
    public virtual void Except()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // class
            var firstExtent = this.Transaction.Extent(m.C1.Composite);

            var secondExtent = this.Transaction.Extent(m.C1.Composite);
            secondExtent.Filter.AddLike(m.C1.C1AllorsString, "ᴀbracadabra");

            var extent = this.Transaction.Except(firstExtent, secondExtent);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, true, true, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // interface
            firstExtent = this.Transaction.Extent(m.I12.Composite);
            firstExtent.Filter.AddLike(m.I12.I12AllorsString, "ᴀbra%");

            secondExtent = this.Transaction.Extent(m.I12.Composite);
            secondExtent.Filter.AddLike(m.I12.I12AllorsString, "ᴀbracadabra");

            extent = this.Transaction.Except(firstExtent, secondExtent);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Different Classes
            firstExtent = this.Transaction.Extent(m.C1.Composite);
            secondExtent = this.Transaction.Extent(m.C2.Composite);

            var exceptionThrown = false;
            try
            {
                extent = this.Transaction.Except(firstExtent, secondExtent);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            Assert.True(exceptionThrown);
        }
    }

    [Fact]
    public void Grow()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddExists(m.C1.C1AllorsString);
            Assert.Equal(3, extent.Count);
            extent.Filter.AddLike(m.C1.C1AllorsString, "ᴀbra");
            Assert.Single(extent);

            // TODO: all possible combinations
        }
    }

    [Fact]
    public void InstanceOf()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class + Class
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddInstanceOf(m.C1.Composite);

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, true, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Class + Interface
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddInstanceOf(m.C1.Composite);

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, true, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Class + Shared Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddInstanceOf(m.C1.Composite);

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, true, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Inteface + Class
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddInstanceOf(m.I1.Composite);

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, true, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface + Interface
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddInstanceOf(m.I1.Composite);

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, true, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddInstanceOf(m.I12.Composite);

            Assert.Equal(8, extent.Count);
            this.AssertC1(extent, true, true, true, true);
            this.AssertC2(extent, true, true, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface + Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddInstanceOf(m.I1.Composite);

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, true, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddInstanceOf(m.I12.Composite);

            Assert.Equal(8, extent.Count);
            this.AssertC1(extent, true, true, true, true);
            this.AssertC2(extent, true, true, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddInstanceOf(m.S1234.Composite);

            Assert.Equal(16, extent.Count);
            this.AssertC1(extent, true, true, true, true);
            this.AssertC2(extent, true, true, true, true);
            this.AssertC3(extent, true, true, true, true);
            this.AssertC4(extent, true, true, true, true);
        }
    }

    [Fact]
    public virtual void Intersect()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // class
            var firstExtent = this.Transaction.Extent(m.C1.Composite);

            var secondExtent = this.Transaction.Extent(m.C1.Composite);
            secondExtent.Filter.AddLike(m.C1.C1AllorsString, "ᴀbracadabra");

            var extent = this.Transaction.Intersect(firstExtent, secondExtent);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Different Classes
            firstExtent = this.Transaction.Extent(m.C1.Composite);
            secondExtent = this.Transaction.Extent(m.C2.Composite);

            var exceptionThrown = false;
            try
            {
                this.Transaction.Intersect(firstExtent, secondExtent);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            Assert.True(exceptionThrown);

            // Interface
            firstExtent = this.Transaction.Extent(m.I12.Composite);
            secondExtent = this.Transaction.Extent(m.I12.Composite);
            secondExtent.Filter.AddInstanceOf(m.C2.Composite);

            Assert.Equal(4, secondExtent.Count);

            extent = this.Transaction.Intersect(firstExtent, secondExtent);

            Assert.Equal(4, extent.Count);
            Assert.False(extent.Contains(this.c1A));
            Assert.False(extent.Contains(this.c1B));
            Assert.False(extent.Contains(this.c1C));
            Assert.False(extent.Contains(this.c1D));
            Assert.True(extent.Contains(this.c2A));
            Assert.True(extent.Contains(this.c2B));
            Assert.True(extent.Contains(this.c2C));
            Assert.True(extent.Contains(this.c2D));
            Assert.False(extent.Contains(this.c3A));
            Assert.False(extent.Contains(this.c3B));
            Assert.False(extent.Contains(this.c3C));
            Assert.False(extent.Contains(this.c3D));
            Assert.False(extent.Contains(this.c4A));
            Assert.False(extent.Contains(this.c4B));
            Assert.False(extent.Contains(this.c4C));
            Assert.False(extent.Contains(this.c4D));
        }
    }

    [Fact]
    public void Naming()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            {
                var extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddEquals(m.S1234.ClassName, "c1");
                extent.Filter.AddContains(m.C1.C1C3one2manies, this.c3B);
                extent.AddSort(m.S1234.ClassName);
                extent.ToArray(typeof(C1));
            }
        }
    }

    [Fact]
    public void NotAnd()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot(v =>
            {
                v.AddAnd(w =>
                {
                    w.AddGreaterThan(m.C1.C1AllorsInteger, 0);
                    w.AddLessThan(m.C1.C1AllorsInteger, 2);
                });
            });

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface
            extent = this.Transaction.Extent(m.I12.Composite);
            {
                extent.Filter.AddNot(v =>
                {
                    v.AddAnd(w =>
                    {
                        w.AddGreaterThan(m.I12.I12AllorsInteger, 0);
                        w.AddLessThan(m.I12.I12AllorsInteger, 2);
                    });
                });
            }

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            {
                extent.Filter.AddNot(v =>
                {
                    v.AddAnd(w =>
                    {
                        w.AddGreaterThan(m.S1234.S1234AllorsInteger, 0);
                        w.AddLessThan(m.S1234.S1234AllorsInteger, 2);
                    });
                });
            }

            Assert.Equal(8, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, true, true);
            this.AssertC3(extent, false, false, true, true);
            this.AssertC4(extent, false, false, true, true);

            // Class
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot(v =>
            {
                v.AddAnd();
            });

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, true, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);
        }
    }

    [Fact]
    public void NotAssociationMany2ManyIn()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            this.Transaction.Commit();

            foreach (var useOperator in this.UseOperator)
            {
                // Extent over C2
                // RelationType from C1 to C2

                // In Extent over Class
                // Empty
                var inExtent = this.Transaction.Extent(m.C1.Composite);
                inExtent.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    inExtentA.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtentB.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                var extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.C2.C1sWhereC1C2many2many, inExtent);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full
                inExtent = this.Transaction.Extent(m.C1.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.C2.C1sWhereC1C2many2many, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Filtered
                inExtent = this.Transaction.Extent(m.C1.Composite);
                inExtent.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    inExtentA.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtentB.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.C2.C1sWhereC1C2many2many, inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, false, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // In Extent over Interface
                // Empty
                inExtent = this.Transaction.Extent(m.I12.Composite);
                inExtent.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.C2.C1sWhereC1C2many2many, inExtent);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full
                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.C2.C1sWhereC1C2many2many, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Filtered
                inExtent = this.Transaction.Extent(m.I12.Composite);
                inExtent.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.C2.C1sWhereC1C2many2many, inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, false, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // RelationType from C1 to I12

                // In Extent over Class
                // Empty
                inExtent = this.Transaction.Extent(m.C1.Composite);
                inExtent.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    inExtentA.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtentB.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.I12.C1sWhereC1I12many2many, inExtent);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full
                inExtent = this.Transaction.Extent(m.C1.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.I12.C1sWhereC1I12many2many, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Filtered
                inExtent = this.Transaction.Extent(m.C1.Composite);
                inExtent.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    inExtentA.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtentB.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.I12.C1sWhereC1I12many2many, inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, false, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // In Extent over Interface
                // Empty
                inExtent = this.Transaction.Extent(m.I12.Composite);
                inExtent.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.I12.C1sWhereC1I12many2many, inExtent);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full
                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.I12.C1sWhereC1I12many2many, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Filtered
                inExtent = this.Transaction.Extent(m.I12.Composite);
                inExtent.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.I12.C1sWhereC1I12many2many, inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, false, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);
            }
        }
    }

    [Fact]
    public void NotAssociationMany2ManyExist()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class
            var extent = this.Transaction.Extent(m.C2.Composite);
            extent.Filter.AddNot().AddExists(m.C2.C1sWhereC1C2many2many);

            Assert.Single(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, true, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface
            extent = this.Transaction.Extent(m.I2.Composite);
            extent.Filter.AddNot().AddExists(m.I2.I1sWhereI1I2many2many);

            Assert.Single(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, true, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddNot().AddExists(m.S1234.S1234sWhereS1234many2many);

            Assert.Equal(6, extent.Count);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, true, false, false, false);
            this.AssertC3(extent, true, false, false, false);
            this.AssertC4(extent, true, true, true, true);

            // Class - Wrong RelationType
            extent = this.Transaction.Extent(m.C2.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddNot().AddExists(m.C1.C1sWhereC1C1many2many);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.I12.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddExists(m.I34.I12sWhereI12I34many2many);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.S12.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddExists(m.S1234.S1234sWhereS1234many2many);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void NotAssociationMany2OneIn()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            foreach (var useOperator in this.UseOperator)
            {
                // Extent over Class

                // RelationType from Class to Class

                // In Extent over Class
                // Empty
                var inExtent = this.Transaction.Extent(m.C1.Composite);
                inExtent.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    inExtentA.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtentB.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                var extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.C2.C1sWhereC1C2many2one, inExtent);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full
                inExtent = this.Transaction.Extent(m.C1.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.C2.C1sWhereC1C2many2one, inExtent);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, false, false, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Filtered
                inExtent = this.Transaction.Extent(m.C1.Composite);
                inExtent.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    inExtentA.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtentB.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.C2.C1sWhereC1C2many2one, inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, false, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // In Extent over Interface
                // Empty
                inExtent = this.Transaction.Extent(m.I12.Composite);
                inExtent.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.C2.C1sWhereC1C2many2one, inExtent);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full
                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.C2.C1sWhereC1C2many2one, inExtent);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, false, false, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Filtered
                inExtent = this.Transaction.Extent(m.I12.Composite);
                inExtent.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.C2.C1sWhereC1C2many2one, inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, false, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // RelationType from Class to Interface

                // In Extent over Class
                // Empty
                inExtent = this.Transaction.Extent(m.C1.Composite);
                inExtent.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    inExtentA.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtentB.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.I12.C1sWhereC1I12many2one, inExtent);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full
                inExtent = this.Transaction.Extent(m.C1.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.I12.C1sWhereC1I12many2one, inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, true, false, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Filtered
                inExtent = this.Transaction.Extent(m.C1.Composite);
                inExtent.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    inExtentA.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtentB.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.I12.C1sWhereC1I12many2one, inExtent);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // In Extent over Interface
                // Empty
                inExtent = this.Transaction.Extent(m.I12.Composite);
                inExtent.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.I12.C1sWhereC1I12many2one, inExtent);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full
                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.I12.C1sWhereC1I12many2one, inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, true, false, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Filtered
                inExtent = this.Transaction.Extent(m.I12.Composite);
                inExtent.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.I12.C1sWhereC1I12many2one, inExtent);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);
            }
        }
    }

    [Fact]
    public void NotAssociationOne2ManyIn()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            foreach (var useOperator in this.UseOperator)
            {
                // Extent over C2

                // RelationType from C1 to C2

                // In Extent over Class
                // Empty
                var inExtent = this.Transaction.Extent(m.C1.Composite);
                inExtent.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    inExtentA.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtentB.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                var extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.C2.C1WhereC1C2one2many, inExtent);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full
                inExtent = this.Transaction.Extent(m.C1.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.C2.C1WhereC1C2one2many, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Filtered
                inExtent = this.Transaction.Extent(m.C1.Composite);
                inExtent.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    inExtentA.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtentB.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.C2.C1WhereC1C2one2many, inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, false, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // In Extent over Interface
                // Empty
                inExtent = this.Transaction.Extent(m.I12.Composite);
                inExtent.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.C2.C1WhereC1C2one2many, inExtent);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full
                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.C2.C1WhereC1C2one2many, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Filtered
                inExtent = this.Transaction.Extent(m.I12.Composite);
                inExtent.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.C2.C1WhereC1C2one2many, inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, false, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // RelationType from C1 to I12

                // In Extent over Class
                // Empty
                inExtent = this.Transaction.Extent(m.C1.Composite);
                inExtent.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    inExtentA.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtentB.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.I12.C1WhereC1I12one2many, inExtent);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full
                inExtent = this.Transaction.Extent(m.C1.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.I12.C1WhereC1I12one2many, inExtent);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, true, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Filtered
                inExtent = this.Transaction.Extent(m.C1.Composite);
                inExtent.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    inExtentA.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtentB.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.I12.C1WhereC1I12one2many, inExtent);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // In Extent over Interface
                // Empty
                inExtent = this.Transaction.Extent(m.I12.Composite);
                inExtent.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.I12.C1WhereC1I12one2many, inExtent);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full
                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.I12.C1WhereC1I12one2many, inExtent);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, true, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Filtered
                inExtent = this.Transaction.Extent(m.I12.Composite);
                inExtent.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.I12.C1WhereC1I12one2many, inExtent);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);
            }
        }
    }

    [Fact]
    public void NotAssociationOne2ManyEquals()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddEquals(m.C1.C1WhereC1C1one2many, this.c1B);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, true, false, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddEquals(m.C1.C1WhereC1C1one2many, this.c1C);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, true, true, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.C2.Composite);
            extent.Filter.AddNot().AddEquals(m.C2.C1WhereC1C2one2many, this.c1B);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, true, false, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.C2.Composite);
            extent.Filter.AddNot().AddEquals(m.C2.C1WhereC1C2one2many, this.c1C);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, true, true, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface
            extent = this.Transaction.Extent(m.I2.Composite);
            extent.Filter.AddNot().AddEquals(m.I2.I1WhereI1I2one2many, this.c1B);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, true, false, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.I2.Composite);
            extent.Filter.AddNot().AddEquals(m.I2.I1WhereI1I2one2many, this.c1C);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, true, true, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddNot().AddEquals(m.S1234.S1234WhereS1234one2many, this.c1B);

            Assert.Equal(15, extent.Count);
            this.AssertC1(extent, true, false, true, true);
            this.AssertC2(extent, true, true, true, true);
            this.AssertC3(extent, true, true, true, true);
            this.AssertC4(extent, true, true, true, true);

            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddNot().AddEquals(m.S1234.S1234WhereS1234one2many, this.c3C);

            Assert.Equal(14, extent.Count);
            this.AssertC1(extent, true, true, false, false);
            this.AssertC2(extent, true, true, true, true);
            this.AssertC3(extent, true, true, true, true);
            this.AssertC4(extent, true, true, true, true);

            // Class - Wrong RelationType
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddNot().AddEquals(m.C2.C3WhereC3C2one2many, this.c2A);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.I12.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddEquals(m.C2.C3WhereC3C2one2many, this.c2A);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.S12.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddEquals(m.C2.C3WhereC3C2one2many, this.c2A);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void NotAssociationOne2ManyExist()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class
            var extent = this.Transaction.Extent(m.C2.Composite);
            extent.Filter.AddNot().AddExists(m.C2.C1WhereC1C2one2many);

            Assert.Single(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, true, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface
            extent = this.Transaction.Extent(m.I2.Composite);
            extent.Filter.AddNot().AddExists(m.I2.I1WhereI1I2one2many);

            Assert.Single(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, true, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddNot().AddExists(m.S1234.S1234WhereS1234one2many);

            Assert.Equal(13, extent.Count);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, true, true, true, true);
            this.AssertC3(extent, true, true, true, true);
            this.AssertC4(extent, true, true, true, true);

            // Class - Wrong RelationType
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddNot().AddExists(m.C2.C3WhereC3C2one2many);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.I12.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddExists(m.C2.C3WhereC3C2one2many);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.S1234.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddExists(m.C2.C3WhereC3C2one2many);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void NotAssociationOne2OneIn()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            foreach (var useOperator in this.UseOperator)
            {
                // Extent over Class

                // RelationType from C1 to C1

                // In Extent over Class
                var inExtent = this.Transaction.Extent(m.C1.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                var extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1WhereC1C1one2one, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // In Extent over Interface
                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1WhereC1C1one2one, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // RelationType from C1 to C2

                // In Extent over Class
                inExtent = this.Transaction.Extent(m.C1.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.C2.C1WhereC1C2one2one, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // In Extent over Class
                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.C2.C1WhereC1C2one2one, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // RelationType from C3 to C4

                // In Extent over Class
                inExtent = this.Transaction.Extent(m.C3.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C3.Composite);
                    var inExtentB = this.Transaction.Extent(m.C3.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C4.Composite);
                extent.Filter.AddNot().AddIn(m.C4.C3WhereC3C4one2one, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, true, false, false, false);

                // In Extent over Interface
                inExtent = this.Transaction.Extent(m.I34.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I34.Composite);
                    var inExtentB = this.Transaction.Extent(m.I34.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C4.Composite);
                extent.Filter.AddNot().AddIn(m.C4.C3WhereC3C4one2one, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, true, false, false, false);

                // RelationType from I12 to C2

                // In Extent over Class
                inExtent = this.Transaction.Extent(m.C1.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.C2.I12WhereI12C2one2one, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, true, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // In Extent over Interface
                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddNot().AddIn(m.C2.I12WhereI12C2one2one, inExtent);

                Assert.Empty(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Extent over Interface

                // RelationType from C1 to I12

                // In Extent over Class
                inExtent = this.Transaction.Extent(m.C1.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.I12.Composite);
                extent.Filter.AddNot().AddIn(m.I12.C1WhereC1I12one2one, inExtent);

                Assert.Equal(5, extent.Count);
                this.AssertC1(extent, true, false, true, true);
                this.AssertC2(extent, true, false, false, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // In Extent over Interface
                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.I12.Composite);
                extent.Filter.AddNot().AddIn(m.I12.C1WhereC1I12one2one, inExtent);

                Assert.Equal(5, extent.Count);
                this.AssertC1(extent, true, false, true, true);
                this.AssertC2(extent, true, false, false, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);
            }
        }
    }

    [Fact]
    public void NotAssociationOne2OneEquals()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddEquals(m.C1.C1WhereC1C1one2one, this.c1B);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, true, false, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.C2.Composite);
            extent.Filter.AddNot().AddEquals(m.C2.C1WhereC1C2one2one, this.c1B);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, true, false, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.C4.Composite);
            extent.Filter.AddNot().AddEquals(m.C4.C3WhereC3C4one2one, this.c3B);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, true, false, true, true);

            // Interface
            extent = this.Transaction.Extent(m.I2.Composite);
            extent.Filter.AddNot().AddEquals(m.I2.I1WhereI1I2one2one, this.c1B);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, true, false, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddNot().AddEquals(m.S1234.S1234WhereS1234one2one, this.c1C);

            Assert.Equal(15, extent.Count);
            this.AssertC1(extent, true, true, true, true);
            this.AssertC2(extent, true, false, true, true);
            this.AssertC3(extent, true, true, true, true);
            this.AssertC4(extent, true, true, true, true);

            // Class - Wrong RelationType
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddNot().AddEquals(m.C2.C3WhereC3C2one2one, this.c2A);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.I12.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddEquals(m.C2.C3WhereC3C2one2one, this.c2A);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.S12.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddEquals(m.C2.C3WhereC3C2one2one, this.c2A);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void NotAssociationOne2OneExist()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddExists(m.C1.C1WhereC1C1one2one);

            Assert.Single(extent);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.C2.Composite);
            extent.Filter.AddNot().AddExists(m.C2.C1WhereC1C2one2one);

            Assert.Single(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, true, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.C2.Composite);
            extent.Filter.AddNot().AddExists(m.C2.C1WhereC1C2one2one);

            Assert.Single(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, true, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.C4.Composite);
            extent.Filter.AddNot().AddExists(m.C4.C3WhereC3C4one2one);

            Assert.Single(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, true, false, false, false);

            // Interface
            extent = this.Transaction.Extent(m.I2.Composite);
            extent.Filter.AddNot().AddExists(m.I2.I1WhereI1I2one2one);

            Assert.Single(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, true, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddNot().AddExists(m.S1234.S1234WhereS1234one2one);

            Assert.Equal(7, extent.Count);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, true, false, false, false);
            this.AssertC3(extent, true, false, false, false);
            this.AssertC4(extent, true, true, true, true);

            // Class - Wrong RelationType
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddNot().AddExists(m.C2.C3WhereC3C2one2one);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.I12.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddExists(m.C2.C3WhereC3C2one2one);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.S1234.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddExists(m.C2.C3WhereC3C2one2one);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void NotAssociationOne2OneInstanceof()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class

            // Class
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddInstanceOf(m.C1.C1WhereC1C1one2one, m.C1.Composite);

            Assert.Single(extent);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.C2.Composite);
            extent.Filter.AddNot().AddInstanceOf(m.C2.C1WhereC1C2one2one, m.C1.Composite);

            Assert.Single(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, true, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.C4.Composite);
            extent.Filter.AddNot().AddInstanceOf(m.C4.C3WhereC3C4one2one, m.C3.Composite);

            Assert.Single(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, true, false, false, false);

            // Interface
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddNot().AddInstanceOf(m.I12.C1WhereC1I12one2one, m.C1.Composite);

            Assert.Equal(5, extent.Count);
            this.AssertC1(extent, true, false, true, true);
            this.AssertC2(extent, true, false, false, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddNot().AddInstanceOf(m.S1234.S1234WhereS1234one2one, m.C1.Composite);

            Assert.Equal(13, extent.Count);
            this.AssertC1(extent, true, false, true, true);
            this.AssertC2(extent, true, false, true, true);
            this.AssertC3(extent, true, false, true, true);
            this.AssertC4(extent, true, true, true, true);

            // Interface

            // Class
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddInstanceOf(m.C1.C1WhereC1C1one2one, m.I1.Composite);

            Assert.Single(extent);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.C2.Composite);
            extent.Filter.AddNot().AddInstanceOf(m.C2.C1WhereC1C2one2one, m.I1.Composite);

            Assert.Single(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, true, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.C4.Composite);
            extent.Filter.AddNot().AddInstanceOf(m.C4.C3WhereC3C4one2one, m.I3.Composite);

            Assert.Single(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, true, false, false, false);

            // Interface
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddNot().AddInstanceOf(m.I12.C1WhereC1I12one2one, m.I1.Composite);

            Assert.Equal(5, extent.Count);
            this.AssertC1(extent, true, false, true, true);
            this.AssertC2(extent, true, false, false, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddNot().AddInstanceOf(m.S1234.S1234WhereS1234one2one, m.S1234.Composite);

            Assert.Equal(7, extent.Count);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, true, false, false, false);
            this.AssertC3(extent, true, false, false, false);
            this.AssertC4(extent, true, true, true, true);

            // TODO: wrong relation
        }
    }

    [Fact]
    public void NotOr()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class
            var extent = this.Transaction.Extent(m.C1.Composite);
            var none = extent.Filter.AddNot().AddOr();
            none.AddGreaterThan(m.C1.C1AllorsInteger, 1);
            none.AddLessThan(m.C1.C1AllorsInteger, 1);

            Assert.Single(extent);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface
            extent = this.Transaction.Extent(m.I12.Composite);
            {
                var not = extent.Filter.AddNot();
                var or = not.AddOr();
                or.AddGreaterThan(m.I12.I12AllorsInteger, 1);
                or.AddLessThan(m.I12.I12AllorsInteger, 1);
            }

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            {
                var not = extent.Filter.AddNot();
                var or = not.AddOr();
                or.AddGreaterThan(m.S1234.S1234AllorsInteger, 1);
                or.AddLessThan(m.S1234.S1234AllorsInteger, 1);
            }

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, true, false, false);
            this.AssertC4(extent, false, true, false, false);

            // Class
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddOr();

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, true, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);
        }
    }

    [Fact]
    public void NotRoleIntegerBetweenValue()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class

            // Between -10 and 0
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddBetween(m.C1.C1AllorsInteger, -10, 0);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Between 0 and 1
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddBetween(m.C1.C1AllorsInteger, 0, 1);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Between 1 and 2
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddBetween(m.C1.C1AllorsInteger, 1, 2);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Between 3 and 10
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddBetween(m.C1.C1AllorsInteger, 3, 10);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface

            // Between -10 and 0
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddNot().AddBetween(m.I12.I12AllorsInteger, -10, 0);

            Assert.Equal(6, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Between 0 and 1
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddNot().AddBetween(m.I12.I12AllorsInteger, 0, 1);

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Between 1 and 2
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddNot().AddBetween(m.I12.I12AllorsInteger, 1, 2);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Between 3 and 10
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddNot().AddBetween(m.I12.I12AllorsInteger, 3, 10);

            Assert.Equal(6, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface

            // Between -10 and 0
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddNot().AddBetween(m.S1234.S1234AllorsInteger, -10, 0);

            Assert.Equal(12, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, true, true, true);
            this.AssertC4(extent, false, true, true, true);

            // Between 0 and 1
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddNot().AddBetween(m.S1234.S1234AllorsInteger, 0, 1);

            Assert.Equal(8, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, true, true);
            this.AssertC3(extent, false, false, true, true);
            this.AssertC4(extent, false, false, true, true);

            // Between 1 and 2
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddNot().AddBetween(m.S1234.S1234AllorsInteger, 1, 2);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Between 3 and 10
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddNot().AddBetween(m.S1234.S1234AllorsInteger, 3, 10);

            Assert.Equal(12, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, true, true, true);
            this.AssertC4(extent, false, true, true, true);

            // Class - Wrong RelationType

            // Between -10 and 0
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddNot().AddBetween(m.C2.C2AllorsInteger, -10, 0);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Between 0 and 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddBetween(m.C2.C2AllorsInteger, 0, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Between 1 and 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddBetween(m.C2.C2AllorsInteger, 1, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Between 3 and 10
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddBetween(m.C2.C2AllorsInteger, 3, 10);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void NotRoleIntegerLessThanValue()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class

            // Less Than 1
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddLessThan(m.C1.C1AllorsInteger, 1);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Less Than 2
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddLessThan(m.C1.C1AllorsInteger, 2);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Less Than 3
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddLessThan(m.C1.C1AllorsInteger, 3);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface

            // Less Than 1
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddNot().AddLessThan(m.I12.I12AllorsInteger, 1);

            Assert.Equal(6, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Less Than 2
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddNot().AddLessThan(m.I12.I12AllorsInteger, 2);

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Less Than 3
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddNot().AddLessThan(m.I12.I12AllorsInteger, 3);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface

            // Less Than 1
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddNot().AddLessThan(m.S1234.S1234AllorsInteger, 1);

            Assert.Equal(12, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, true, true, true);
            this.AssertC4(extent, false, true, true, true);

            // Less Than 2
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddNot().AddLessThan(m.S1234.S1234AllorsInteger, 2);

            Assert.Equal(8, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, true, true);
            this.AssertC3(extent, false, false, true, true);
            this.AssertC4(extent, false, false, true, true);

            // Less Than 3
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddNot().AddLessThan(m.S1234.S1234AllorsInteger, 3);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Class - Wrong RelationType

            // Less Than 1
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddNot().AddLessThan(m.C2.C2AllorsInteger, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Less Than 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddLessThan(m.C2.C2AllorsInteger, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Less Than 3
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddLessThan(m.C2.C2AllorsInteger, 3);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType

            // Less Than 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddLessThan(m.I2.I2AllorsInteger, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Less Than 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddLessThan(m.I2.I2AllorsInteger, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Less Than 3
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddLessThan(m.I2.I2AllorsInteger, 3);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType

            // Less Than 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddLessThan(m.S2.S2AllorsInteger, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Less Than 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddLessThan(m.S2.S2AllorsInteger, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Less Than 3
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddLessThan(m.S2.S2AllorsInteger, 3);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void NotRoleIntegerGreaterThanValue()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class

            // Greater Than 0
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddGreaterThan(m.C1.C1AllorsInteger, 0);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Greater Than 1
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddGreaterThan(m.C1.C1AllorsInteger, 1);

            Assert.Single(extent);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Greater Than 2
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddGreaterThan(m.C1.C1AllorsInteger, 2);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface

            // Greater Than 0
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddNot().AddGreaterThan(m.I12.I12AllorsInteger, 0);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Greater Than 1
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddNot().AddGreaterThan(m.I12.I12AllorsInteger, 1);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Greater Than 2
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddNot().AddGreaterThan(m.I12.I12AllorsInteger, 2);

            Assert.Equal(6, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface

            // Greater Than 0
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddNot().AddGreaterThan(m.S1234.S1234AllorsInteger, 0);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Greater Than 1
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddNot().AddGreaterThan(m.S1234.S1234AllorsInteger, 1);

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, true, false, false);
            this.AssertC4(extent, false, true, false, false);

            // Greater Than 2
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddNot().AddGreaterThan(m.S1234.S1234AllorsInteger, 2);

            Assert.Equal(12, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, true, true, true);
            this.AssertC4(extent, false, true, true, true);

            // Class - Wrong RelationType

            // Greater Than 0
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddNot().AddGreaterThan(m.C2.C2AllorsInteger, 0);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Greater Than 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddGreaterThan(m.C2.C2AllorsInteger, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Greater Than 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddGreaterThan(m.C2.C2AllorsInteger, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType

            // Greater Than 0
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddGreaterThan(m.I2.I2AllorsInteger, 0);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Greater Than 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddGreaterThan(m.I2.I2AllorsInteger, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Greater Than 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddGreaterThan(m.I2.I2AllorsInteger, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType

            // Greater Than 0
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddGreaterThan(m.I2.I2AllorsInteger, 0);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Greater Than 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddGreaterThan(m.I2.I2AllorsInteger, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Greater Than 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddGreaterThan(m.I2.I2AllorsInteger, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void NotRoleIntegerExist()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddExists(m.C1.C1AllorsInteger);

            Assert.Single(extent);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddNot().AddExists(m.I12.I12AllorsInteger);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, true, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddNot().AddExists(m.S1234.S1234AllorsInteger);

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, true, false, false, false);
            this.AssertC3(extent, true, false, false, false);
            this.AssertC4(extent, true, false, false, false);

            // Class - Wrong RelationType
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddNot().AddExists(m.C2.C2AllorsInteger);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddExists(m.I2.I2AllorsInteger);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddExists(m.S2.S2AllorsInteger);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void NotRoleMany2ManyInExtent()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            foreach (var useOperator in this.UseOperator)
            {
                // Extent over Class

                // RelationType from C1 to C2
                // In Extent over Class
                // Empty
                var inExtent = this.Transaction.Extent(m.C2.Composite);
                inExtent.Filter.AddEquals(m.C2.C2AllorsString, "Nothing here!");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C2.Composite);
                    inExtentA.Filter.AddEquals(m.C2.C2AllorsString, "Nothing here!");
                    var inExtentB = this.Transaction.Extent(m.C2.Composite);
                    inExtentB.Filter.AddEquals(m.C2.C2AllorsString, "Nothing here!");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                var extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C2many2manies, inExtent);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, true, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full
                inExtent = this.Transaction.Extent(m.C2.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C2.Composite);
                    var inExtentB = this.Transaction.Extent(m.C2.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C2many2manies, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Filtered
                inExtent = this.Transaction.Extent(m.C2.Composite);
                inExtent.Filter.AddEquals(m.C2.C2AllorsString, "ᴀbra");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C2.Composite);
                    inExtentA.Filter.AddEquals(m.C2.C2AllorsString, "ᴀbra");
                    var inExtentB = this.Transaction.Extent(m.C2.Composite);
                    inExtentB.Filter.AddEquals(m.C2.C2AllorsString, "ᴀbra");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C2many2manies, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // In Extent over Class
                // Empty
                inExtent = this.Transaction.Extent(m.I12.Composite);
                inExtent.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C2many2manies, inExtent);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, true, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full
                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C2many2manies, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Filtered
                inExtent = this.Transaction.Extent(m.I12.Composite);
                inExtent.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C2many2manies, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // RelationType from C1 to C1

                // In Extent over Class
                // Empty
                inExtent = this.Transaction.Extent(m.C1.Composite);
                inExtent.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    inExtentA.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtentB.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C1many2manies, inExtent);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, true, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full
                inExtent = this.Transaction.Extent(m.C1.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C1many2manies, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Filtered
                inExtent = this.Transaction.Extent(m.C1.Composite);
                inExtent.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    inExtentA.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtentB.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C1many2manies, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // In Extent over Class
                // Empty
                inExtent = this.Transaction.Extent(m.I12.Composite);
                inExtent.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C1many2manies, inExtent);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, true, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full
                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C1many2manies, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Filtered
                inExtent = this.Transaction.Extent(m.I12.Composite);
                inExtent.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C1many2manies, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // RelationType from C1 to I12

                // In Extent over Class
                // Empty
                inExtent = this.Transaction.Extent(m.C1.Composite);
                inExtent.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    inExtentA.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtentB.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1I12many2manies, inExtent);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, true, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full
                inExtent = this.Transaction.Extent(m.C1.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1I12many2manies, inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, true, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Filtered
                inExtent = this.Transaction.Extent(m.C1.Composite);
                inExtent.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    inExtentA.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtentB.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1I12many2manies, inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, true, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // In Extent over Class
                // Empty
                inExtent = this.Transaction.Extent(m.I12.Composite);
                inExtent.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1I12many2manies, inExtent);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, true, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full
                inExtent = this.Transaction.Extent(m.C1.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1I12many2manies, inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, true, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Filtered
                inExtent = this.Transaction.Extent(m.I12.Composite);
                inExtent.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1I12many2manies, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // In Extent I12<->I34
                // Empty
                inExtent = this.Transaction.Extent(m.I34.Composite);
                inExtent.Filter.AddEquals(m.I34.I34AllorsString, "Nothing here!");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I34.Composite);
                    inExtentA.Filter.AddEquals(m.I34.I34AllorsString, "Nothing here!");
                    var inExtentB = this.Transaction.Extent(m.I34.Composite);
                    inExtentB.Filter.AddEquals(m.I34.I34AllorsString, "Nothing here!");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.I12.Composite);
                extent.Filter.AddNot().AddIn(m.I12.I12I34many2manies, inExtent);

                Assert.Equal(8, extent.Count);
                this.AssertC1(extent, true, true, true, true);
                this.AssertC2(extent, true, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full
                inExtent = this.Transaction.Extent(m.I34.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I34.Composite);
                    var inExtentB = this.Transaction.Extent(m.I34.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.I12.Composite);
                extent.Filter.AddNot().AddIn(m.I12.I12I34many2manies, inExtent);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Filtered
                inExtent = this.Transaction.Extent(m.I34.Composite);
                inExtent.Filter.AddEquals(m.I34.I34AllorsString, "ᴀbra");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I34.Composite);
                    inExtentA.Filter.AddEquals(m.I34.I34AllorsString, "ᴀbra");
                    var inExtentB = this.Transaction.Extent(m.I34.Composite);
                    inExtentB.Filter.AddEquals(m.I34.I34AllorsString, "ᴀbra");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.I12.Composite);
                extent.Filter.AddNot().AddIn(m.I12.I12I34many2manies, inExtent);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, true, true, true);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);
            }
        }
    }

    [Fact]
    public void NotRoleMany2ManyInArray()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            this.Transaction.Commit();

            // Extent over Class

            // RelationType from C1 to C2
            // In Extent over Class
            // Empty
            var inExtent = this.Transaction.Extent(m.C2.Composite);
            inExtent.Filter.AddEquals(m.C2.C2AllorsString, "Nothing here!");

            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddIn(m.C1.C1C2many2manies, (IEnumerable<IObject>)inExtent.ToArray());

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, true, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Full
            inExtent = this.Transaction.Extent(m.C2.Composite);

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddIn(m.C1.C1C2many2manies, (IEnumerable<IObject>)inExtent.ToArray());

            Assert.Single(extent);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Filtered
            inExtent = this.Transaction.Extent(m.C2.Composite);
            inExtent.Filter.AddEquals(m.C2.C2AllorsString, "ᴀbra");

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddIn(m.C1.C1C2many2manies, (IEnumerable<IObject>)inExtent.ToArray());

            Assert.Single(extent);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // In Extent over Class
            // Empty
            inExtent = this.Transaction.Extent(m.I12.Composite);
            inExtent.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddIn(m.C1.C1C2many2manies, (IEnumerable<IObject>)inExtent.ToArray());

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, true, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Full
            inExtent = this.Transaction.Extent(m.I12.Composite);

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddIn(m.C1.C1C2many2manies, (IEnumerable<IObject>)inExtent.ToArray());

            Assert.Single(extent);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Filtered
            inExtent = this.Transaction.Extent(m.I12.Composite);
            inExtent.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddIn(m.C1.C1C2many2manies, (IEnumerable<IObject>)inExtent.ToArray());

            Assert.Single(extent);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // RelationType from C1 to C1

            // In Extent over Class
            // Empty
            inExtent = this.Transaction.Extent(m.C1.Composite);
            inExtent.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddIn(m.C1.C1C1many2manies, (IEnumerable<IObject>)inExtent.ToArray());

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, true, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Full
            inExtent = this.Transaction.Extent(m.C1.Composite);

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddIn(m.C1.C1C1many2manies, (IEnumerable<IObject>)inExtent.ToArray());

            Assert.Single(extent);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Filtered
            inExtent = this.Transaction.Extent(m.C1.Composite);
            inExtent.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddIn(m.C1.C1C1many2manies, (IEnumerable<IObject>)inExtent.ToArray());

            Assert.Single(extent);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // In Extent over Class
            // Empty
            inExtent = this.Transaction.Extent(m.I12.Composite);
            inExtent.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddIn(m.C1.C1C1many2manies, (IEnumerable<IObject>)inExtent.ToArray());

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, true, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Full
            inExtent = this.Transaction.Extent(m.I12.Composite);

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddIn(m.C1.C1C1many2manies, (IEnumerable<IObject>)inExtent.ToArray());

            Assert.Single(extent);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Filtered
            inExtent = this.Transaction.Extent(m.I12.Composite);
            inExtent.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddIn(m.C1.C1C1many2manies, (IEnumerable<IObject>)inExtent.ToArray());

            Assert.Single(extent);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // RelationType from C1 to I12

            // In Extent over Class
            // Empty
            inExtent = this.Transaction.Extent(m.C1.Composite);
            inExtent.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddIn(m.C1.C1I12many2manies, (IEnumerable<IObject>)inExtent.ToArray());

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, true, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Full
            inExtent = this.Transaction.Extent(m.C1.Composite);

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddIn(m.C1.C1I12many2manies, (IEnumerable<IObject>)inExtent.ToArray());

            this.Transaction.Commit();

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, true, false, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Filtered
            inExtent = this.Transaction.Extent(m.C1.Composite);
            inExtent.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddIn(m.C1.C1I12many2manies, (IEnumerable<IObject>)inExtent.ToArray());

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, true, false, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // In Extent over Class
            // Empty
            inExtent = this.Transaction.Extent(m.I12.Composite);
            inExtent.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddIn(m.C1.C1I12many2manies, (IEnumerable<IObject>)inExtent.ToArray());

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, true, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Full
            inExtent = this.Transaction.Extent(m.C1.Composite);

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddIn(m.C1.C1I12many2manies, (IEnumerable<IObject>)inExtent.ToArray());

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, true, false, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Filtered
            inExtent = this.Transaction.Extent(m.I12.Composite);
            inExtent.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddIn(m.C1.C1I12many2manies, (IEnumerable<IObject>)inExtent.ToArray());

            Assert.Single(extent);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);
        }
    }

    [Fact]
    public void NotRoleMany2ManyExist()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddExists(m.C1.C1C2many2manies);

            Assert.Single(extent);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddNot().AddExists(m.I12.I12C2many2manies);

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddNot().AddExists(m.S1234.S1234C2many2manies);

            Assert.Equal(12, extent.Count);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, true, true, true, true);
            this.AssertC4(extent, true, true, true, true);

            // Class - Wrong RelationType
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddNot().AddExists(m.C3.C3C2many2manies);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.I12.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddExists(m.C3.C3C2many2manies);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.S12.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddExists(m.C3.C3C2many2manies);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void NotRoleOne2ManyIn()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            foreach (var useOperator in this.UseOperator)
            {
                // Extent over Class
                // RelationType from Class to Class

                // In Extent over Class
                // Emtpy Extent
                var inExtent = this.Transaction.Extent(m.C1.Composite);
                inExtent.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    inExtentA.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtentB.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                var extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C1one2manies, inExtent);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, true, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full Extent
                inExtent = this.Transaction.Extent(m.C1.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C1one2manies, inExtent);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, true, false, false, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                inExtent = this.Transaction.Extent(m.C2.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C2.Composite);
                    var inExtentB = this.Transaction.Extent(m.C2.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C2one2manies, inExtent);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, true, false, false, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                inExtent = this.Transaction.Extent(m.C4.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C4.Composite);
                    var inExtentB = this.Transaction.Extent(m.C4.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C3.Composite);
                extent.Filter.AddNot().AddIn(m.C3.C3C4one2manies, inExtent);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, true, false, false, true);
                this.AssertC4(extent, false, false, false, false);

                // In Extent over Interface
                // Emtpy Extent
                inExtent = this.Transaction.Extent(m.I12.Composite);
                inExtent.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C1one2manies, inExtent);

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, true, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full Extent
                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C1one2manies, inExtent);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, true, false, false, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C2one2manies, inExtent);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, true, false, false, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                inExtent = this.Transaction.Extent(m.I34.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I34.Composite);
                    var inExtentB = this.Transaction.Extent(m.I34.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C3.Composite);
                extent.Filter.AddNot().AddIn(m.C3.C3C4one2manies, inExtent);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, true, false, false, true);
                this.AssertC4(extent, false, false, false, false);

                // RelationType from Class to Interface

                // In Extent over Class
                inExtent = this.Transaction.Extent(m.C2.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C2.Composite);
                    var inExtentB = this.Transaction.Extent(m.C2.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.I12.I12C2one2manies, inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, true, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // In Extent over Interface
                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.I12.I12C2one2manies, inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, true, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);
            }
        }
    }

    [Fact]
    public void NotRoleOne2ManyInArray()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            foreach (var useOperator in this.UseOperator)
            {
                // Extent over Class
                // RelationType from Class to Class

                // In Extent over Class
                // Emtpy Extent
                var inExtent = this.Transaction.Extent(m.C1.Composite);
                inExtent.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    inExtentA.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtentB.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                var extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C1one2manies, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, true, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full Extent
                inExtent = this.Transaction.Extent(m.C1.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C1one2manies, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, true, false, false, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                inExtent = this.Transaction.Extent(m.C2.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C2.Composite);
                    var inExtentB = this.Transaction.Extent(m.C2.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C2one2manies, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, true, false, false, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                inExtent = this.Transaction.Extent(m.C4.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C4.Composite);
                    var inExtentB = this.Transaction.Extent(m.C4.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C3.Composite);
                extent.Filter.AddNot().AddIn(m.C3.C3C4one2manies, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, true, false, false, true);
                this.AssertC4(extent, false, false, false, false);

                // In Extent over Interface
                // Emtpy Extent
                inExtent = this.Transaction.Extent(m.I12.Composite);
                inExtent.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C1one2manies, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Equal(4, extent.Count);
                this.AssertC1(extent, true, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full Extent
                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C1one2manies, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, true, false, false, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C2one2manies, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, true, false, false, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                inExtent = this.Transaction.Extent(m.I34.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I34.Composite);
                    var inExtentB = this.Transaction.Extent(m.I34.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C3.Composite);
                extent.Filter.AddNot().AddIn(m.C3.C3C4one2manies, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, true, false, false, true);
                this.AssertC4(extent, false, false, false, false);

                // RelationType from Class to Interface

                // In Extent over Class
                inExtent = this.Transaction.Extent(m.C2.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C2.Composite);
                    var inExtentB = this.Transaction.Extent(m.C2.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.I12.I12C2one2manies, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, true, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // In Extent over Interface
                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.I12.I12C2one2manies, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, true, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);
            }
        }
    }

    [Fact]
    public void NotRoleOne2ManyContains()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddContains(m.C1.C1C2one2manies, this.c2C);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, true, true, false, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddContains(m.C1.C1I12one2manies, this.c2C);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, true, true, false, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddNot().AddContains(m.S1234.S1234one2manies, this.c1B);

            Assert.Equal(15, extent.Count);
            this.AssertC1(extent, true, false, true, true);
            this.AssertC2(extent, true, true, true, true);
            this.AssertC3(extent, true, true, true, true);
            this.AssertC4(extent, true, true, true, true);

            // TODO: wrong relation
        }
    }

    [Fact]
    public void NotRoleOne2ManyExist()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddExists(m.C1.C1C2one2manies);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, true, false, false, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddNot().AddExists(m.I12.I12C2one2manies);

            Assert.Equal(6, extent.Count);
            this.AssertC1(extent, true, false, true, true);
            this.AssertC2(extent, true, true, false, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddNot().AddExists(m.S1234.S1234C2one2manies);

            Assert.Equal(14, extent.Count);
            this.AssertC1(extent, true, false, true, true);
            this.AssertC2(extent, true, true, true, true);
            this.AssertC3(extent, true, true, false, true);
            this.AssertC4(extent, true, true, true, true);

            // Class - Wrong RelationType
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddNot().AddExists(m.C3.C3C2one2manies);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.I12.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddExists(m.C3.C3C2one2manies);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.S12.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddExists(m.C3.C3C2one2manies);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void NotRoleOne2OneIn()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            foreach (var useOperator in this.UseOperator)
            {
                // Extent over Class

                // RelationType from Class to Class

                // In Extent over Class
                var inExtent = this.Transaction.Extent(m.C1.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                var extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C1one2one, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                inExtent = this.Transaction.Extent(m.C2.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C2.Composite);
                    var inExtentB = this.Transaction.Extent(m.C2.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C2one2one, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                inExtent = this.Transaction.Extent(m.C4.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C4.Composite);
                    var inExtentB = this.Transaction.Extent(m.C4.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C3.Composite);
                extent.Filter.AddNot().AddIn(m.C3.C3C4one2one, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, true, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // In Extent over Interface
                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C1one2one, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C2one2one, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                inExtent = this.Transaction.Extent(m.I34.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I34.Composite);
                    var inExtentB = this.Transaction.Extent(m.I34.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C3.Composite);
                extent.Filter.AddNot().AddIn(m.C3.C3C4one2one, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, true, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // RelationType from Class to Interface

                // In Extent over Class
                inExtent = this.Transaction.Extent(m.C2.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C2.Composite);
                    var inExtentB = this.Transaction.Extent(m.C2.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1I12one2one, inExtent);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, true, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // In Extent over Interface
                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1I12one2one, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);
            }
        }
    }

    [Fact]
    public void NotRoleOne2OneInArray()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            foreach (var useOperator in this.UseOperator)
            {
                // Extent over Class

                // RelationType from Class to Class

                // In Extent over Class
                var inExtent = this.Transaction.Extent(m.C1.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                var extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C1one2one, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Single(extent);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                inExtent = this.Transaction.Extent(m.C2.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C2.Composite);
                    var inExtentB = this.Transaction.Extent(m.C2.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C2one2one, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Single(extent);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                inExtent = this.Transaction.Extent(m.C4.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C4.Composite);
                    var inExtentB = this.Transaction.Extent(m.C4.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C3.Composite);
                extent.Filter.AddNot().AddIn(m.C3.C3C4one2one, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Single(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, true, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // In Extent over Interface
                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C1one2one, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Single(extent);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C2one2one, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Single(extent);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                inExtent = this.Transaction.Extent(m.I34.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I34.Composite);
                    var inExtentB = this.Transaction.Extent(m.I34.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C3.Composite);
                extent.Filter.AddNot().AddIn(m.C3.C3C4one2one, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Single(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, true, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // RelationType from Class to Interface

                // In Extent over Class
                inExtent = this.Transaction.Extent(m.C2.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C2.Composite);
                    var inExtentB = this.Transaction.Extent(m.C2.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1I12one2one, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, true, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // In Extent over Interface
                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1I12one2one, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Single(extent);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);
            }
        }
    }

    [Fact]
    public void NotRoleOne2OneEquals()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddEquals(m.C1.C1C1one2one, this.c1B);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, true, false, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddEquals(m.C1.C1C2one2one, this.c2B);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, true, false, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddNot().AddEquals(m.I12.I12C2one2one, this.c2A);

            Assert.Equal(7, extent.Count);
            this.AssertC1(extent, true, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddNot().AddEquals(m.S1234.S1234C2one2one, this.c2A);

            Assert.Equal(15, extent.Count);
            this.AssertC1(extent, true, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, true, true, true, true);
            this.AssertC4(extent, true, true, true, true);

            // Class - Wrong RelationType
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddEquals(m.C3.C3C2one2one, this.c2A);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.I12.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.C3.C3C2one2one, this.c2A);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.S12.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.C3.C3C2one2one, this.c2A);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void NotRoleOne2OneExist()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddExists(m.C1.C1C2one2one);

            Assert.Single(extent);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddExists(m.C1.C1C2one2one);

            Assert.Single(extent);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddNot().AddExists(m.I12.I12C2one2one);

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddNot().AddExists(m.S1234.S1234C2one2one);

            Assert.Equal(12, extent.Count);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, true, true, true, true);
            this.AssertC4(extent, true, true, true, true);

            // Class - Wrong RelationType
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddNot().AddExists(m.C3.C3C2one2one);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.I12.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddExists(m.C3.C3C2one2one);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.S12.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddExists(m.C3.C3C2one2one);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void NotRoleOne2OneInstanceOf()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class

            // Class
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddInstanceOf(m.C1.C1C1one2one, m.C1.Composite);

            Assert.Single(extent);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddInstanceOf(m.C1.C1C2one2one, m.C2.Composite);

            Assert.Single(extent);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddInstanceOf(m.C1.C1I12one2one, m.C2.Composite);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, true, true, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddNot().AddInstanceOf(m.S1234.S1234one2one, m.C2.Composite);

            Assert.Equal(13, extent.Count);
            this.AssertC1(extent, true, true, false, true);
            this.AssertC2(extent, true, true, false, true);
            this.AssertC3(extent, true, true, false, true);
            this.AssertC4(extent, true, true, true, true);

            // Interface

            // Class
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddInstanceOf(m.C1.C1C2one2one, m.I2.Composite);

            Assert.Single(extent);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddInstanceOf(m.C1.C1I12one2one, m.I2.Composite);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, true, true, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddInstanceOf(m.C1.C1I12one2one, m.I12.Composite);

            Assert.Single(extent);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddNot().AddInstanceOf(m.S1234.S1234one2one, m.S1234.Composite);

            Assert.Equal(7, extent.Count);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, true, false, false, false);
            this.AssertC3(extent, true, false, false, false);
            this.AssertC4(extent, true, true, true, true);

            // TODO: wrong relation
        }
    }

    [Fact]
    public void NotRoleMany2OneIn()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            foreach (var useOperator in this.UseOperator)
            {
                // Extent over Class

                // RelationType from Class to Class

                // In Extent over Class
                var inExtent = this.Transaction.Extent(m.C1.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                var extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C1many2one, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                inExtent = this.Transaction.Extent(m.C2.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C2.Composite);
                    var inExtentB = this.Transaction.Extent(m.C2.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C2many2one, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                inExtent = this.Transaction.Extent(m.C4.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C4.Composite);
                    var inExtentB = this.Transaction.Extent(m.C4.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C3.Composite);
                extent.Filter.AddNot().AddIn(m.C3.C3C4many2one, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, true, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // In Extent over Interface
                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C1many2one, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C2many2one, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                inExtent = this.Transaction.Extent(m.I34.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I34.Composite);
                    var inExtentB = this.Transaction.Extent(m.I34.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C3.Composite);
                extent.Filter.AddNot().AddIn(m.C3.C3C4many2one, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, true, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // RelationType from Class to Interface

                // In Extent over Class
                inExtent = this.Transaction.Extent(m.C2.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C2.Composite);
                    var inExtentB = this.Transaction.Extent(m.C2.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1I12many2one, inExtent);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, true, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // In Extent over Interface
                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1I12many2one, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);
            }
        }
    }

    [Fact]
    public void NotRoleMany2OneInArray()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            foreach (var useOperator in this.UseOperator)
            {
                // Extent over Class

                // RelationType from Class to Class

                // In Extent over Class
                var inExtent = this.Transaction.Extent(m.C1.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                var extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C1many2one, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Single(extent);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                inExtent = this.Transaction.Extent(m.C2.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C2.Composite);
                    var inExtentB = this.Transaction.Extent(m.C2.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C2many2one, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Single(extent);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                inExtent = this.Transaction.Extent(m.C4.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C4.Composite);
                    var inExtentB = this.Transaction.Extent(m.C4.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C3.Composite);
                extent.Filter.AddNot().AddIn(m.C3.C3C4many2one, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Single(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, true, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // In Extent over Interface
                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C1many2one, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Single(extent);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1C2many2one, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Single(extent);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                inExtent = this.Transaction.Extent(m.I34.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I34.Composite);
                    var inExtentB = this.Transaction.Extent(m.I34.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C3.Composite);
                extent.Filter.AddNot().AddIn(m.C3.C3C4many2one, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Single(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, true, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // RelationType from Class to Interface

                // In Extent over Class
                inExtent = this.Transaction.Extent(m.C2.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C2.Composite);
                    var inExtentB = this.Transaction.Extent(m.C2.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1I12many2one, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, true, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // In Extent over Interface
                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddNot().AddIn(m.C1.C1I12many2one, (IEnumerable<IObject>)inExtent.ToArray());

                Assert.Single(extent);
                this.AssertC1(extent, true, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);
            }
        }
    }

    [Fact]
    public void NotRoleStringEqualsValue()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class

            // Equal ""
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddEquals(m.C1.C1AllorsString, string.Empty);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.C3.Composite);
            extent.Filter.AddNot().AddEquals(m.C3.C3AllorsString, string.Empty);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, true, true, true);
            this.AssertC4(extent, false, false, false, false);

            // Equal "ᴀbra"
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddEquals(m.C1.C1AllorsString, "ᴀbra");

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.C3.Composite);
            extent.Filter.AddNot().AddEquals(m.C3.C3AllorsString, "ᴀbra");

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, true, true);
            this.AssertC4(extent, false, false, false, false);

            // Equal "ᴀbracadabra"
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddEquals(m.C1.C1AllorsString, "ᴀbracadabra");

            Assert.Single(extent);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.C3.Composite);
            extent.Filter.AddNot().AddEquals(m.C3.C3AllorsString, "ᴀbracadabra");

            Assert.Single(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, true, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface

            // Equal ""
            extent = this.Transaction.Extent(m.I1.Composite);
            extent.Filter.AddNot().AddEquals(m.I1.I1AllorsString, string.Empty);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.I3.Composite);
            extent.Filter.AddNot().AddEquals(m.I3.I3AllorsString, string.Empty);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, true, true, true);
            this.AssertC4(extent, false, false, false, false);

            // Equal "ᴀbra"
            extent = this.Transaction.Extent(m.I1.Composite);
            extent.Filter.AddNot().AddEquals(m.I1.I1AllorsString, "ᴀbra");

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.I3.Composite);
            extent.Filter.AddNot().AddEquals(m.I3.I3AllorsString, "ᴀbra");

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, true, true);
            this.AssertC4(extent, false, false, false, false);

            // Equal "ᴀbracadabra"
            extent = this.Transaction.Extent(m.I1.Composite);
            extent.Filter.AddNot().AddEquals(m.I1.I1AllorsString, "ᴀbracadabra");

            Assert.Single(extent);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.I3.Composite);
            extent.Filter.AddNot().AddEquals(m.I3.I3AllorsString, "ᴀbracadabra");

            Assert.Single(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, true, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Shared Interface

            // Equal ""
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddNot().AddEquals(m.I12.I12AllorsString, string.Empty);

            Assert.Equal(6, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.I34.Composite);
            extent.Filter.AddNot().AddEquals(m.I34.I34AllorsString, string.Empty);

            Assert.Equal(6, extent.Count);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, true, true, true);
            this.AssertC4(extent, false, true, true, true);

            // Equal "ᴀbra"
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddNot().AddEquals(m.I12.I12AllorsString, "ᴀbra");

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddEquals(m.I12.I12AllorsString, "ᴀbra");

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.I23.Composite);
            extent.Filter.AddNot().AddEquals(m.I23.I23AllorsString, "ᴀbra");

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, true, true);
            this.AssertC3(extent, false, false, true, true);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.C2.Composite);
            extent.Filter.AddNot().AddEquals(m.I23.I23AllorsString, "ᴀbra");

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.C3.Composite);
            extent.Filter.AddNot().AddEquals(m.I23.I23AllorsString, "ᴀbra");

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, true, true);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.I34.Composite);
            extent.Filter.AddNot().AddEquals(m.I34.I34AllorsString, "ᴀbra");

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, true, true);
            this.AssertC4(extent, false, false, true, true);

            extent = this.Transaction.Extent(m.C3.Composite);
            extent.Filter.AddNot().AddEquals(m.I34.I34AllorsString, "ᴀbra");

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, true, true);
            this.AssertC4(extent, false, false, false, false);

            // Equal "ᴀbracadabra"
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddNot().AddEquals(m.I12.I12AllorsString, "ᴀbracadabra");

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddEquals(m.I12.I12AllorsString, "ᴀbracadabra");

            Assert.Single(extent);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.I34.Composite);
            extent.Filter.AddNot().AddEquals(m.I34.I34AllorsString, "ᴀbracadabra");

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, true, false, false);
            this.AssertC4(extent, false, true, false, false);

            // Super Interface

            // Equal ""
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddNot().AddEquals(m.S1234.S1234AllorsString, string.Empty);

            Assert.Equal(12, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, true, true, true);
            this.AssertC4(extent, false, true, true, true);

            // Equal "ᴀbra"
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddNot().AddEquals(m.S1234.S1234AllorsString, "ᴀbra");

            Assert.Equal(8, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, true, true);
            this.AssertC3(extent, false, false, true, true);
            this.AssertC4(extent, false, false, true, true);

            // Equal "ᴀbracadabra"
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddNot().AddEquals(m.S1234.S1234AllorsString, "ᴀbracadabra");

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, true, false, false);
            this.AssertC4(extent, false, true, false, false);

            // Class - Wrong RelationType

            // Equal ""
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddNot().AddEquals(m.C2.C2AllorsString, string.Empty);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Equal "ᴀbra"
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddEquals(m.C2.C2AllorsString, "ᴀbra");
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Equal "ᴀbracadabra"
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddEquals(m.C2.C2AllorsString, "ᴀbracadabra");
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType

            // Equal ""
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddEquals(m.I2.I2AllorsString, string.Empty);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Equal "ᴀbra"
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddEquals(m.I2.I2AllorsString, "ᴀbra");
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Equal "ᴀbracadabra"
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddEquals(m.I2.I2AllorsString, "ᴀbracadabra");
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType

            // Equal ""
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddEquals(m.S2.S2AllorsString, string.Empty);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Equal "ᴀbra"
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddEquals(m.S2.S2AllorsString, "ᴀbra");
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Equal "ᴀbracadabra"
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddEquals(m.S2.S2AllorsString, "ᴀbracadabra");
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void NotRoleStringExist()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddExists(m.C1.C1AllorsString);

            Assert.Single(extent);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddNot().AddExists(m.I12.I12AllorsString);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, true, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddNot().AddExists(m.S1234.S1234AllorsString);

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, true, false, false, false);
            this.AssertC2(extent, true, false, false, false);
            this.AssertC3(extent, true, false, false, false);
            this.AssertC4(extent, true, false, false, false);

            // Class - Wrong RelationType
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddNot().AddExists(m.C2.C2AllorsString);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddExists(m.I2.I2AllorsString);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddExists(m.S2.S2AllorsString);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void NotRoleStringLike()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class

            // Like ""
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddLike(m.C1.C1AllorsString, string.Empty);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Like "ᴀbra"
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddLike(m.C1.C1AllorsString, "ᴀbra");

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Like "ᴀbracadabra"
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddLike(m.C1.C1AllorsString, "ᴀbracadabra");

            Assert.Single(extent);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Like "notfound"
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddLike(m.C1.C1AllorsString, "notfound");

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Like "%ra%"
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddLike(m.C1.C1AllorsString, "%ra%");

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Like "%bra"
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddLike(m.C1.C1AllorsString, "%bra");

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Like "%cadabra"
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddNot().AddLike(m.C1.C1AllorsString, "%cadabra");

            Assert.Single(extent);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface

            // Like ""
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddNot().AddLike(m.I12.I12AllorsString, string.Empty);

            Assert.Equal(6, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Like "ᴀbra"
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddNot().AddLike(m.I12.I12AllorsString, "ᴀbra");

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Like "ᴀbracadabra"
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddNot().AddLike(m.I12.I12AllorsString, "ᴀbracadabra");

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface

            // Like ""
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddNot().AddLike(m.S1234.S1234AllorsString, string.Empty);

            Assert.Equal(12, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, true, true, true);
            this.AssertC4(extent, false, true, true, true);

            // Like "ᴀbra"
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddNot().AddLike(m.S1234.S1234AllorsString, "ᴀbra");

            Assert.Equal(8, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, true, true);
            this.AssertC3(extent, false, false, true, true);
            this.AssertC4(extent, false, false, true, true);

            // Like "ᴀbracadabra"
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddNot().AddLike(m.S1234.S1234AllorsString, "ᴀbracadabra");

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC1(extent, false, true, false, false);

            // Class - Wrong RelationType

            // Like ""
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddNot().AddLike(m.C2.C2AllorsString, string.Empty);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Like "ᴀbra"
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddLike(m.C2.C2AllorsString, "ᴀbra");
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Like "ᴀbracadabra"
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddLike(m.C2.C2AllorsString, "ᴀbracadabra");
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType

            // Like ""
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddLike(m.I2.I2AllorsString, string.Empty);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Like "ᴀbra"
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddLike(m.I2.I2AllorsString, "ᴀbra");
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Like "ᴀbracadabra"
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddLike(m.I2.I2AllorsString, "ᴀbracadabra");
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType

            // Like ""
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddLike(m.S2.S2AllorsString, string.Empty);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Like "ᴀbra"
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddLike(m.S2.S2AllorsString, "ᴀbra");
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Like "ᴀbracadabra"
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddNot().AddLike(m.S2.S2AllorsString, "ᴀbracadabra");
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public virtual void Operation()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            var firstExtent = this.Transaction.Extent(m.C1.Composite);
            firstExtent.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");

            var secondExtent = this.Transaction.Extent(m.C1.Composite);
            secondExtent.Filter.AddLike(m.C1.C1AllorsString, "ᴀbracadabra");

            var extent = this.Transaction.Union(firstExtent, secondExtent);

            Assert.Equal(3, extent.Count);

            firstExtent.Filter.AddEquals(m.C1.C1AllorsString, "Oops");

            Assert.Equal(2, extent.Count);

            secondExtent.Filter.AddEquals(m.C1.C1AllorsString, "I did it again");

            Assert.Empty(extent);

            // TODO: all possible combinations
        }
    }

    [Fact]
    public void Optimizations()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Dangling empty And behind Or
            var extent = this.Transaction.Extent(m.C1.Composite);
            var or = extent.Filter.AddOr();

            or.AddAnd();
            or.AddLessThan(m.C1.C1AllorsInteger, 2);

            Assert.Single(extent);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Dangling empty Or behind Or
            extent = this.Transaction.Extent(m.C1.Composite);
            or = extent.Filter.AddOr();

            or.AddOr();
            or.AddLessThan(m.C1.C1AllorsInteger, 2);

            Assert.Single(extent);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Dangling empty Not behind Or
            extent = this.Transaction.Extent(m.C1.Composite);
            or = extent.Filter.AddOr();

            or.AddNot();
            or.AddLessThan(m.C1.C1AllorsInteger, 2);

            Assert.Single(extent);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Dangling empty And behind And
            extent = this.Transaction.Extent(m.C1.Composite);
            var and = extent.Filter.AddAnd();

            and.AddAnd();
            and.AddLessThan(m.C1.C1AllorsInteger, 2);

            Assert.Single(extent);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Dangling empty Or behind And
            extent = this.Transaction.Extent(m.C1.Composite);
            and = extent.Filter.AddAnd();

            and.AddOr();
            and.AddLessThan(m.C1.C1AllorsInteger, 2);

            Assert.Single(extent);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Dangling empty Not behind And
            extent = this.Transaction.Extent(m.C1.Composite);
            and = extent.Filter.AddAnd();

            and.AddNot();
            and.AddLessThan(m.C1.C1AllorsInteger, 2);

            Assert.Single(extent);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Dangling empty And
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddAnd();

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, true, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);
        }
    }

    [Fact]
    public void Or()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class
            var extent = this.Transaction.Extent(m.C1.Composite);
            var any = extent.Filter.AddOr();
            any.AddGreaterThan(m.C1.C1AllorsInteger, 0);
            any.AddLessThan(m.C1.C1AllorsInteger, 3);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface
            extent = this.Transaction.Extent(m.I12.Composite);
            {
                var or = extent.Filter.AddOr();
                or.AddGreaterThan(m.I12.I12AllorsInteger, 0);
                or.AddLessThan(m.I12.I12AllorsInteger, 3);
            }

            Assert.Equal(6, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            {
                var or = extent.Filter.AddOr();
                or.AddGreaterThan(m.S1234.S1234AllorsInteger, 0);
                or.AddLessThan(m.S1234.S1234AllorsInteger, 3);
            }

            Assert.Equal(12, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, true, true, true);
            this.AssertC4(extent, false, true, true, true);

            // Class Without predicates
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddOr();

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, true, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);
        }
    }

    [Fact]
    public void OrIn()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Association (Amgiguous Name)
            Extent<Company> parents = this.Transaction.Extent(m.Company.Composite);

            Extent<Company> children = this.Transaction.Extent(m.Company.Composite);
            children.Filter.AddIn(m.Company.CompanyWhereChild, parents);

            Extent<Person> persons = this.Transaction.Extent(m.Person.Composite);
            var or = persons.Filter.AddOr();
            or.AddIn(m.Person.Company, parents);
            or.AddIn(m.Person.Company, children);

            Assert.Empty(persons);
        }
    }

    [Fact]
    public void RoleIntegerExist()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddExists(m.C1.C1AllorsInteger);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddExists(m.I12.I12AllorsInteger);

            Assert.Equal(6, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddExists(m.S1234.S1234AllorsInteger);

            Assert.Equal(12, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, true, true, true);
            this.AssertC4(extent, false, true, true, true);

            // Class - Wrong RelationType
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddExists(m.C2.C2AllorsInteger);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddExists(m.I2.I2AllorsInteger);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddExists(m.S2.S2AllorsInteger);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void RoleIntegerBetweenRole()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class

            // Between C1
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddBetween(m.C1.C1AllorsInteger, m.C1.C1IntegerBetweenA, m.C1.C1IntegerBetweenB);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // TODO: Greater than Role
            // Interface

            // Between -10 and 0
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddBetween(m.I12.I12AllorsInteger, -10, 0);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Between 0 and 1
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddBetween(m.I12.I12AllorsInteger, 0, 1);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Between 1 and 2
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddBetween(m.I12.I12AllorsInteger, 1, 2);

            Assert.Equal(6, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Between 3 and 10
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddBetween(m.I12.I12AllorsInteger, 3, 10);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface

            // Between -10 and 0
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddBetween(m.S1234.S1234AllorsInteger, -10, 0);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Between 0 and 1
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddBetween(m.S1234.S1234AllorsInteger, 0, 1);

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, true, false, false);
            this.AssertC4(extent, false, true, false, false);

            // Between 1 and 2
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddBetween(m.S1234.S1234AllorsInteger, 1, 2);

            Assert.Equal(12, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, true, true, true);
            this.AssertC4(extent, false, true, true, true);

            // Between 3 and 10
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddBetween(m.S1234.S1234AllorsInteger, 3, 10);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Class - Wrong RelationType

            // Between -10 and 0
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddBetween(m.C2.C2AllorsInteger, -10, 0);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Between 0 and 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddBetween(m.C2.C2AllorsInteger, 0, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Between 1 and 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddBetween(m.C2.C2AllorsInteger, 1, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Between 3 and 10
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddBetween(m.C2.C2AllorsInteger, 3, 10);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void RoleIntegerLessThanRole()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class

            // Less Than 1
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddLessThan(m.C1.C1AllorsInteger, m.C1.C1IntegerLessThan);

            Assert.Single(extent);
            this.AssertC1(extent, false, false, false, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // TODO: Less than Role
            // Interface

            // Less Than 1
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddLessThan(m.I12.I12AllorsInteger, 1);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Less Than 2
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddLessThan(m.I12.I12AllorsInteger, 2);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Less Than 3
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddLessThan(m.I12.I12AllorsInteger, 3);

            Assert.Equal(6, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface

            // Less Than 1
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddLessThan(m.S1234.S1234AllorsInteger, 1);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Less Than 2
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddLessThan(m.S1234.S1234AllorsInteger, 2);

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, true, false, false);
            this.AssertC4(extent, false, true, false, false);

            // Less Than 3
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddLessThan(m.S1234.S1234AllorsInteger, 3);

            Assert.Equal(12, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, true, true, true);
            this.AssertC4(extent, false, true, true, true);

            // Class - Wrong RelationType

            // Less Than 1
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddLessThan(m.C2.C2AllorsInteger, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Less Than 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLessThan(m.C2.C2AllorsInteger, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Less Than 3
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLessThan(m.C2.C2AllorsInteger, 3);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType

            // Less Than 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLessThan(m.I2.I2AllorsInteger, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Less Than 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLessThan(m.I2.I2AllorsInteger, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Less Than 3
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLessThan(m.I2.I2AllorsInteger, 3);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType

            // Less Than 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLessThan(m.S2.S2AllorsInteger, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Less Than 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLessThan(m.S2.S2AllorsInteger, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Less Than 3
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLessThan(m.S2.S2AllorsInteger, 3);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void RoleIntegerGreaterThanRole()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class

            // C1
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddGreaterThan(m.C1.C1AllorsInteger, m.C1.C1IntegerGreaterThan);

            Assert.Single(extent);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // TODO: Greater than Role
            // Interface

            // Greater Than 0
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddGreaterThan(m.I12.I12AllorsInteger, 0);

            Assert.Equal(6, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Greater Than 1
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddGreaterThan(m.I12.I12AllorsInteger, 1);

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Greater Than 2
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddGreaterThan(m.I12.I12AllorsInteger, 2);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface

            // Greater Than 0
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddGreaterThan(m.S1234.S1234AllorsInteger, 0);

            Assert.Equal(12, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, true, true, true);
            this.AssertC4(extent, false, true, true, true);

            // Greater Than 1
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddGreaterThan(m.S1234.S1234AllorsInteger, 1);

            Assert.Equal(8, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, true, true);
            this.AssertC3(extent, false, false, true, true);
            this.AssertC4(extent, false, false, true, true);

            // Greater Than 2
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddGreaterThan(m.S1234.S1234AllorsInteger, 2);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Class - Wrong RelationType

            // Greater Than 0
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddGreaterThan(m.C2.C2AllorsInteger, 0);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Greater Than 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddGreaterThan(m.C2.C2AllorsInteger, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Greater Than 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddGreaterThan(m.C2.C2AllorsInteger, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType

            // Greater Than 0
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddGreaterThan(m.I2.I2AllorsInteger, 0);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Greater Than 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddGreaterThan(m.I2.I2AllorsInteger, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Greater Than 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddGreaterThan(m.I2.I2AllorsInteger, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType

            // Greater Than 0
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddGreaterThan(m.I2.I2AllorsInteger, 0);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Greater Than 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddGreaterThan(m.I2.I2AllorsInteger, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Greater Than 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddGreaterThan(m.I2.I2AllorsInteger, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void RoleIntegerBetweenValue()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class

            // Between -10 and 0
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddBetween(m.C1.C1AllorsInteger, -10, 0);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Between 0 and 1
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddBetween(m.C1.C1AllorsInteger, 0, 1);

            Assert.Single(extent);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Between 1 and 2
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddBetween(m.C1.C1AllorsInteger, 1, 2);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Between 3 and 10
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddBetween(m.C1.C1AllorsInteger, 3, 10);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface

            // Between -10 and 0
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddBetween(m.I12.I12AllorsInteger, -10, 0);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Between 0 and 1
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddBetween(m.I12.I12AllorsInteger, 0, 1);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Between 1 and 2
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddBetween(m.I12.I12AllorsInteger, 1, 2);

            Assert.Equal(6, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Between 3 and 10
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddBetween(m.I12.I12AllorsInteger, 3, 10);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface

            // Between -10 and 0
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddBetween(m.S1234.S1234AllorsInteger, -10, 0);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Between 0 and 1
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddBetween(m.S1234.S1234AllorsInteger, 0, 1);

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, true, false, false);
            this.AssertC4(extent, false, true, false, false);

            // Between 1 and 2
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddBetween(m.S1234.S1234AllorsInteger, 1, 2);

            Assert.Equal(12, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, true, true, true);
            this.AssertC4(extent, false, true, true, true);

            // Between 3 and 10
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddBetween(m.S1234.S1234AllorsInteger, 3, 10);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Class - Wrong RelationType

            // Between -10 and 0
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddBetween(m.C2.C2AllorsInteger, -10, 0);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Between 0 and 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddBetween(m.C2.C2AllorsInteger, 0, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Between 1 and 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddBetween(m.C2.C2AllorsInteger, 1, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Between 3 and 10
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddBetween(m.C2.C2AllorsInteger, 3, 10);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void RoleIntegerLessThanValue()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class

            // Less Than 1
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddLessThan(m.C1.C1AllorsInteger, 1);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Less Than 2
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddLessThan(m.C1.C1AllorsInteger, 2);

            Assert.Single(extent);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Less Than 3
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddLessThan(m.C1.C1AllorsInteger, 3);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface

            // Less Than 1
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddLessThan(m.I12.I12AllorsInteger, 1);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Less Than 2
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddLessThan(m.I12.I12AllorsInteger, 2);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Less Than 3
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddLessThan(m.I12.I12AllorsInteger, 3);

            Assert.Equal(6, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface

            // Less Than 1
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddLessThan(m.S1234.S1234AllorsInteger, 1);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Less Than 2
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddLessThan(m.S1234.S1234AllorsInteger, 2);

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, true, false, false);
            this.AssertC4(extent, false, true, false, false);

            // Less Than 3
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddLessThan(m.S1234.S1234AllorsInteger, 3);

            Assert.Equal(12, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, true, true, true);
            this.AssertC4(extent, false, true, true, true);

            // Class - Wrong RelationType

            // Less Than 1
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddLessThan(m.C2.C2AllorsInteger, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Less Than 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLessThan(m.C2.C2AllorsInteger, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Less Than 3
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLessThan(m.C2.C2AllorsInteger, 3);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType

            // Less Than 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLessThan(m.I2.I2AllorsInteger, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Less Than 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLessThan(m.I2.I2AllorsInteger, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Less Than 3
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLessThan(m.I2.I2AllorsInteger, 3);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType

            // Less Than 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLessThan(m.S2.S2AllorsInteger, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Less Than 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLessThan(m.S2.S2AllorsInteger, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Less Than 3
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLessThan(m.S2.S2AllorsInteger, 3);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void RoleIntegerGreaterThanValue()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class

            // Greater Than 0
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddGreaterThan(m.C1.C1AllorsInteger, 0);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Greater Than 1
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddGreaterThan(m.C1.C1AllorsInteger, 1);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Greater Than 2
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddGreaterThan(m.C1.C1AllorsInteger, 2);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface
            // Greater Than 0
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddGreaterThan(m.I12.I12AllorsInteger, 0);

            Assert.Equal(6, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Greater Than 1
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddGreaterThan(m.I12.I12AllorsInteger, 1);

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Greater Than 2
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddGreaterThan(m.I12.I12AllorsInteger, 2);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface
            // Greater Than 0
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddGreaterThan(m.S1234.S1234AllorsInteger, 0);

            Assert.Equal(12, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, true, true, true);
            this.AssertC4(extent, false, true, true, true);

            // Greater Than 1
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddGreaterThan(m.S1234.S1234AllorsInteger, 1);

            Assert.Equal(8, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, true, true);
            this.AssertC3(extent, false, false, true, true);
            this.AssertC4(extent, false, false, true, true);

            // Greater Than 2
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddGreaterThan(m.S1234.S1234AllorsInteger, 2);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Class - Wrong RelationType
            // Greater Than 0
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddGreaterThan(m.C2.C2AllorsInteger, 0);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Greater Than 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddGreaterThan(m.C2.C2AllorsInteger, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Greater Than 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddGreaterThan(m.C2.C2AllorsInteger, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType
            // Greater Than 0
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddGreaterThan(m.I2.I2AllorsInteger, 0);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Greater Than 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddGreaterThan(m.I2.I2AllorsInteger, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Greater Than 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddGreaterThan(m.I2.I2AllorsInteger, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType
            // Greater Than 0
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddGreaterThan(m.I2.I2AllorsInteger, 0);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Greater Than 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddGreaterThan(m.I2.I2AllorsInteger, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Greater Than 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddGreaterThan(m.I2.I2AllorsInteger, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void RoleIntegerEquals()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class
            // Equal 0
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddEquals(m.C1.C1AllorsInteger, 0);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Equal 1
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddEquals(m.C1.C1AllorsInteger, 1);

            Assert.Single(extent);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Equal 2
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddEquals(m.C1.C1AllorsInteger, 2);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface
            // Equal 0
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddEquals(m.I12.I12AllorsInteger, 0);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Equal 1
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddEquals(m.I12.I12AllorsInteger, 1);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Equal 2
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddEquals(m.I12.I12AllorsInteger, 2);

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface
            // Equal 0
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddEquals(m.S1234.S1234AllorsInteger, 0);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Equal 1
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddEquals(m.S1234.S1234AllorsInteger, 1);

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, true, false, false);
            this.AssertC4(extent, false, true, false, false);

            // Equal 2
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddEquals(m.S1234.S1234AllorsInteger, 2);

            Assert.Equal(8, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, true, true);
            this.AssertC3(extent, false, false, true, true);
            this.AssertC4(extent, false, false, true, true);

            // Class - Wrong RelationType
            // Equal 0
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddEquals(m.C2.C2AllorsInteger, 0);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Equal 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.C2.C2AllorsInteger, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Equal 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.C2.C2AllorsInteger, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType
            // Equal 0
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.I2.I2AllorsInteger, 0);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Equal 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.I2.I2AllorsInteger, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Equal 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.I2.I2AllorsInteger, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType
            // Equal 0
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.S2.S2AllorsInteger, 0);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Equal 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.S2.S2AllorsInteger, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Equal 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.S2.S2AllorsInteger, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void RoleFloatBetweenValue()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class

            // Between -10 and 0
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddBetween(m.C1.C1AllorsDouble, -10, 0);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Between 0 and 1
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddBetween(m.C1.C1AllorsDouble, 0, 1);

            Assert.Single(extent);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Between 1 and 2
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddBetween(m.C1.C1AllorsDouble, 1, 2);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Between 3 and 10
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddBetween(m.C1.C1AllorsDouble, 3, 10);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface

            // Between -10 and 0
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddBetween(m.I12.I12AllorsDouble, -10, 0);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Between 0 and 1
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddBetween(m.I12.I12AllorsDouble, 0, 1);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Between 1 and 2
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddBetween(m.I12.I12AllorsDouble, 1, 2);

            Assert.Equal(6, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Between 3 and 10
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddBetween(m.I12.I12AllorsDouble, 3, 10);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface

            // Between -10 and 0
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddBetween(m.S1234.S1234AllorsDouble, -10, 0);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Between 0 and 1
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddBetween(m.S1234.S1234AllorsDouble, 0, 1);

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, true, false, false);
            this.AssertC4(extent, false, true, false, false);

            // Between 1 and 2
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddBetween(m.S1234.S1234AllorsDouble, 1, 2);

            Assert.Equal(12, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, true, true, true);
            this.AssertC4(extent, false, true, true, true);

            // Between 3 and 10
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddBetween(m.S1234.S1234AllorsDouble, 3, 10);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Class - Wrong RelationType

            // Between -10 and 0
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddBetween(m.C2.C2AllorsDouble, -10, 0);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Between 0 and 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddBetween(m.C2.C2AllorsDouble, 0, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Between 1 and 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddBetween(m.C2.C2AllorsDouble, 1, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Between 3 and 10
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddBetween(m.C2.C2AllorsDouble, 3, 10);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void RoleFloatLessThanValue()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class

            // Less Than 1
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddLessThan(m.C1.C1AllorsDouble, 1);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Less Than 2
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddLessThan(m.C1.C1AllorsDouble, 2);

            Assert.Single(extent);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Less Than 3
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddLessThan(m.C1.C1AllorsDouble, 3);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface

            // Less Than 1
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddLessThan(m.I12.I12AllorsDouble, 1);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Less Than 2
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddLessThan(m.I12.I12AllorsDouble, 2);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Less Than 3
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddLessThan(m.I12.I12AllorsDouble, 3);

            Assert.Equal(6, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface

            // Less Than 1
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddLessThan(m.S1234.S1234AllorsDouble, 1);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Less Than 2
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddLessThan(m.S1234.S1234AllorsDouble, 2);

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, true, false, false);
            this.AssertC4(extent, false, true, false, false);

            // Less Than 3
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddLessThan(m.S1234.S1234AllorsDouble, 3);

            Assert.Equal(12, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, true, true, true);
            this.AssertC4(extent, false, true, true, true);

            // Class - Wrong RelationType

            // Less Than 1
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddLessThan(m.C2.C2AllorsDouble, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Less Than 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLessThan(m.C2.C2AllorsDouble, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Less Than 3
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLessThan(m.C2.C2AllorsDouble, 3);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType

            // Less Than 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLessThan(m.I2.I2AllorsDouble, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Less Than 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLessThan(m.I2.I2AllorsDouble, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Less Than 3
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLessThan(m.I2.I2AllorsDouble, 3);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType

            // Less Than 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLessThan(m.S2.S2AllorsDouble, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Less Than 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLessThan(m.S2.S2AllorsDouble, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Less Than 3
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLessThan(m.S2.S2AllorsDouble, 3);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void RoleFloatGreaterThanValue()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class

            // Greater Than 0
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddGreaterThan(m.C1.C1AllorsDouble, 0);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Greater Than 1
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddGreaterThan(m.C1.C1AllorsDouble, 1);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Greater Than 2
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddGreaterThan(m.C1.C1AllorsDouble, 2);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface

            // Greater Than 0
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddGreaterThan(m.I12.I12AllorsDouble, 0);

            Assert.Equal(6, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Greater Than 1
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddGreaterThan(m.I12.I12AllorsDouble, 1);

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Greater Than 2
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddGreaterThan(m.I12.I12AllorsDouble, 2);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface

            // Greater Than 0
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddGreaterThan(m.S1234.S1234AllorsDouble, 0);

            Assert.Equal(12, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, true, true, true);
            this.AssertC4(extent, false, true, true, true);

            // Greater Than 1
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddGreaterThan(m.S1234.S1234AllorsDouble, 1);

            Assert.Equal(8, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, true, true);
            this.AssertC3(extent, false, false, true, true);
            this.AssertC4(extent, false, false, true, true);

            // Greater Than 2
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddGreaterThan(m.S1234.S1234AllorsDouble, 2);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Class - Wrong RelationType

            // Greater Than 0
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddGreaterThan(m.C2.C2AllorsDouble, 0);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Greater Than 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddGreaterThan(m.C2.C2AllorsDouble, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Greater Than 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddGreaterThan(m.C2.C2AllorsDouble, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType

            // Greater Than 0
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddGreaterThan(m.I2.I2AllorsDouble, 0);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Greater Than 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddGreaterThan(m.I2.I2AllorsDouble, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Greater Than 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddGreaterThan(m.I2.I2AllorsDouble, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType

            // Greater Than 0
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddGreaterThan(m.I2.I2AllorsDouble, 0);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Greater Than 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddGreaterThan(m.I2.I2AllorsDouble, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Greater Than 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddGreaterThan(m.I2.I2AllorsDouble, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void RoleFloatEquals()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class
            // Equal 0
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddEquals(m.C1.C1AllorsDouble, 0);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Equal 1
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddEquals(m.C1.C1AllorsDouble, 1);

            Assert.Single(extent);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Equal 2
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddEquals(m.C1.C1AllorsDouble, 2);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface
            // Equal 0
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddEquals(m.I12.I12AllorsDouble, 0);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Equal 1
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddEquals(m.I12.I12AllorsDouble, 1);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Equal 2
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddEquals(m.I12.I12AllorsDouble, 2);

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface
            // Equal 0
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddEquals(m.S1234.S1234AllorsDouble, 0);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Equal 1
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddEquals(m.S1234.S1234AllorsDouble, 1);

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, true, false, false);
            this.AssertC4(extent, false, true, false, false);

            // Equal 2
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddEquals(m.S1234.S1234AllorsDouble, 2);

            Assert.Equal(8, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, true, true);
            this.AssertC3(extent, false, false, true, true);
            this.AssertC4(extent, false, false, true, true);

            // Class - Wrong RelationType
            // Equal 0
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddEquals(m.C2.C2AllorsDouble, 0);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Equal 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.C2.C2AllorsDouble, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Equal 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.C2.C2AllorsDouble, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType
            // Equal 0
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.I2.I2AllorsDouble, 0);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Equal 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.I2.I2AllorsDouble, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Equal 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.I2.I2AllorsDouble, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType
            // Equal 0
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.S2.S2AllorsDouble, 0);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Equal 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.S2.S2AllorsDouble, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Equal 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.S2.S2AllorsDouble, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void RoleDateTimeBetweenValue()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            foreach (var flag in TrueFalse)
            {
                var dateTime1 = new DateTime(2000, 1, 1, 0, 0, 1, DateTimeKind.Utc);
                var dateTime2 = new DateTime(2000, 1, 1, 0, 0, 2, DateTimeKind.Utc);
                var dateTime3 = new DateTime(2000, 1, 1, 0, 0, 3, DateTimeKind.Utc);
                var dateTime4 = new DateTime(2000, 1, 1, 0, 0, 4, DateTimeKind.Utc);
                var dateTime5 = new DateTime(2000, 1, 1, 0, 0, 5, DateTimeKind.Utc);
                var dateTime6 = new DateTime(2000, 1, 1, 0, 0, 6, DateTimeKind.Utc);
                var dateTime7 = new DateTime(2000, 1, 1, 0, 0, 7, DateTimeKind.Utc);
                var dateTime10 = new DateTime(2000, 1, 1, 0, 0, 10, DateTimeKind.Utc);

                if (flag)
                {
                    dateTime1 = dateTime1.ToLocalTime();
                    dateTime2 = dateTime2.ToLocalTime();
                    dateTime3 = dateTime3.ToLocalTime();
                    dateTime4 = dateTime4.ToLocalTime();
                    dateTime5 = dateTime5.ToLocalTime();
                    dateTime6 = dateTime6.ToLocalTime();
                    dateTime7 = dateTime7.ToLocalTime();
                    dateTime10 = dateTime10.ToLocalTime();
                }

                // Class
                // Between 1 and 3
                var extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddBetween(m.C1.C1AllorsDateTime, dateTime1, dateTime3);

                Assert.Empty(extent);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Between 3 and 4
                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddBetween(m.C1.C1AllorsDateTime, dateTime3, dateTime4);

                Assert.Single(extent);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Between 4 and 5
                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddBetween(m.C1.C1AllorsDateTime, dateTime4, dateTime5);

                Assert.Equal(3, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.True(extent.Contains(this.c1C));
                Assert.True(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Between 6 and 10
                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddBetween(m.C1.C1AllorsDateTime, dateTime6, dateTime10);

                Assert.Empty(extent);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Interface
                // Between 1 and 3
                extent = this.Transaction.Extent(m.I12.Composite);
                extent.Filter.AddBetween(m.I12.I12AllorsDateTime, dateTime1, dateTime3);

                Assert.Empty(extent);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Between 3 and 4
                extent = this.Transaction.Extent(m.I12.Composite);
                extent.Filter.AddBetween(m.I12.I12AllorsDateTime, dateTime3, dateTime4);

                Assert.Equal(2, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Between 4 and 5
                extent = this.Transaction.Extent(m.I12.Composite);
                extent.Filter.AddBetween(m.I12.I12AllorsDateTime, dateTime4, dateTime5);

                Assert.Equal(6, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.True(extent.Contains(this.c1C));
                Assert.True(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.True(extent.Contains(this.c2C));
                Assert.True(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Between 6 and 10
                extent = this.Transaction.Extent(m.I12.Composite);
                extent.Filter.AddBetween(m.I12.I12AllorsDateTime, dateTime6, dateTime10);

                Assert.Empty(extent);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Super Interface
                // Between 1 and 3
                extent = this.Transaction.Extent(m.S1234.Composite);
                extent.Filter.AddBetween(m.S1234.S1234AllorsDateTime, dateTime1, dateTime3);

                Assert.Empty(extent);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Between 3 and 4
                extent = this.Transaction.Extent(m.S1234.Composite);
                extent.Filter.AddBetween(m.S1234.S1234AllorsDateTime, dateTime3, dateTime4);

                Assert.Equal(4, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.True(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.True(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Between 4 and 5
                extent = this.Transaction.Extent(m.S1234.Composite);
                extent.Filter.AddBetween(m.S1234.S1234AllorsDateTime, dateTime4, dateTime5);

                Assert.Equal(12, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.True(extent.Contains(this.c1C));
                Assert.True(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.True(extent.Contains(this.c2C));
                Assert.True(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.True(extent.Contains(this.c3B));
                Assert.True(extent.Contains(this.c3C));
                Assert.True(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.True(extent.Contains(this.c4B));
                Assert.True(extent.Contains(this.c4C));
                Assert.True(extent.Contains(this.c4D));

                // Between 6 and 10
                extent = this.Transaction.Extent(m.S1234.Composite);
                extent.Filter.AddBetween(m.S1234.S1234AllorsDateTime, dateTime6, dateTime10);

                Assert.Empty(extent);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Class - Wrong RelationType0
                // Between 1 and 3
                extent = this.Transaction.Extent(m.C1.Composite);

                var exception = false;
                try
                {
                    extent.Filter.AddBetween(m.C2.C2AllorsDateTime, dateTime1, dateTime3);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Between 3 and 4
                extent = this.Transaction.Extent(m.C1.Composite);

                exception = false;
                try
                {
                    extent.Filter.AddBetween(m.C2.C2AllorsDateTime, dateTime3, dateTime4);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Between 4 and 5
                extent = this.Transaction.Extent(m.C1.Composite);

                exception = false;
                try
                {
                    extent.Filter.AddBetween(m.C2.C2AllorsDateTime, dateTime4, dateTime5);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Between 6 and 10
                extent = this.Transaction.Extent(m.C1.Composite);

                exception = false;
                try
                {
                    extent.Filter.AddBetween(m.C2.C2AllorsDateTime, dateTime6, dateTime10);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }
    }

    [Fact]
    public void RoleDateTimeLessThanValue()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            foreach (var flag in TrueFalse)
            {
                var dateTime1 = new DateTime(2000, 1, 1, 0, 0, 1, DateTimeKind.Utc);
                var dateTime2 = new DateTime(2000, 1, 1, 0, 0, 2, DateTimeKind.Utc);
                var dateTime3 = new DateTime(2000, 1, 1, 0, 0, 3, DateTimeKind.Utc);
                var dateTime4 = new DateTime(2000, 1, 1, 0, 0, 4, DateTimeKind.Utc);
                var dateTime5 = new DateTime(2000, 1, 1, 0, 0, 5, DateTimeKind.Utc);
                var dateTime6 = new DateTime(2000, 1, 1, 0, 0, 6, DateTimeKind.Utc);
                var dateTime7 = new DateTime(2000, 1, 1, 0, 0, 7, DateTimeKind.Utc);
                var dateTime10 = new DateTime(2000, 1, 1, 0, 0, 10, DateTimeKind.Utc);

                if (flag)
                {
                    dateTime1 = dateTime1.ToLocalTime();
                    dateTime2 = dateTime2.ToLocalTime();
                    dateTime3 = dateTime3.ToLocalTime();
                    dateTime4 = dateTime4.ToLocalTime();
                    dateTime5 = dateTime5.ToLocalTime();
                    dateTime6 = dateTime6.ToLocalTime();
                    dateTime7 = dateTime7.ToLocalTime();
                    dateTime10 = dateTime10.ToLocalTime();
                }

                // Class
                // Less Than 4
                var extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddLessThan(m.C1.C1AllorsDateTime, dateTime4);

                Assert.Empty(extent);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Less Than 5
                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddLessThan(m.C1.C1AllorsDateTime, dateTime5);

                Assert.Single(extent);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Less Than 6
                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddLessThan(m.C1.C1AllorsDateTime, dateTime6);

                Assert.Equal(3, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.True(extent.Contains(this.c1C));
                Assert.True(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Interface
                // Less Than 4
                extent = this.Transaction.Extent(m.I12.Composite);
                extent.Filter.AddLessThan(m.I12.I12AllorsDateTime, dateTime4);

                Assert.Empty(extent);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Less Than 5
                extent = this.Transaction.Extent(m.I12.Composite);
                extent.Filter.AddLessThan(m.I12.I12AllorsDateTime, dateTime5);

                Assert.Equal(2, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Less Than 6
                extent = this.Transaction.Extent(m.I12.Composite);
                extent.Filter.AddLessThan(m.I12.I12AllorsDateTime, dateTime6);

                Assert.Equal(6, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.True(extent.Contains(this.c1C));
                Assert.True(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.True(extent.Contains(this.c2C));
                Assert.True(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Super Interface
                // Less Than 4
                extent = this.Transaction.Extent(m.S1234.Composite);
                extent.Filter.AddLessThan(m.S1234.S1234AllorsDateTime, dateTime4);

                Assert.Empty(extent);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Less Than 5
                extent = this.Transaction.Extent(m.S1234.Composite);
                extent.Filter.AddLessThan(m.S1234.S1234AllorsDateTime, dateTime5);

                Assert.Equal(4, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.True(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.True(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Less Than 6
                extent = this.Transaction.Extent(m.S1234.Composite);
                extent.Filter.AddLessThan(m.S1234.S1234AllorsDateTime, dateTime6);

                Assert.Equal(12, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.True(extent.Contains(this.c1C));
                Assert.True(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.True(extent.Contains(this.c2C));
                Assert.True(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.True(extent.Contains(this.c3B));
                Assert.True(extent.Contains(this.c3C));
                Assert.True(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.True(extent.Contains(this.c4B));
                Assert.True(extent.Contains(this.c4C));
                Assert.True(extent.Contains(this.c4D));

                // Class - Wrong RelationType

                // Less Than 4
                extent = this.Transaction.Extent(m.C1.Composite);

                var exception = false;
                try
                {
                    extent.Filter.AddLessThan(m.C2.C2AllorsDateTime, dateTime4);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Less Than 5
                extent = this.Transaction.Extent(m.C1.Composite);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(m.C2.C2AllorsDateTime, dateTime5);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Less Than 6
                extent = this.Transaction.Extent(m.C1.Composite);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(m.C2.C2AllorsDateTime, dateTime6);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType
                // Less Than 4
                extent = this.Transaction.Extent(m.C1.Composite);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(m.I2.I2AllorsDateTime, dateTime4);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Less Than 5
                extent = this.Transaction.Extent(m.C1.Composite);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(m.I2.I2AllorsDateTime, dateTime5);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Less Than 6
                extent = this.Transaction.Extent(m.C1.Composite);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(m.I2.I2AllorsDateTime, dateTime6);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType
                // Less Than 4
                extent = this.Transaction.Extent(m.C1.Composite);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(m.S2.S2AllorsDateTime, dateTime4);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Less Than 5
                extent = this.Transaction.Extent(m.C1.Composite);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(m.S2.S2AllorsDateTime, dateTime5);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Less Than 6
                extent = this.Transaction.Extent(m.C1.Composite);

                exception = false;
                try
                {
                    extent.Filter.AddLessThan(m.S2.S2AllorsDateTime, dateTime6);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }
    }

    [Fact]
    public void RoleDateTimeGreaterThanValue()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            foreach (var flag in TrueFalse)
            {
                var dateTime1 = new DateTime(2000, 1, 1, 0, 0, 1, DateTimeKind.Utc);
                var dateTime2 = new DateTime(2000, 1, 1, 0, 0, 2, DateTimeKind.Utc);
                var dateTime3 = new DateTime(2000, 1, 1, 0, 0, 3, DateTimeKind.Utc);
                var dateTime4 = new DateTime(2000, 1, 1, 0, 0, 4, DateTimeKind.Utc);
                var dateTime5 = new DateTime(2000, 1, 1, 0, 0, 5, DateTimeKind.Utc);
                var dateTime6 = new DateTime(2000, 1, 1, 0, 0, 6, DateTimeKind.Utc);
                var dateTime7 = new DateTime(2000, 1, 1, 0, 0, 7, DateTimeKind.Utc);
                var dateTime10 = new DateTime(2000, 1, 1, 0, 0, 10, DateTimeKind.Utc);

                if (flag)
                {
                    dateTime1 = dateTime1.ToLocalTime();
                    dateTime2 = dateTime2.ToLocalTime();
                    dateTime3 = dateTime3.ToLocalTime();
                    dateTime4 = dateTime4.ToLocalTime();
                    dateTime5 = dateTime5.ToLocalTime();
                    dateTime6 = dateTime6.ToLocalTime();
                    dateTime7 = dateTime7.ToLocalTime();
                    dateTime10 = dateTime10.ToLocalTime();
                }

                // Class
                // Greater Than 3
                var extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddGreaterThan(m.C1.C1AllorsDateTime, dateTime3);

                Assert.Equal(3, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.True(extent.Contains(this.c1C));
                Assert.True(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Greater Than 4
                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddGreaterThan(m.C1.C1AllorsDateTime, dateTime4);

                Assert.Equal(2, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.True(extent.Contains(this.c1C));
                Assert.True(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Greater Than 5
                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddGreaterThan(m.C1.C1AllorsDateTime, dateTime5);

                Assert.Empty(extent);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Interface
                // Greater Than 3
                extent = this.Transaction.Extent(m.I12.Composite);
                extent.Filter.AddGreaterThan(m.I12.I12AllorsDateTime, dateTime3);

                Assert.Equal(6, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.True(extent.Contains(this.c1C));
                Assert.True(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.True(extent.Contains(this.c2C));
                Assert.True(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Greater Than 4
                extent = this.Transaction.Extent(m.I12.Composite);
                extent.Filter.AddGreaterThan(m.I12.I12AllorsDateTime, dateTime4);

                Assert.Equal(4, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.True(extent.Contains(this.c1C));
                Assert.True(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.True(extent.Contains(this.c2C));
                Assert.True(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Greater Than 5
                extent = this.Transaction.Extent(m.I12.Composite);
                extent.Filter.AddGreaterThan(m.I12.I12AllorsDateTime, dateTime5);

                Assert.Empty(extent);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Super Interface
                // Greater Than 3
                extent = this.Transaction.Extent(m.S1234.Composite);
                extent.Filter.AddGreaterThan(m.S1234.S1234AllorsDateTime, dateTime3);

                Assert.Equal(12, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.True(extent.Contains(this.c1C));
                Assert.True(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.True(extent.Contains(this.c2C));
                Assert.True(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.True(extent.Contains(this.c3B));
                Assert.True(extent.Contains(this.c3C));
                Assert.True(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.True(extent.Contains(this.c4B));
                Assert.True(extent.Contains(this.c4C));
                Assert.True(extent.Contains(this.c4D));

                // Greater Than 4
                extent = this.Transaction.Extent(m.S1234.Composite);
                extent.Filter.AddGreaterThan(m.S1234.S1234AllorsDateTime, dateTime4);

                Assert.Equal(8, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.True(extent.Contains(this.c1C));
                Assert.True(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.True(extent.Contains(this.c2C));
                Assert.True(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.True(extent.Contains(this.c3C));
                Assert.True(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.True(extent.Contains(this.c4C));
                Assert.True(extent.Contains(this.c4D));

                // Greater Than 5
                extent = this.Transaction.Extent(m.S1234.Composite);
                extent.Filter.AddGreaterThan(m.S1234.S1234AllorsDateTime, dateTime5);

                Assert.Empty(extent);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Class - Wrong RelationType

                // Greater Than 3
                extent = this.Transaction.Extent(m.C1.Composite);

                var exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(m.C2.C2AllorsDateTime, dateTime3);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Greater Than 4
                extent = this.Transaction.Extent(m.C1.Composite);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(m.C2.C2AllorsDateTime, dateTime4);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Greater Than 5
                extent = this.Transaction.Extent(m.C1.Composite);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(m.C2.C2AllorsDateTime, dateTime5);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType

                // Greater Than 3
                extent = this.Transaction.Extent(m.C1.Composite);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(m.I2.I2AllorsDateTime, dateTime3);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Greater Than 4
                extent = this.Transaction.Extent(m.C1.Composite);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(m.I2.I2AllorsDateTime, dateTime4);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Greater Than 5
                extent = this.Transaction.Extent(m.C1.Composite);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(m.I2.I2AllorsDateTime, dateTime5);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType

                // Greater Than 3
                extent = this.Transaction.Extent(m.C1.Composite);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(m.I2.I2AllorsDateTime, dateTime3);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Greater Than 4
                extent = this.Transaction.Extent(m.C1.Composite);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(m.I2.I2AllorsDateTime, dateTime4);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Greater Than 5
                extent = this.Transaction.Extent(m.C1.Composite);

                exception = false;
                try
                {
                    extent.Filter.AddGreaterThan(m.I2.I2AllorsDateTime, dateTime5);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }
    }

    [Fact]
    public void RoleDateTimeEquals()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            foreach (var flag in TrueFalse)
            {
                var dateTime1 = new DateTime(2000, 1, 1, 0, 0, 1, DateTimeKind.Utc);
                var dateTime2 = new DateTime(2000, 1, 1, 0, 0, 2, DateTimeKind.Utc);
                var dateTime3 = new DateTime(2000, 1, 1, 0, 0, 3, DateTimeKind.Utc);
                var dateTime4 = new DateTime(2000, 1, 1, 0, 0, 4, DateTimeKind.Utc);
                var dateTime5 = new DateTime(2000, 1, 1, 0, 0, 5, DateTimeKind.Utc);
                var dateTime6 = new DateTime(2000, 1, 1, 0, 0, 6, DateTimeKind.Utc);
                var dateTime7 = new DateTime(2000, 1, 1, 0, 0, 7, DateTimeKind.Utc);
                var dateTime10 = new DateTime(2000, 1, 1, 0, 0, 10, DateTimeKind.Utc);

                if (flag)
                {
                    dateTime1 = dateTime1.ToLocalTime();
                    dateTime2 = dateTime2.ToLocalTime();
                    dateTime3 = dateTime3.ToLocalTime();
                    dateTime4 = dateTime4.ToLocalTime();
                    dateTime5 = dateTime5.ToLocalTime();
                    dateTime6 = dateTime6.ToLocalTime();
                    dateTime7 = dateTime7.ToLocalTime();
                    dateTime10 = dateTime10.ToLocalTime();
                }

                // Class
                // Equal 3
                var extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddEquals(m.C1.C1AllorsDateTime, dateTime3);

                Assert.Empty(extent);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Equal 4
                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddEquals(m.C1.C1AllorsDateTime, dateTime4);

                Assert.Single(extent);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Equal 5
                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddEquals(m.C1.C1AllorsDateTime, dateTime5);

                Assert.Equal(2, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.True(extent.Contains(this.c1C));
                Assert.True(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Interface
                // Equal 3
                extent = this.Transaction.Extent(m.I12.Composite);
                extent.Filter.AddEquals(m.I12.I12AllorsDateTime, dateTime3);

                Assert.Empty(extent);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Equal 4
                extent = this.Transaction.Extent(m.I12.Composite);
                extent.Filter.AddEquals(m.I12.I12AllorsDateTime, dateTime4);

                Assert.Equal(2, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Equal 5
                extent = this.Transaction.Extent(m.I12.Composite);
                extent.Filter.AddEquals(m.I12.I12AllorsDateTime, dateTime5);

                Assert.Equal(4, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.True(extent.Contains(this.c1C));
                Assert.True(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.True(extent.Contains(this.c2C));
                Assert.True(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Super Interface
                // Equal 3
                extent = this.Transaction.Extent(m.S1234.Composite);
                extent.Filter.AddEquals(m.S1234.S1234AllorsDateTime, dateTime3);

                Assert.Empty(extent);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Equal 4
                extent = this.Transaction.Extent(m.S1234.Composite);
                extent.Filter.AddEquals(m.S1234.S1234AllorsDateTime, dateTime4);

                Assert.Equal(4, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.True(extent.Contains(this.c1B));
                Assert.False(extent.Contains(this.c1C));
                Assert.False(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.True(extent.Contains(this.c2B));
                Assert.False(extent.Contains(this.c2C));
                Assert.False(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.True(extent.Contains(this.c3B));
                Assert.False(extent.Contains(this.c3C));
                Assert.False(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.True(extent.Contains(this.c4B));
                Assert.False(extent.Contains(this.c4C));
                Assert.False(extent.Contains(this.c4D));

                // Equal 5
                extent = this.Transaction.Extent(m.S1234.Composite);
                extent.Filter.AddEquals(m.S1234.S1234AllorsDateTime, dateTime5);

                Assert.Equal(8, extent.Count);
                Assert.False(extent.Contains(this.c1A));
                Assert.False(extent.Contains(this.c1B));
                Assert.True(extent.Contains(this.c1C));
                Assert.True(extent.Contains(this.c1D));
                Assert.False(extent.Contains(this.c2A));
                Assert.False(extent.Contains(this.c2B));
                Assert.True(extent.Contains(this.c2C));
                Assert.True(extent.Contains(this.c2D));
                Assert.False(extent.Contains(this.c3A));
                Assert.False(extent.Contains(this.c3B));
                Assert.True(extent.Contains(this.c3C));
                Assert.True(extent.Contains(this.c3D));
                Assert.False(extent.Contains(this.c4A));
                Assert.False(extent.Contains(this.c4B));
                Assert.True(extent.Contains(this.c4C));
                Assert.True(extent.Contains(this.c4D));

                // Class - Wrong RelationType
                // Equal 3
                extent = this.Transaction.Extent(m.C1.Composite);

                var exception = false;
                try
                {
                    extent.Filter.AddEquals(m.C2.C2AllorsDateTime, dateTime3);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal 4
                extent = this.Transaction.Extent(m.C1.Composite);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(m.C2.C2AllorsDateTime, dateTime4);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal 5
                extent = this.Transaction.Extent(m.C1.Composite);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(m.C2.C2AllorsDateTime, dateTime5);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Interface - Wrong RelationType
                // Equal 3
                extent = this.Transaction.Extent(m.C1.Composite);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(m.I2.I2AllorsDateTime, dateTime3);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal 4
                extent = this.Transaction.Extent(m.C1.Composite);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(m.I2.I2AllorsDateTime, dateTime4);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal 5
                extent = this.Transaction.Extent(m.C1.Composite);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(m.I2.I2AllorsDateTime, dateTime5);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Super Interface - Wrong RelationType
                // Equal 3
                extent = this.Transaction.Extent(m.C1.Composite);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(m.S2.S2AllorsDateTime, dateTime3);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal 4
                extent = this.Transaction.Extent(m.C1.Composite);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(m.S2.S2AllorsDateTime, dateTime4);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);

                // Equal 5
                extent = this.Transaction.Extent(m.C1.Composite);

                exception = false;
                try
                {
                    extent.Filter.AddEquals(m.S2.S2AllorsDateTime, dateTime5);
                }
                catch
                {
                    exception = true;
                }

                Assert.True(exception);
            }
        }
    }

    [Fact]
    public void RoleDecimalBetweenValue()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class

            // Between -10 and 0
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddBetween(m.C1.C1AllorsDecimal, -10, 0);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Between 0 and 1
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddBetween(m.C1.C1AllorsDecimal, 0, 1);

            Assert.Single(extent);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Between 1 and 2
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddBetween(m.C1.C1AllorsDecimal, 1, 2);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Between 3 and 10
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddBetween(m.C1.C1AllorsDecimal, 3, 10);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface

            // Between -10 and 0
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddBetween(m.I12.I12AllorsDecimal, -10, 0);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Between 0 and 1
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddBetween(m.I12.I12AllorsDecimal, 0, 1);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Between 1 and 2
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddBetween(m.I12.I12AllorsDecimal, 1, 2);

            Assert.Equal(6, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Between 3 and 10
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddBetween(m.I12.I12AllorsDecimal, 3, 10);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface

            // Between -10 and 0
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddBetween(m.S1234.S1234AllorsDecimal, -10, 0);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Between 0 and 1
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddBetween(m.S1234.S1234AllorsDecimal, 0, 1);

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, true, false, false);
            this.AssertC4(extent, false, true, false, false);

            // Between 1 and 2
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddBetween(m.S1234.S1234AllorsDecimal, 1, 2);

            Assert.Equal(12, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, true, true, true);
            this.AssertC4(extent, false, true, true, true);

            // Between 3 and 10
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddBetween(m.S1234.S1234AllorsDecimal, 3, 10);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Class - Wrong RelationType

            // Between -10 and 0
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddBetween(m.C2.C2AllorsDecimal, -10, 0);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Between 0 and 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddBetween(m.C2.C2AllorsDecimal, 0, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Between 1 and 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddBetween(m.C2.C2AllorsDecimal, 1, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Between 3 and 10
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddBetween(m.C2.C2AllorsDecimal, 3, 10);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void RoleDecimalLessThanValue()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class

            // Less Than 1
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddLessThan(m.C1.C1AllorsDecimal, 1);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Less Than 2
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddLessThan(m.C1.C1AllorsDecimal, 2);

            Assert.Single(extent);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Less Than 3
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddLessThan(m.C1.C1AllorsDecimal, 3);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface

            // Less Than 1
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddLessThan(m.I12.I12AllorsDecimal, 1);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Less Than 2
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddLessThan(m.I12.I12AllorsDecimal, 2);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Less Than 3
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddLessThan(m.I12.I12AllorsDecimal, 3);

            Assert.Equal(6, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface

            // Less Than 1
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddLessThan(m.S1234.S1234AllorsDecimal, 1);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Less Than 2
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddLessThan(m.S1234.S1234AllorsDecimal, 2);

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, true, false, false);
            this.AssertC4(extent, false, true, false, false);

            // Less Than 3
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddLessThan(m.S1234.S1234AllorsDecimal, 3);

            Assert.Equal(12, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, true, true, true);
            this.AssertC4(extent, false, true, true, true);

            // Class - Wrong RelationType

            // Less Than 1
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddLessThan(m.C2.C2AllorsDecimal, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Less Than 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLessThan(m.C2.C2AllorsDecimal, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Less Than 3
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLessThan(m.C2.C2AllorsDecimal, 3);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType

            // Less Than 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLessThan(m.I2.I2AllorsDecimal, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Less Than 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLessThan(m.I2.I2AllorsDecimal, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Less Than 3
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLessThan(m.I2.I2AllorsDecimal, 3);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType

            // Less Than 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLessThan(m.S2.S2AllorsDecimal, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Less Than 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLessThan(m.S2.S2AllorsDecimal, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Less Than 3
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLessThan(m.S2.S2AllorsDecimal, 3);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void RoleDecimalGreaterThanValue()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class

            // Greater Than 0
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddGreaterThan(m.C1.C1AllorsDecimal, 0);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Greater Than 1
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddGreaterThan(m.C1.C1AllorsDecimal, 1);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Greater Than 2
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddGreaterThan(m.C1.C1AllorsDecimal, 2);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface

            // Greater Than 0
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddGreaterThan(m.I12.I12AllorsDecimal, 0);

            Assert.Equal(6, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Greater Than 1
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddGreaterThan(m.I12.I12AllorsDecimal, 1);

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Greater Than 2
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddGreaterThan(m.I12.I12AllorsDecimal, 2);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface

            // Greater Than 0
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddGreaterThan(m.S1234.S1234AllorsDecimal, 0);

            Assert.Equal(12, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, true, true, true);
            this.AssertC4(extent, false, true, true, true);

            // Greater Than 1
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddGreaterThan(m.S1234.S1234AllorsDecimal, 1);

            Assert.Equal(8, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, true, true);
            this.AssertC3(extent, false, false, true, true);
            this.AssertC4(extent, false, false, true, true);

            // Greater Than 2
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddGreaterThan(m.S1234.S1234AllorsDecimal, 2);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Class - Wrong RelationType

            // Greater Than 0
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddGreaterThan(m.C2.C2AllorsDecimal, 0);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Greater Than 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddGreaterThan(m.C2.C2AllorsDecimal, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Greater Than 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddGreaterThan(m.C2.C2AllorsDecimal, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType

            // Greater Than 0
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddGreaterThan(m.I2.I2AllorsDecimal, 0);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Greater Than 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddGreaterThan(m.I2.I2AllorsDecimal, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Greater Than 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddGreaterThan(m.I2.I2AllorsDecimal, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType

            // Greater Than 0
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddGreaterThan(m.I2.I2AllorsDecimal, 0);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Greater Than 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddGreaterThan(m.I2.I2AllorsDecimal, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Greater Than 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddGreaterThan(m.I2.I2AllorsDecimal, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void RoleDecimalEquals()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class
            // Equal 0
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddEquals(m.C1.C1AllorsDecimal, 0);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Equal 1
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddEquals(m.C1.C1AllorsDecimal, 1);

            Assert.Single(extent);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Equal 2
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddEquals(m.C1.C1AllorsDecimal, 2);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface
            // Equal 0
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddEquals(m.I12.I12AllorsDecimal, 0);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Equal 1
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddEquals(m.I12.I12AllorsDecimal, 1);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Equal 2
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddEquals(m.I12.I12AllorsDecimal, 2);

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface
            // Equal 0
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddEquals(m.S1234.S1234AllorsDecimal, 0);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Equal 1
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddEquals(m.S1234.S1234AllorsDecimal, 1);

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, true, false, false);
            this.AssertC4(extent, false, true, false, false);

            // Equal 2
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddEquals(m.S1234.S1234AllorsDecimal, 2);

            Assert.Equal(8, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, true, true);
            this.AssertC3(extent, false, false, true, true);
            this.AssertC4(extent, false, false, true, true);

            // Class - Wrong RelationType
            // Equal 0
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddEquals(m.C2.C2AllorsDecimal, 0);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Equal 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.C2.C2AllorsDecimal, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Equal 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.C2.C2AllorsDecimal, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType
            // Equal 0
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.I2.I2AllorsDecimal, 0);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Equal 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.I2.I2AllorsDecimal, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Equal 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.I2.I2AllorsDecimal, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType
            // Equal 0
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.S2.S2AllorsDecimal, 0);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Equal 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.S2.S2AllorsDecimal, 1);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Equal 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.S2.S2AllorsDecimal, 2);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void RoleEnumerationEquals()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // AllorsInteger
            // Class

            // Equal 0
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddEquals(m.C1.C1AllorsInteger, Zero2Four.Zero);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Equal 1
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddEquals(m.C1.C1AllorsInteger, Zero2Four.One);

            Assert.Single(extent);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Equal 2
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddEquals(m.C1.C1AllorsInteger, Zero2Four.Two);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface

            // Equal 0
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddEquals(m.I12.I12AllorsInteger, Zero2Four.Zero);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Equal 1
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddEquals(m.I12.I12AllorsInteger, Zero2Four.One);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Equal 2
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddEquals(m.I12.I12AllorsInteger, Zero2Four.Two);

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface

            // Equal 0
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddEquals(m.S1234.S1234AllorsInteger, Zero2Four.Zero);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Equal 1
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddEquals(m.S1234.S1234AllorsInteger, Zero2Four.One);

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, true, false, false);
            this.AssertC4(extent, false, true, false, false);

            // Equal 2
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddEquals(m.S1234.S1234AllorsInteger, Zero2Four.Two);

            Assert.Equal(8, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, true, true);
            this.AssertC3(extent, false, false, true, true);
            this.AssertC4(extent, false, false, true, true);

            // Class - Wrong RelationType

            // Equal 0
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddEquals(m.C2.C2AllorsInteger, Zero2Four.Zero);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Equal 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.C2.C2AllorsInteger, Zero2Four.One);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Equal 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.C2.C2AllorsInteger, Zero2Four.Two);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType

            // Equal 0
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.I2.I2AllorsInteger, Zero2Four.Zero);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Equal 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.I2.I2AllorsInteger, Zero2Four.One);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Equal 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.I2.I2AllorsInteger, Zero2Four.Two);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType

            // Equal 0
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.S2.S2AllorsInteger, Zero2Four.Zero);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Equal 1
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.S2.S2AllorsInteger, Zero2Four.One);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Equal 2
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.S2.S2AllorsInteger, Zero2Four.Two);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Wrong type
            extent = this.Transaction.Extent(m.C1.Composite);

            var exceptionThrown = false;
            C1 first = null;
            try
            {
                extent.Filter.AddEquals(m.C1.C1AllorsBinary, Zero2Four.Zero);
                first = (C1)extent.First();
            }
            catch
            {
                exceptionThrown = true;
            }

            Assert.Null(first);
            Assert.True(exceptionThrown, "Only integer supports Enumeration");
        }
    }

    [Fact]
    public void RoleCompositeEqualsRole()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            var exceptionThrown = false;

            // Class
            var extent = this.Transaction.Extent(m.C1.Composite);
            try
            {
                extent.Filter.AddEquals(m.C1.C1C2one2one, m.I1.I1C1one2one);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            Assert.True(exceptionThrown);
        }
    }

    [Fact]
    public void RoleMany2ManyIn()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            foreach (var useOperator in this.UseOperator)
            {
                // Extent over Class
                // RelationType from Class to Class

                // In Extent over Class
                // Empty
                var inExtent = this.Transaction.Extent(m.C1.Composite);
                inExtent.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    inExtentA.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtentB.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                var extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddIn(m.C1.C1C1many2manies, inExtent);

                Assert.Empty(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full
                inExtent = this.Transaction.Extent(m.C1.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddIn(m.C1.C1C1many2manies, inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Filtered
                inExtent = this.Transaction.Extent(m.C1.Composite);
                inExtent.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    inExtentA.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtentB.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddIn(m.C1.C1C1many2manies, inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // In Extent over Interface
                // Empty
                inExtent = this.Transaction.Extent(m.I12.Composite);
                inExtent.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddIn(m.C1.C1C1many2manies, inExtent);

                Assert.Empty(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full
                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddIn(m.C1.C1C1many2manies, inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Filtered
                inExtent = this.Transaction.Extent(m.I12.Composite);
                inExtent.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddIn(m.C1.C1C1many2manies, inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // RelationType from Class to Interface

                // In Extent over Class
                // Empty
                inExtent = this.Transaction.Extent(m.C1.Composite);
                inExtent.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    inExtentA.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtentB.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddIn(m.C1.C1I12many2manies, inExtent);

                Assert.Empty(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full
                inExtent = this.Transaction.Extent(m.C1.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddIn(m.C1.C1I12many2manies, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Filtered
                inExtent = this.Transaction.Extent(m.C1.Composite);
                inExtent.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    inExtentA.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtentB.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddIn(m.C1.C1I12many2manies, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // In Extent over Interface
                // Empty
                inExtent = this.Transaction.Extent(m.I12.Composite);
                inExtent.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddIn(m.C1.C1I12many2manies, inExtent);

                Assert.Empty(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full
                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddIn(m.C1.C1I12many2manies, inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Filtered
                inExtent = this.Transaction.Extent(m.I12.Composite);
                inExtent.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddIn(m.C1.C1I12many2manies, inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);
            }
        }
    }

    [Fact]
    public void RoleMany2ManyContains()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddContains(m.C1.C1C2many2manies, this.c2C);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddContains(m.C1.C1C2many2manies, this.c2B);
            extent.Filter.AddContains(m.C1.C1C2many2manies, this.c2C);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddContains(m.C1.C1I12many2manies, this.c2C);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddContains(m.S1234.S1234many2manies, this.c1A);

            Assert.Equal(9, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, true, true, true);
            this.AssertC4(extent, false, false, false, false);

            // TODO: wrong relation
        }
    }

    [Fact]
    public void RoleMany2ManyExist()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddExists(m.C1.C1C2many2manies);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddExists(m.I12.I12C2many2manies);

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, true, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddExists(m.S1234.S1234C2many2manies);

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, true, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Class - Wrong RelationType
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddExists(m.C3.C3C2many2manies);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.I12.Composite);

            exception = false;
            try
            {
                extent.Filter.AddExists(m.C3.C3C2many2manies);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.S12.Composite);

            exception = false;
            try
            {
                extent.Filter.AddExists(m.C3.C3C2many2manies);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void RoleOne2ManyIn()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            foreach (var useOperator in this.UseOperator)
            {
                // Extent over Class

                // RelationType from Class to Class

                // In Extent over Class
                // Emtpy Extent
                var inExtent = this.Transaction.Extent(m.C1.Composite);
                inExtent.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    inExtentA.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtentB.Filter.AddEquals(m.C1.C1AllorsString, "Nothing here!");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                var extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddIn(m.C1.C1C1one2manies, inExtent);

                Assert.Empty(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full Extent
                inExtent = this.Transaction.Extent(m.C1.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddIn(m.C1.C1C1one2manies, inExtent);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, true, true, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                inExtent = this.Transaction.Extent(m.C2.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C2.Composite);
                    var inExtentB = this.Transaction.Extent(m.C2.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddIn(m.C1.C1C2one2manies, inExtent);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, true, true, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                inExtent = this.Transaction.Extent(m.C4.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C4.Composite);
                    var inExtentB = this.Transaction.Extent(m.C4.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C3.Composite);
                extent.Filter.AddIn(m.C3.C3C4one2manies, inExtent);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, true, true, false);
                this.AssertC4(extent, false, false, false, false);

                // In Extent over Shared Interface
                // Emtpy Extent
                inExtent = this.Transaction.Extent(m.I12.Composite);
                inExtent.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    inExtentA.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtentB.Filter.AddEquals(m.I12.I12AllorsString, "Nothing here!");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddIn(m.C1.C1C1one2manies, inExtent);

                Assert.Empty(extent);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full Extent
                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddIn(m.C1.C1C1one2manies, inExtent);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, true, true, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddIn(m.C1.C1C2one2manies, inExtent);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, true, true, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                inExtent = this.Transaction.Extent(m.I34.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I34.Composite);
                    var inExtentB = this.Transaction.Extent(m.I34.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C3.Composite);
                extent.Filter.AddIn(m.C3.C3C4one2manies, inExtent);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, true, true, false);
                this.AssertC4(extent, false, false, false, false);

                // RelationType from Interface to Class

                // In Extent over Class
                inExtent = this.Transaction.Extent(m.C2.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C2.Composite);
                    var inExtentB = this.Transaction.Extent(m.C2.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddIn(m.I12.I12C2one2manies, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // In Extent over Interface
                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddIn(m.I12.I12C2one2manies, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);
            }
        }
    }

    [Fact]
    public void RoleOne2ManyContains()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddContains(m.C1.C1C2one2manies, this.c2C);

            Assert.Single(extent);
            this.AssertC1(extent, false, false, true, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddContains(m.C1.C1I12one2manies, this.c2C);

            Assert.Single(extent);
            this.AssertC1(extent, false, false, true, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddContains(m.S1234.S1234one2manies, this.c1B);

            Assert.Single(extent);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // TODO: wrong relation
        }
    }

    [Fact]
    public void RoleOne2ManyExist()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddExists(m.C1.C1C2one2manies);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, true, true, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddExists(m.I12.I12C2one2manies);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, false, true, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddExists(m.S1234.S1234C2one2manies);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, true, false);
            this.AssertC4(extent, false, false, false, false);

            // Class - Wrong RelationType
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddExists(m.C3.C3C2one2manies);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.I12.Composite);

            exception = false;
            try
            {
                extent.Filter.AddExists(m.C3.C3C2one2manies);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.S12.Composite);

            exception = false;
            try
            {
                extent.Filter.AddExists(m.C3.C3C2one2manies);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void RoleOne2OneInExtent()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            foreach (var useOperator in this.UseOperator)
            {
                // Extent over Class

                // RelationType from Class to Class

                // In Extent over Class
                var inExtent = this.Transaction.Extent(m.C1.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                var extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddIn(m.C1.C1C1one2one, inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                inExtent = this.Transaction.Extent(m.C2.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C2.Composite);
                    var inExtentB = this.Transaction.Extent(m.C2.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddIn(m.C1.C1C2one2one, inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                inExtent = this.Transaction.Extent(m.C4.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C4.Composite);
                    var inExtentB = this.Transaction.Extent(m.C4.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C3.Composite);
                extent.Filter.AddIn(m.C3.C3C4one2one, inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, true, true, true);
                this.AssertC4(extent, false, false, false, false);

                // In Extent over Shared Interface
                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddIn(m.C1.C1C1one2one, inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddIn(m.C1.C1C2one2one, inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                inExtent = this.Transaction.Extent(m.I34.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I34.Composite);
                    var inExtentB = this.Transaction.Extent(m.I34.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C3.Composite);
                extent.Filter.AddIn(m.C3.C3C4one2one, inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, true, true, true);
                this.AssertC4(extent, false, false, false, false);

                // RelationType from Class to Interface

                // In Extent over Class
                inExtent = this.Transaction.Extent(m.C2.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C2.Composite);
                    var inExtentB = this.Transaction.Extent(m.C2.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddIn(m.C1.C1I12one2one, inExtent);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // In Extent over Shared Interface
                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddIn(m.C1.C1I12one2one, inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);
            }
        }
    }

    [Fact]
    public void RoleOne2OneInArray()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Extent over Class

            // RelationType from Class to Class

            // In Extent over Class
            var inExtent = this.Transaction.Extent(m.C1.Composite).ToArray();

            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddIn(m.C1.C1C1one2one, (IEnumerable<IObject>)inExtent);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            inExtent = this.Transaction.Extent(m.C2.Composite).ToArray();

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddIn(m.C1.C1C2one2one, (IEnumerable<IObject>)inExtent);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            inExtent = this.Transaction.Extent(m.C4.Composite).ToArray();

            extent = this.Transaction.Extent(m.C3.Composite);
            extent.Filter.AddIn(m.C3.C3C4one2one, (IEnumerable<IObject>)inExtent);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, true, true, true);
            this.AssertC4(extent, false, false, false, false);

            // In Extent over Shared Interface
            inExtent = this.Transaction.Extent(m.I12.Composite).ToArray();

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddIn(m.C1.C1C1one2one, (IEnumerable<IObject>)inExtent);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            inExtent = this.Transaction.Extent(m.I12.Composite).ToArray();

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddIn(m.C1.C1C2one2one, (IEnumerable<IObject>)inExtent);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            inExtent = this.Transaction.Extent(m.I34.Composite).ToArray();

            extent = this.Transaction.Extent(m.C3.Composite);
            extent.Filter.AddIn(m.C3.C3C4one2one, (IEnumerable<IObject>)inExtent);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, true, true, true);
            this.AssertC4(extent, false, false, false, false);

            // RelationType from Class to Interface

            // In Extent over Class
            inExtent = this.Transaction.Extent(m.C2.Composite).ToArray();

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddIn(m.C1.C1I12one2one, (IEnumerable<IObject>)inExtent);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // In Extent over Shared Interface
            inExtent = this.Transaction.Extent(m.I12.Composite);

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddIn(m.C1.C1I12one2one, (IEnumerable<IObject>)inExtent);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);
        }
    }

    [Fact]
    public void RoleMany2OneIn()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            foreach (var useOperator in this.UseOperator)
            {
                // Extent over Class

                // RelationType from Class to Class

                // In Extent over Shared Interface

                // With filter
                var inExtent = this.Transaction.Extent(m.C1.Composite);
                inExtent.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C1.Composite);
                    inExtentA.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                    var inExtentB = this.Transaction.Extent(m.C1.Composite);
                    inExtentB.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                var extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddIn(m.C1.C1C1many2one, inExtent);

                Assert.Single(extent);
                this.AssertC1(extent, false, true, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // Full
                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddIn(m.C1.C1C1many2one, inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddIn(m.C1.C1C2many2one, inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                inExtent = this.Transaction.Extent(m.I34.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I34.Composite);
                    var inExtentB = this.Transaction.Extent(m.I34.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C3.Composite);
                extent.Filter.AddIn(m.C3.C3C4many2one, inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, false, false, false);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, true, true, true);
                this.AssertC4(extent, false, false, false, false);

                // RelationType from Class to Interface

                // In Extent over Class
                inExtent = this.Transaction.Extent(m.C2.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.C2.Composite);
                    var inExtentB = this.Transaction.Extent(m.C2.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddIn(m.C1.C1I12many2one, inExtent);

                Assert.Equal(2, extent.Count);
                this.AssertC1(extent, false, false, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);

                // In Extent over Shared Interface
                inExtent = this.Transaction.Extent(m.I12.Composite);
                if (useOperator)
                {
                    var inExtentA = this.Transaction.Extent(m.I12.Composite);
                    var inExtentB = this.Transaction.Extent(m.I12.Composite);
                    inExtent = this.Transaction.Union(inExtentA, inExtentB);
                }

                extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddIn(m.C1.C1I12many2one, inExtent);

                Assert.Equal(3, extent.Count);
                this.AssertC1(extent, false, true, true, true);
                this.AssertC2(extent, false, false, false, false);
                this.AssertC3(extent, false, false, false, false);
                this.AssertC4(extent, false, false, false, false);
            }
        }
    }

    [Fact]
    public void RoleOne2OneEquals()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddEquals(m.C1.C1C1one2one, this.c1B);

            Assert.Single(extent);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddEquals(m.C1.C1C2one2one, this.c2B);

            Assert.Single(extent);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddEquals(m.I12.I12C2one2one, this.c2A);

            Assert.Single(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, true, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddEquals(m.S1234.S1234C2one2one, this.c2A);

            Assert.Single(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, true, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Class - Wrong RelationType
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddEquals(m.C3.C3C2one2one, this.c2A);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.I12.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.C3.C3C2one2one, this.c2A);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.S12.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.C3.C3C2one2one, this.c2A);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void RoleOne2OneExist()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddExists(m.C1.C1C1one2one);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddExists(m.C1.C1C2one2one);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.C3.Composite);
            extent.Filter.AddExists(m.C3.C3C4one2one);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, true, true, true);
            this.AssertC4(extent, false, false, false, false);

            // Interface
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddExists(m.I12.I12C2one2one);

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, true, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddExists(m.S1234.S1234C2one2one);

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, true, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Class - Wrong RelationType
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddExists(m.C3.C3C2one2one);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.I12.Composite);

            exception = false;
            try
            {
                extent.Filter.AddExists(m.C3.C3C2one2one);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.S12.Composite);

            exception = false;
            try
            {
                extent.Filter.AddExists(m.C3.C3C2one2one);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void RoleOne2OneInstanceof()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class

            // Class
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddInstanceOf(m.C1.C1C1one2one, m.C1.Composite);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddInstanceOf(m.C1.C1C2one2one, m.C2.Composite);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddInstanceOf(m.C1.C1I12one2one, m.C2.Composite);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddInstanceOf(m.S1234.S1234one2one, m.C2.Composite);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, false, true, false);
            this.AssertC2(extent, false, false, true, false);
            this.AssertC3(extent, false, false, true, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface

            // Class
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddInstanceOf(m.C1.C1C2one2one, m.I2.Composite);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddInstanceOf(m.C1.C1I12one2one, m.I2.Composite);

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddInstanceOf(m.C1.C1I12one2one, m.I12.Composite);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddInstanceOf(m.S1234.S1234one2one, m.S1234.Composite);

            Assert.Equal(9, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, true, true, true);
            this.AssertC4(extent, false, false, false, false);

            // TODO: wrong relation
        }
    }

    [Fact]
    public void RoleStringEqualsRole()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddEquals(m.C1.C1AllorsString, m.C1.C1StringEquals);

            Assert.Single(extent);
            this.AssertC1(extent, false, false, true, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);
        }
    }

    [Fact]
    public void RoleStringEqualsValue()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class

            // Equal ""
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddEquals(m.C1.C1AllorsString, string.Empty);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.C3.Composite);
            extent.Filter.AddEquals(m.C3.C3AllorsString, string.Empty);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Equal "ᴀbra"
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");

            Assert.Single(extent);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.C3.Composite);
            extent.Filter.AddEquals(m.C3.C3AllorsString, "ᴀbra");

            Assert.Single(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, true, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Equal "ᴀbracadabra"
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbracadabra");

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.C3.Composite);
            extent.Filter.AddEquals(m.C3.C3AllorsString, "ᴀbracadabra");

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, true, true);
            this.AssertC4(extent, false, false, false, false);

            // Exclusive Interface

            // Equal ""
            extent = this.Transaction.Extent(m.I1.Composite);
            extent.Filter.AddEquals(m.I1.I1AllorsString, string.Empty);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.I3.Composite);
            extent.Filter.AddEquals(m.I3.I3AllorsString, string.Empty);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Equal "ᴀbra"
            extent = this.Transaction.Extent(m.I1.Composite);
            extent.Filter.AddEquals(m.I1.I1AllorsString, "ᴀbra");

            Assert.Single(extent);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.I3.Composite);
            extent.Filter.AddEquals(m.I3.I3AllorsString, "ᴀbra");

            Assert.Single(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, true, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Equal "ᴀbracadabra"
            extent = this.Transaction.Extent(m.I1.Composite);
            extent.Filter.AddEquals(m.I1.I1AllorsString, "ᴀbracadabra");

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.I3.Composite);
            extent.Filter.AddEquals(m.I3.I3AllorsString, "ᴀbracadabra");

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, true, true);
            this.AssertC4(extent, false, false, false, false);

            // Shared Interface

            // Equal ""
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddEquals(m.I12.I12AllorsString, string.Empty);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.I34.Composite);
            extent.Filter.AddEquals(m.I34.I34AllorsString, string.Empty);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Equal "ᴀbra"
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbra");

            Assert.Single(extent);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.I23.Composite);
            extent.Filter.AddEquals(m.I23.I23AllorsString, "ᴀbra");

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, true, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.C2.Composite);
            extent.Filter.AddEquals(m.I23.I23AllorsString, "ᴀbra");

            Assert.Single(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.C3.Composite);
            extent.Filter.AddEquals(m.I23.I23AllorsString, "ᴀbra");

            Assert.Single(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, true, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.I34.Composite);
            extent.Filter.AddEquals(m.I34.I34AllorsString, "ᴀbra");

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, true, false, false);
            this.AssertC4(extent, false, true, false, false);

            extent = this.Transaction.Extent(m.C3.Composite);
            extent.Filter.AddEquals(m.I34.I34AllorsString, "ᴀbra");

            Assert.Single(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, true, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Equal "ᴀbracadabra"
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbracadabra");

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddEquals(m.I12.I12AllorsString, "ᴀbracadabra");

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            extent = this.Transaction.Extent(m.I34.Composite);
            extent.Filter.AddEquals(m.I34.I34AllorsString, "ᴀbracadabra");

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, true, true);
            this.AssertC4(extent, false, false, true, true);

            // Super Interface

            // Equal ""
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddEquals(m.S1234.S1234AllorsString, string.Empty);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Equal "ᴀbra"
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddEquals(m.S1234.S1234AllorsString, "ᴀbra");

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, true, false, false);
            this.AssertC4(extent, false, true, false, false);

            // Equal "ᴀbracadabra"
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddEquals(m.S1234.S1234AllorsString, "ᴀbracadabra");

            Assert.Equal(8, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, true, true);
            this.AssertC3(extent, false, false, true, true);
            this.AssertC4(extent, false, false, true, true);

            // Class - Wrong RelationType

            // Equal ""
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddEquals(m.C2.C2AllorsString, string.Empty);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Equal "ᴀbra"
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.C2.C2AllorsString, "ᴀbra");
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Equal "ᴀbracadabra"
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.C2.C2AllorsString, "ᴀbracadabra");
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType

            // Equal ""
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.I2.I2AllorsString, string.Empty);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Equal "ᴀbra"
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.I2.I2AllorsString, "ᴀbra");
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Equal "ᴀbracadabra"
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.I2.I2AllorsString, "ᴀbracadabra");
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType

            // Equal ""
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.S2.S2AllorsString, string.Empty);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Equal "ᴀbra"
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.S2.S2AllorsString, "ᴀbra");
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Equal "ᴀbracadabra"
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddEquals(m.S2.S2AllorsString, "ᴀbracadabra");
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void RoleStringExist()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddExists(m.C1.C1AllorsString);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddExists(m.I12.I12AllorsString);

            Assert.Equal(6, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddExists(m.S1234.S1234AllorsString);

            Assert.Equal(12, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, true, true, true);
            this.AssertC3(extent, false, true, true, true);
            this.AssertC4(extent, false, true, true, true);

            // Class - Wrong RelationType
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddExists(m.C2.C2AllorsString);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddExists(m.I2.I2AllorsString);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddExists(m.S2.S2AllorsString);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public void RoleStringLike()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class

            // Like ""
            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddLike(m.C1.C1AllorsString, string.Empty);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Like "ᴀbra"
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddLike(m.C1.C1AllorsString, "ᴀbra");

            Assert.Single(extent);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Like "ᴀbracadabra"
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddLike(m.C1.C1AllorsString, "ᴀbracadabra");

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Like "notfound"
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddLike(m.C1.C1AllorsString, "notfound");

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Like "%ra%"
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddLike(m.C1.C1AllorsString, "%ra%");

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Like "%bra"
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddLike(m.C1.C1AllorsString, "%bra");

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Like "%cadabra"
            extent = this.Transaction.Extent(m.C1.Composite);
            extent.Filter.AddLike(m.C1.C1AllorsString, "%cadabra");

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Interface

            // Like ""
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddLike(m.I12.I12AllorsString, string.Empty);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Like "ᴀbra"
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddLike(m.I12.I12AllorsString, "ᴀbra");

            Assert.Equal(2, extent.Count);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Like "ᴀbracadabra"
            extent = this.Transaction.Extent(m.I12.Composite);
            extent.Filter.AddLike(m.I12.I12AllorsString, "ᴀbracadabra");

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, true, true);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Super Interface

            // Like ""
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddLike(m.S1234.S1234AllorsString, string.Empty);

            Assert.Empty(extent);
            this.AssertC1(extent, false, false, false, false);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Like "ᴀbra"
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddLike(m.S1234.S1234AllorsString, "ᴀbra");

            Assert.Equal(4, extent.Count);
            this.AssertC1(extent, false, true, false, false);
            this.AssertC2(extent, false, true, false, false);
            this.AssertC3(extent, false, true, false, false);
            this.AssertC4(extent, false, true, false, false);

            // Like "ᴀbracadabra"
            extent = this.Transaction.Extent(m.S1234.Composite);
            extent.Filter.AddLike(m.S1234.S1234AllorsString, "ᴀbracadabra");

            Assert.Equal(8, extent.Count);
            this.AssertC1(extent, false, false, true, true);
            this.AssertC2(extent, false, false, true, true);
            this.AssertC3(extent, false, false, true, true);
            this.AssertC4(extent, false, false, true, true);

            // Class - Wrong RelationType

            // Like ""
            extent = this.Transaction.Extent(m.C1.Composite);

            var exception = false;
            try
            {
                extent.Filter.AddLike(m.C2.C2AllorsString, string.Empty);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Like "ᴀbra"
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLike(m.C2.C2AllorsString, "ᴀbra");
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Like "ᴀbracadabra"
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLike(m.C2.C2AllorsString, "ᴀbracadabra");
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Interface - Wrong RelationType

            // Like ""
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLike(m.I2.I2AllorsString, string.Empty);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Like "ᴀbra"
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLike(m.I2.I2AllorsString, "ᴀbra");
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Like "ᴀbracadabra"
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLike(m.I2.I2AllorsString, "ᴀbracadabra");
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Super Interface - Wrong RelationType

            // Like ""
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLike(m.S2.S2AllorsString, string.Empty);
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Like "ᴀbra"
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLike(m.S2.S2AllorsString, "ᴀbra");
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);

            // Like "ᴀbracadabra"
            extent = this.Transaction.Extent(m.C1.Composite);

            exception = false;
            try
            {
                extent.Filter.AddLike(m.S2.S2AllorsString, "ᴀbracadabra");
            }
            catch
            {
                exception = true;
            }

            Assert.True(exception);
        }
    }

    [Fact]
    public virtual void Shared()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            var sharedExtent = this.Transaction.Extent(m.C2.Composite);
            sharedExtent.Filter.AddLike(m.C2.C2AllorsString, "%");
            var firstExtent = this.Transaction.Extent(m.C1.Composite);
            firstExtent.Filter.AddIn(m.C1.C1C2many2manies, sharedExtent);
            var secondExtent = this.Transaction.Extent(m.C1.Composite);
            secondExtent.Filter.AddIn(m.C1.C1C2many2manies, sharedExtent);
            var intersectExtent = this.Transaction.Intersect(firstExtent, secondExtent);
            intersectExtent.ToArray(typeof(C1));
        }
    }

    [Fact]
    public virtual void SortOne()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            this.c1B.C1AllorsString = "3";
            this.c1C.C1AllorsString = "1";
            this.c1D.C1AllorsString = "2";

            this.Transaction.Commit();

            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.AddSort(m.C1.C1AllorsString);

            var sortedObjects = (C1[])extent.ToArray(typeof(C1));
            Assert.Equal(4, sortedObjects.Length);
            Assert.Equal(this.c1C, sortedObjects[0]);
            Assert.Equal(this.c1D, sortedObjects[1]);
            Assert.Equal(this.c1B, sortedObjects[2]);
            Assert.Equal(this.c1A, sortedObjects[3]);

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.AddSort(m.C1.C1AllorsString, SortDirection.Ascending);

            sortedObjects = (C1[])extent.ToArray(typeof(C1));
            Assert.Equal(4, sortedObjects.Length);
            Assert.Equal(this.c1C, sortedObjects[0]);
            Assert.Equal(this.c1D, sortedObjects[1]);
            Assert.Equal(this.c1B, sortedObjects[2]);
            Assert.Equal(this.c1A, sortedObjects[3]);

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.AddSort(m.C1.C1AllorsString, SortDirection.Descending);

            sortedObjects = (C1[])extent.ToArray(typeof(C1));
            Assert.Equal(4, sortedObjects.Length);
            Assert.Equal(this.c1A, sortedObjects[0]);
            Assert.Equal(this.c1B, sortedObjects[1]);
            Assert.Equal(this.c1D, sortedObjects[2]);
            Assert.Equal(this.c1C, sortedObjects[3]);

            foreach (var useOperator in this.UseOperator)
            {
                if (useOperator)
                {
                    var firstExtent = this.Transaction.Extent(m.C1.Composite);
                    firstExtent.Filter.AddLike(m.C1.C1AllorsString, "1");
                    var secondExtent = this.Transaction.Extent(m.C1.Composite);
                    extent = this.Transaction.Union(firstExtent, secondExtent);
                    secondExtent.Filter.AddLike(m.C1.C1AllorsString, "3");
                    extent.AddSort(m.C1.C1AllorsString);

                    sortedObjects = (C1[])extent.ToArray(typeof(C1));
                    Assert.Equal(2, sortedObjects.Length);
                    Assert.Equal(this.c1C, sortedObjects[0]);
                    Assert.Equal(this.c1B, sortedObjects[1]);
                }
            }
        }
    }

    [Fact]
    public virtual void SortTwo()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            this.c1B.C1AllorsString = "a";
            this.c1C.C1AllorsString = "b";
            this.c1D.C1AllorsString = "a";

            this.c1B.C1AllorsInteger = 2;
            this.c1C.C1AllorsInteger = 1;
            this.c1D.C1AllorsInteger = 0;

            this.Transaction.Commit();

            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.AddSort(m.C1.C1AllorsString);
            extent.AddSort(m.C1.C1AllorsInteger);

            var sortedObjects = (C1[])extent.ToArray(typeof(C1));
            Assert.Equal(4, sortedObjects.Length);
            Assert.Equal(this.c1D, sortedObjects[0]);
            Assert.Equal(this.c1B, sortedObjects[1]);
            Assert.Equal(this.c1C, sortedObjects[2]);
            Assert.Equal(this.c1A, sortedObjects[3]);

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.AddSort(m.C1.C1AllorsString);
            extent.AddSort(m.C1.C1AllorsInteger, SortDirection.Ascending);

            sortedObjects = (C1[])extent.ToArray(typeof(C1));
            Assert.Equal(4, sortedObjects.Length);
            Assert.Equal(this.c1D, sortedObjects[0]);
            Assert.Equal(this.c1B, sortedObjects[1]);
            Assert.Equal(this.c1C, sortedObjects[2]);
            Assert.Equal(this.c1A, sortedObjects[3]);

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.AddSort(m.C1.C1AllorsString);
            extent.AddSort(m.C1.C1AllorsInteger, SortDirection.Descending);

            sortedObjects = (C1[])extent.ToArray(typeof(C1));
            Assert.Equal(4, sortedObjects.Length);
            Assert.Equal(this.c1B, sortedObjects[0]);
            Assert.Equal(this.c1D, sortedObjects[1]);
            Assert.Equal(this.c1C, sortedObjects[2]);
            Assert.Equal(this.c1A, sortedObjects[3]);

            extent = this.Transaction.Extent(m.C1.Composite);
            extent.AddSort(m.C1.C1AllorsString, SortDirection.Descending);
            extent.AddSort(m.C1.C1AllorsInteger, SortDirection.Descending);

            sortedObjects = (C1[])extent.ToArray(typeof(C1));
            Assert.Equal(4, sortedObjects.Length);
            Assert.Equal(this.c1A, sortedObjects[0]);
            Assert.Equal(this.c1C, sortedObjects[1]);
            Assert.Equal(this.c1B, sortedObjects[2]);
            Assert.Equal(this.c1D, sortedObjects[3]);
        }
    }

    [Fact]
    public virtual void SortDifferentTransaction()
    {
        foreach (var init in this.Inits)
        {
            init();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            var c1A = this.Transaction.Build<C1>();
            var c1B = this.Transaction.Build<C1>();
            var c1C = this.Transaction.Build<C1>();
            var c1D = this.Transaction.Build<C1>();

            c1A.C1AllorsString = "2";
            c1B.C1AllorsString = "1";
            c1C.C1AllorsString = "3";

            var extent = this.Transaction.Extent(m.C1.Composite);
            extent.AddSort(m.C1.C1AllorsString, SortDirection.Ascending);

            var sortedObjects = (C1[])extent.ToArray(typeof(C1));

            Assert.Equal(4, sortedObjects.Length);
            Assert.Equal(c1B, sortedObjects[0]);
            Assert.Equal(c1A, sortedObjects[1]);
            Assert.Equal(c1C, sortedObjects[2]);
            Assert.Equal(c1D, sortedObjects[3]);

            var c1AId = c1A.Id;

            this.Transaction.Commit();

            using (var transaction2 = this.CreateTransaction())
            {
                c1A = (C1)transaction2.Instantiate(c1AId);

                extent = transaction2.Extent(m.C1.Composite);
                extent.AddSort(m.C1.C1AllorsString, SortDirection.Ascending);

                sortedObjects = (C1[])extent.ToArray(typeof(C1));

                Assert.Equal(4, sortedObjects.Length);
                Assert.Equal(c1B, sortedObjects[0]);
                Assert.Equal(c1A, sortedObjects[1]);
                Assert.Equal(c1C, sortedObjects[2]);
                Assert.Equal(c1D, sortedObjects[3]);
            }
        }
    }

    [Fact]
    public void Hierarchy()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            var extent = this.Transaction.Extent(m.I4.Composite);
            Assert.Equal(4, extent.Count);
            this.AssertC4(extent, true, true, true, true);
        }
    }

    [Fact]
    public virtual void Union()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Class

            // Filtered
            var firstExtent = this.Transaction.Extent(m.C1.Composite);
            firstExtent.Filter.AddEquals(m.C1.C1AllorsString, "ᴀbra");

            var secondExtent = this.Transaction.Extent(m.C1.Composite);
            secondExtent.Filter.AddLike(m.C1.C1AllorsString, "ᴀbracadabra");

            var extent = this.Transaction.Union(firstExtent, secondExtent);

            Assert.Equal(3, extent.Count);
            this.AssertC1(extent, false, true, true, true);
            this.AssertC2(extent, false, false, false, false);
            this.AssertC3(extent, false, false, false, false);
            this.AssertC4(extent, false, false, false, false);

            // Different Classes
            firstExtent = this.Transaction.Extent(m.C1.Composite);
            secondExtent = this.Transaction.Extent(m.C2.Composite);

            var exceptionThrown = false;
            try
            {
                this.Transaction.Union(firstExtent, secondExtent);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            Assert.True(exceptionThrown);

            // Name clashes
            Extent<Company> parents = this.Transaction.Extent(m.Company.Composite);

            Extent<Company> children = this.Transaction.Extent(m.Company.Composite);
            children.Filter.AddIn(m.Company.CompanyWhereChild, parents);

            Extent<Company> allCompanies = this.Transaction.Union(parents, children);

            Extent<Person> persons = this.Transaction.Extent(m.Person.Composite);
            persons.Filter.AddIn(m.Person.Company, allCompanies);

            Assert.Empty(persons);
        }
    }

    [Fact]
    public void ValidateAssociationIn()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Wrong Parameters
            var exceptionThrown = false;
            try
            {
                var inExtent = this.Transaction.Extent(m.C1.Composite);

                var extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddIn(((RoleType)m.C1.I12AllorsBoolean).AssociationType, inExtent);
                extent.ToArray();
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            Assert.True(exceptionThrown);
        }
    }

    [Fact]
    public void ValidateAssociationContains()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Wrong Parameters
            var exceptionThrown = false;
            try
            {
                var extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddContains(m.C2.C1WhereC1C2one2many, this.c1C);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            Assert.True(exceptionThrown);

            exceptionThrown = false;
            try
            {
                var extent = this.Transaction.Extent(m.C2.Composite);
                extent.Filter.AddContains(m.C2.C1WhereC1C2one2one, this.c1C);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            Assert.True(exceptionThrown);
        }
    }

    [Fact]
    public void ValidateAssociationEquals()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Wrong Parameters
            var exceptionThrown = false;
            try
            {
                var extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddEquals(m.C1.C1sWhereC1C1many2many, this.c1B);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            Assert.True(exceptionThrown);

            exceptionThrown = false;
            try
            {
                var extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddEquals(m.C1.C1sWhereC1C1many2one, this.c1B);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            Assert.True(exceptionThrown);
        }
    }

    [Fact]
    public void ValidateAssociationExist()
    {
        // TODO:
    }

    [Fact]
    public void ValidateAssociationNotExist()
    {
        // TODO:
    }

    [Fact]
    public void ValidateRoleBetween()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Wrong Parameters
            var exceptionThrown = false;
            try
            {
                var extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddBetween(m.C1.C1C2one2one, 0, 1);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            Assert.True(exceptionThrown);
        }
    }

    [Fact]
    public void ValidateRoleContainsFilter()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Wrong Parameters
            var exceptionThrown = false;
            try
            {
                var extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddContains(m.C1.C1AllorsString, this.c2C);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            Assert.True(exceptionThrown);
        }
    }

    [Fact]
    public void ValidateRoleEqualFilter()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Wrong Parameters
            var exceptionThrown = false;
            try
            {
                var extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddEquals(m.C1.C1C2one2manies, this.c2B);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            Assert.True(exceptionThrown);

            // Wrong Parameters
            exceptionThrown = false;
            try
            {
                var extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddEquals(m.C1.C1C2many2manies, this.c2B);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            Assert.True(exceptionThrown);
        }
    }

    [Fact]
    public void ValidateRoleExistFilter()
    {
        // TODO:
    }

    [Fact]
    public void ValidateRoleGreaterThanFilter()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Wrong Parameters
            var exceptionThrown = false;
            try
            {
                var extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddGreaterThan(m.C1.C1C2one2one, 0);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            Assert.True(exceptionThrown);
        }
    }

    [Fact]
    public void ValidateRoleInFilter()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Wrong Parameters
            var exceptionThrown = false;
            try
            {
                var inExtent = this.Transaction.Extent(m.C1.Composite);

                var extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddIn(m.C1.C1AllorsString, inExtent);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            Assert.True(exceptionThrown);
        }
    }

    [Fact]
    public void ValidateRoleLessThanFilter()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Wrong Parameters
            var exceptionThrown = false;
            try
            {
                var extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddLessThan(m.C1.C1C2one2one, 1);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            Assert.True(exceptionThrown);
        }
    }

    [Fact]
    public void ValidateRoleLikeThanFilter()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // Wrong Parameters
            var exceptionThrown = false;
            try
            {
                var extent = this.Transaction.Extent(m.C1.Composite);
                extent.Filter.AddLike(m.C1.C1AllorsBoolean, string.Empty);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            Assert.True(exceptionThrown);
        }
    }

    [Fact]
    public void ValidateRoleNotExistFilter()
    {
        // TODO:
    }

    [Fact]
    public void RoleContainsMany2ManyAndContained()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // many2many contains
            var c1 = this.Transaction.Build<C1>();
            var c2 = this.Transaction.Build<C2>();
            var c3 = this.Transaction.Build<C3>();

            c2.AddC3Many2Many(c3);
            c1.C1C2many2one = c2;

            var c2s = this.Transaction.Extent(m.C2.Composite);
            c2s.Filter.AddContains(m.C2.C3Many2Manies, c3);

            Extent<C1> c1s = this.Transaction.Extent(m.C1.Composite);
            c1s.Filter.AddIn(m.C1.C1C2many2one, c2s);

            Assert.Single(c1s);
            Assert.Equal(c1, c1s[0]);
        }
    }

    [Fact]
    public void RoleContainsOne2ManySharedClassAndContained()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // manymany contains
            var c2 = this.Transaction.Build<C2>();
            var c3 = this.Transaction.Build<C3>();
            var c4 = this.Transaction.Build<C4>();

            c3.AddC3C4one2many(c4);
            c2.C3Many2One = c3;

            var c3s = this.Transaction.Extent(m.C3.Composite);
            c3s.Filter.AddContains(m.C3.C3C4one2manies, c4);

            Extent<C2> c2s = this.Transaction.Extent(m.C2.Composite);
            c2s.Filter.AddIn(m.C2.C3Many2One, c3s);

            Assert.Single(c2s);
            Assert.Equal(c2, c2s[0]);
        }
    }

    [Fact]
    public void AssociationContainsMany2ManyAndContained()
    {
        foreach (var init in this.Inits)
        {
            init();
            this.Populate();
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            // many2many contains
            var c1 = this.Transaction.Build<C1>();
            var c2 = this.Transaction.Build<C2>();
            var c3 = this.Transaction.Build<C3>();

            c3.AddC3C2many2many(c2);
            c1.C1C2many2one = c2;

            var c2s = this.Transaction.Extent(m.C2.Composite);
            c2s.Filter.AddContains(m.C2.C3sWhereC3C2many2many, c3);

            Extent<C1> c1s = this.Transaction.Extent(m.C1.Composite);
            c1s.Filter.AddIn(m.C1.C1C2many2one, c2s);

            Assert.Single(c1s);
            Assert.Equal(c1, c1s[0]);
        }
    }

    protected abstract ITransaction CreateTransaction();

    protected void Populate()
    {
        var population = new TestPopulation(this.Transaction);

        this.c1A = population.C1A;
        this.c1B = population.C1B;
        this.c1C = population.C1C;
        this.c1D = population.C1D;

        this.c2A = population.C2A;
        this.c2B = population.C2B;
        this.c2C = population.C2C;
        this.c2D = population.C2D;

        this.c3A = population.C3A;
        this.c3B = population.C3B;
        this.c3C = population.C3C;
        this.c3D = population.C3D;

        this.c4A = population.C4A;
        this.c4B = population.C4B;
        this.c4C = population.C4C;
        this.c4D = population.C4D;
    }

    private static Unit GetAllorsString(ObjectType objectType) => (Unit)objectType.MetaPopulation.FindById(UnitIds.String);

    private void AssertC1(Extent extent, bool assert0, bool assert1, bool assert2, bool assert3)
    {
        if (assert0)
        {
            Assert.True(extent.Contains(this.c1A), "C1_1");
        }
        else
        {
            Assert.False(extent.Contains(this.c1A), "C1_1");
        }

        if (assert1)
        {
            Assert.True(extent.Contains(this.c1B), "C1_2");
        }
        else
        {
            Assert.False(extent.Contains(this.c1B), "C1_2");
        }

        if (assert2)
        {
            Assert.True(extent.Contains(this.c1C), "C1_3");
        }
        else
        {
            Assert.False(extent.Contains(this.c1C), "C1_3");
        }

        if (assert3)
        {
            Assert.True(extent.Contains(this.c1D), "C1_4");
        }
        else
        {
            Assert.False(extent.Contains(this.c1D), "C1_4");
        }
    }

    private void AssertC2(Extent extent, bool assert0, bool assert1, bool assert2, bool assert3)
    {
        if (assert0)
        {
            Assert.True(extent.Contains(this.c2A), "C2_1");
        }
        else
        {
            Assert.False(extent.Contains(this.c2A), "C2_1");
        }

        if (assert1)
        {
            Assert.True(extent.Contains(this.c2B), "C2_2");
        }
        else
        {
            Assert.False(extent.Contains(this.c2B), "C2_2");
        }

        if (assert2)
        {
            Assert.True(extent.Contains(this.c2C), "C2_3");
        }
        else
        {
            Assert.False(extent.Contains(this.c2C), "C2_3");
        }

        if (assert3)
        {
            Assert.True(extent.Contains(this.c2D), "C2_4");
        }
        else
        {
            Assert.False(extent.Contains(this.c2D), "C2_4");
        }
    }

    private void AssertC3(Extent extent, bool assert0, bool assert1, bool assert2, bool assert3)
    {
        if (assert0)
        {
            Assert.True(extent.Contains(this.c3A), "C3_1");
        }
        else
        {
            Assert.False(extent.Contains(this.c3A), "C3_1");
        }

        if (assert1)
        {
            Assert.True(extent.Contains(this.c3B), "C3_2");
        }
        else
        {
            Assert.False(extent.Contains(this.c3B), "C3_2");
        }

        if (assert2)
        {
            Assert.True(extent.Contains(this.c3C), "C3_3");
        }
        else
        {
            Assert.False(extent.Contains(this.c3C), "C3_3");
        }

        if (assert3)
        {
            Assert.True(extent.Contains(this.c3D), "C3_4");
        }
        else
        {
            Assert.False(extent.Contains(this.c3D), "C3_4");
        }
    }

    private void AssertC4(Extent extent, bool assert0, bool assert1, bool assert2, bool assert3)
    {
        if (assert0)
        {
            Assert.True(extent.Contains(this.c4A), "C4_1");
        }
        else
        {
            Assert.False(extent.Contains(this.c4A), "C4_1");
        }

        if (assert1)
        {
            Assert.True(extent.Contains(this.c4B), "C4_2");
        }
        else
        {
            Assert.False(extent.Contains(this.c4B), "C4_2");
        }

        if (assert2)
        {
            Assert.True(extent.Contains(this.c4C), "C4_3");
        }
        else
        {
            Assert.False(extent.Contains(this.c4C), "C4_3");
        }

        if (assert3)
        {
            Assert.True(extent.Contains(this.c4D), "C4_4");
        }
        else
        {
            Assert.False(extent.Contains(this.c4D), "C4_4");
        }
    }

    #region Population
#pragma warning disable IDE1006
    protected C1 c1A;
    protected C1 c1B;
    protected C1 c1C;
    protected C1 c1D;
    protected C2 c2A;
    protected C2 c2B;
    protected C2 c2C;
    protected C2 c2D;
    protected C3 c3A;
    protected C3 c3B;
    protected C3 c3C;
    protected C3 c3D;
    protected C4 c4A;
    protected C4 c4B;
    protected C4 c4C;
    protected C4 c4D;
#pragma warning restore IDE1006
    #endregion
}
