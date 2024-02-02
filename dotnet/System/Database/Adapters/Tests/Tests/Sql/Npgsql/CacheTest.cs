﻿// <copyright file="CacheTest.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.Npgsql;

using Xunit;

public class CacheTest : Adapters.CacheTest, IClassFixture<Fixture<CacheTest>>
{
    private readonly Profile profile;

    public CacheTest(Fixture<CacheTest> fixture) => this.profile = new Profile(fixture.ConnectionString);

    public override void Dispose() => this.profile.Dispose();

    protected override IDatabase CreateDatabase() => this.profile.CreateDatabase();
}
