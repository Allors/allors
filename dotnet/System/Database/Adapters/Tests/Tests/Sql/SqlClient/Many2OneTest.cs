﻿// <copyright file="Many2OneTest.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.SqlClient;

using Xunit;

public class Many2OneTest : Adapters.Many2OneTest, IClassFixture<Fixture<Many2OneTest>>
{
    private readonly Profile profile;

    public Many2OneTest(Fixture<CacheTest> fixture) => this.profile = new Profile(fixture.ConnectionString);

    protected override IProfile Profile => this.profile;

    public override void Dispose() => this.profile.Dispose();
}
