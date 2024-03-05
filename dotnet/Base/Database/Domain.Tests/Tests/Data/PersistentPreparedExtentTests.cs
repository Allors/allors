// <copyright file="PreparedExtentTests.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Protocol.Json.SystemText;
    using Database;
    using Domain;
    using Services;
    using Xunit;

    public class PersistentPreparedExtentTests : DomainTest, IClassFixture<Fixture>
    {
        public PersistentPreparedExtentTests(Fixture fixture) : base(fixture) { }

        [Fact]
        public async void WithParameter()
        {
            var organisations = this.Transaction.Extent<Organization>().ToArray();

            var extentService = this.Transaction.Database.Services.Get<IPreparedExtents>();
            var organizationByName = extentService.Get(PersistentPreparedExtent.ByNameId);

            var arguments = new Protocol.Json.Arguments(new Dictionary<string, object>
            {
                { "name", "Acme" },
            }, new UnitConvert());

            var organizations = organizationByName.Build(this.Transaction, arguments).Cast<Organization>().ToArray();

            Assert.Single(organizations);

            var organization = organizations[0];

            Assert.Equal("Acme", organization.Name);
        }
    }
}
