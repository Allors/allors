// <copyright file="CacheTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
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
            var existingOrganisation = BuildOrganisation("existing organisation");

            this.Transaction.Derive();
            this.Transaction.Commit();

            foreach (var session in new[] { this.Transaction })
            {
                session.Commit();

                var cachedOrganisation = new Organisations(session).Cache[existingOrganisation.UniqueId];
                Assert.Equal(existingOrganisation.UniqueId, cachedOrganisation.UniqueId);
                Assert.Same(session, cachedOrganisation.Strategy.Transaction);

                var newOrganisation = this.BuildOrganisation("new organisation");
                cachedOrganisation = new Organisations(session).Cache[newOrganisation.UniqueId];
                Assert.Equal(newOrganisation.UniqueId, cachedOrganisation.UniqueId);
                Assert.Same(session, cachedOrganisation.Strategy.Transaction);

                session.Rollback();
            }
        }
    }
}
