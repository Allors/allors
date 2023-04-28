// <copyright file="LoadTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using Allors.Workspace.Adapters.Tests;

namespace Allors.Workspace.Adapters.Json.Newtonsoft.Tests
{
    using Xunit;

    public class DiffTests : Adapters.Tests.DiffTests, IClassFixture<Fixture>
    {
        public DiffTests(Fixture fixture) : base(fixture) => this.Profile = new Profile();

        public override IProfile Profile { get; }
    }
}
