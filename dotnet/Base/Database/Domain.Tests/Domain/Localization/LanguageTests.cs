// <copyright file="LanguageTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain.Tests
{
    using Domain;
    using Xunit;

    public class LanguageTests : DomainTest, IClassFixture<Fixture>
    {
        public LanguageTests(Fixture fixture) : base(fixture) { }

        [Fact]
        public void GivenLanguageWhenValidatingThenRequiredRelationsMustExist()
        {
            this.Transaction.Build<Language>();

            Assert.True(this.Transaction.Derive(false).HasErrors);

            this.Transaction.Build<Language>(v => v.IsoCode = "XX");

            Assert.True(this.Transaction.Derive(false).HasErrors);

            this.Transaction.Build<Language>(v =>
            {
                v.IsoCode = "XX";
                v.AddLocalisedName(this.Transaction.Build<LocalisedText>(w =>
                {
                    w.Locale = new Locales(this.Transaction).LocaleByName["en"];
                    w.Text = "XXX";
                }));
            });

            Assert.False(this.Transaction.Derive(false).HasErrors);
        }
    }
}
