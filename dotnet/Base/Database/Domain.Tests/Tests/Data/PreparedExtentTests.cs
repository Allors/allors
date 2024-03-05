// <copyright file="PreparedExtentTests.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain.Tests
{
    using System.Collections.Generic;
    using System.Linq;
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

            var organizations = organizationByName.Build(this.Transaction, arguments).Cast<Organization>().ToArray();

            Assert.Single(organizations);

            var organization = organizations[0];

            Assert.Equal("Acme", organization.Name);
        }

    }
}
