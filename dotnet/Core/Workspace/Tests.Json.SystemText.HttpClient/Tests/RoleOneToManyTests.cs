// <copyright file="Many2OneTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.Workspace.Json
{
    using Xunit;

    public class RoleOneToManyTests : Workspace.RoleOneToManyTests, IClassFixture<Fixture>
    {
        public RoleOneToManyTests(Fixture fixture) : base(fixture) => this.Profile = new Profile();

        public override IProfile Profile { get; }
    }
}