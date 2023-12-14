// <copyright file="RangesAddTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Meta.Static.Tests;

using Xunit;

public class MetaBuilderTests
{
    [Fact]
    public void Build()
    {
        var m = new MetaBuilder();
        m.Build();
    }

}
