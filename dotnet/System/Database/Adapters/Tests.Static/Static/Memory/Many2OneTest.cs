// <copyright file="Many2OneTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory;

using System;

public class Many2OneTest : Adapters.Many2OneTest, IDisposable
{
    private readonly Profile profile = new();

    protected override IProfile Profile => this.profile;

    public override void Dispose() => this.profile.Dispose();
}
