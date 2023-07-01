﻿// <copyright file="LocaleTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
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
                v.Country = new Countries(this.Transaction).FindBy(this.M.Country.IsoCode, "BE");
            });

            Assert.True(this.Transaction.Derive(false).HasErrors);

            this.Transaction.Rollback();

            locale = this.Transaction.Build<Locale>(v =>
            {
                v.Language = new Languages(this.Transaction).FindBy(this.M.Language.IsoCode, "en");
            });

            Assert.False(this.Transaction.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenLocale_WhenDeriving_ThenNameIsSet()
        {
            var locale = this.Transaction.Build<Locale>(v =>
            {
                v.Language = new Languages(this.Transaction).FindBy(this.M.Language.IsoCode, "en");
                v.Country = new Countries(this.Transaction).FindBy(this.M.Country.IsoCode, "BE");
            });

            this.Transaction.Derive();

            Assert.Equal("en-BE", locale.Name);
        }

        [Fact]
        public void GivenLocaleWhenValidatingThenNameIsSet()
        {
            var locale = new Locales(this.Transaction).FindBy(this.M.Locale.Name, "nl");

            Assert.Equal("nl", locale.Name);
        }
    }
}
