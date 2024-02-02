// <copyright file="CountryTests.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain.Tests
{
    using Domain;
    using Xunit;

    public class CountryTests : DomainTest, IClassFixture<Fixture>
    {
        public CountryTests(Fixture fixture) : base(fixture) { }

        [Fact]
        public void GivenCountryWhenValidatingThenRequiredRelationsMustExist()
        {
            this.Transaction.Build<Country>();

            Assert.True(this.Transaction.Derive(false).HasErrors);

            this.Transaction.Rollback();

            this.Transaction.Build<Country>(v =>
            {
                v.Key = "XX";
            });

            Assert.False(this.Transaction.Derive(false).HasErrors);
        }
    }
}
