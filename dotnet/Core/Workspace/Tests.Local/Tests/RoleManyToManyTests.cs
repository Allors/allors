// <copyright file="Many2OneTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.Workspace.Local
{
    using Xunit;

    public class RoleManyToManyTests : Workspace.RoleManyToManyTests, IClassFixture<Fixture>
    {
        public RoleManyToManyTests(Fixture fixture) : base(fixture) => this.Profile = new Profile(fixture);

        public override IProfile Profile { get; }
    }
}
