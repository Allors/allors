// <copyright file="PushTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using Allors.Workspace.Adapters.Tests;

namespace Allors.Workspace.Adapters.Direct.Tests
{
    using Xunit;

    public class MvvmTests : Adapters.Tests.MvvmTests, IClassFixture<Fixture>
    {
        public MvvmTests(Fixture fixture) : base(fixture) => this.Profile = new Profile(fixture);

        public override IProfile Profile { get; }
    }
}
