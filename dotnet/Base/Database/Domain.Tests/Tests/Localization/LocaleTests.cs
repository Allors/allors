// <copyright file="LocaleTests.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain.Tests
{
    using Domain;
    using Xunit;

    public class LocaleTests : DomainTest, IClassFixture<Fixture>
    {
        public LocaleTests(Fixture fixture) : base(fixture) { }

        [Fact]
        public void GivenLocale_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var locale = this.Transaction.Build<Locale>();

            Assert.True(this.Transaction.Derive(false).HasErrors);

            this.Transaction.Rollback();

            locale = this.Transaction.Build<Locale>(v =>
            {
                v.Country = this.Transaction.Extent<Country>().FindBy(this.M.Country.Key, "BE");
            });

            Assert.True(this.Transaction.Derive(false).HasErrors);

            this.Transaction.Rollback();

            locale = this.Transaction.Build<Locale>(v =>
            {
                v.Language = this.Transaction.Extent<Language>().FindBy(this.M.Language.Key, "en");
            });

            Assert.False(this.Transaction.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenLocale_WhenDeriving_ThenNameIsSet()
        {
            var locale = this.Transaction.Build<Locale>(v =>
            {
                v.Language = this.Transaction.Extent<Language>().FindBy(this.M.Language.Key, "en");
                v.Country = this.Transaction.Extent<Country>().FindBy(this.M.Country.Key, "BE");
            });

            this.Transaction.Derive();

            Assert.Equal("en-BE", locale.Key);
        }

        [Fact]
        public void GivenLocaleWhenValidatingThenNameIsSet()
        {
            var locale = this.Transaction.Extent<Locale>().FindBy(this.M.Locale.Key, "nl");

            Assert.Equal("nl", locale.Key);
        }
    }
}
