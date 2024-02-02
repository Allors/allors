// <copyright file="CacheTest.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain.Tests
{
    using Xunit;

    public class CacheTest : DomainTest, IClassFixture<Fixture>
    {
        public CacheTest(Fixture fixture) : base(fixture) { }

        [Fact]
        public void Default()
        {
            var existingOrganization = BuildOrganization("existing organization");

            this.Transaction.Derive();
            this.Transaction.Commit();

            var organizationByUniqueId = this.Transaction.Scoped<OrganizationByUniqueId>();

            foreach (var transaction in new[] { this.Transaction })
            {
                transaction.Commit();

                var cachedOrganization = organizationByUniqueId[existingOrganization.UniqueId];
                Assert.Equal(existingOrganization.UniqueId, cachedOrganization.UniqueId);
                Assert.Same(transaction, cachedOrganization.Strategy.Transaction);

                var newOrganization = this.BuildOrganization("new organization");
                cachedOrganization = organizationByUniqueId[newOrganization.UniqueId];
                Assert.Equal(newOrganization.UniqueId, cachedOrganization.UniqueId);
                Assert.Same(transaction, cachedOrganization.Strategy.Transaction);

                transaction.Rollback();
            }
        }
    }
}
