// <copyright file="PreparedExtentTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain.Tests
{
    using System.Collections.Generic;
    using Allors.Database.Configuration;
    using Allors.Database.Services;
    using Xunit;

    public class PreparedExtentTests : DomainTest, IClassFixture<Fixture>
    {
        public PreparedExtentTests(Fixture fixture) : base(fixture) { }

        [Fact]
        public async void WithParameter()
        {
            var extentService = this.Transaction.Database.Services.Get<IPreparedExtents>();
            var organizationByName = extentService.Get(PreparedExtents.OrganizationByName);

            var arguments = new Arguments(new Dictionary<string, object> { { "name", "Acme" }, });

            Extent<Organization> organizations = organizationByName.Build(this.Transaction, arguments).ToArray();

            Assert.Single(organizations);

            var organization = organizations[0];

            Assert.Equal("Acme", organization.Name);
        }

    }
}
