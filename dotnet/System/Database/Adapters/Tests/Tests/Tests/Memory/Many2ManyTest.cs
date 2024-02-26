// <copyright file="Many2ManyTest.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the Default type.
// </summary>

namespace Allors.Database.Adapters.Memory;

using System;

public class Many2ManyTest : Adapters.Many2ManyTest, IDisposable
{
    private readonly Profile profile = new();

    protected override IProfile Profile => this.profile;

    public override void Dispose() => this.profile.Dispose();
}
