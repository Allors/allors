// <copyright file="ExtentTest.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory;

public class ExtentTest : Adapters.ExtentTest
{
    private readonly Profile profile = new();

    protected override IProfile Profile => this.profile;

    public override void Dispose() => this.profile.Dispose();

    protected override ITransaction CreateTransaction() => this.profile.CreateTransaction();
}
