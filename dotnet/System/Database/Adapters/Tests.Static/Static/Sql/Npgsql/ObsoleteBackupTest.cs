// <copyright file="SerializationTest.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.Npgsql;

using Xunit;

public class ObsoleteBackupTest : Adapters.ObsoleteBackupTest, IClassFixture<Fixture<ObsoleteBackupTest>>
{
    private readonly Profile profile;

    public ObsoleteBackupTest() => this.profile = new Profile(this.GetType().Name);

    protected override IProfile Profile => this.profile;

    public override void Dispose() => this.profile.Dispose();

    protected override IDatabase CreatePopulation() => this.profile.CreateMemoryDatabase();
}
