// <copyright file="CurrencyTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the CurrencyTests type.</summary>

namespace Allors.Database.Domain.Tests
{
    using Domain;
    using Xunit;

    public class CurrencyTests : DomainTest, IClassFixture<Fixture>
    {
        public CurrencyTests(Fixture fixture) : base(fixture) { }

        [Fact]
        public void GivenCurrencyWhenValidatingThenRequiredRelationsMustExist()
        {
            this.Transaction.Build<Currency>();

            Assert.True(this.Transaction.Derive(false).HasErrors);

            this.Transaction.Rollback();

            this.Transaction.Build<Currency>(v =>
            {
                v.Key = "BND";
            });

            Assert.False(this.Transaction.Derive(false).HasErrors);
        }
    }
}
