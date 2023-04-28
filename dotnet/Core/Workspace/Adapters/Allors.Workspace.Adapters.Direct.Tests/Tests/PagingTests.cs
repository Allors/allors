// <copyright file="PagingTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using Allors.Workspace.Adapters.Tests;

namespace Allors.Workspace.Adapters.Direct.Tests
{
    using Xunit;

    public class PagingTests : Adapters.Tests.PagingTests, IClassFixture<Fixture>
    {
        public PagingTests(Fixture fixture) : base(fixture) => this.Profile = new Profile(fixture);

        public override IProfile Profile { get; }
    }
}
