// <copyright file="MethodTests.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using Allors.Workspace.Adapters.Tests;

namespace Allors.Workspace.Adapters.Direct.Tests
{
    using Xunit;

    public class SecurityTests : Adapters.Tests.SecurityTests, IClassFixture<Fixture>
    {
        public SecurityTests(Fixture fixture) : base(fixture) => this.Profile = new Profile(fixture);

        public override IProfile Profile { get; }
    }
}
