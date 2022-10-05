// <copyright file="LocaleTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain.Tests
{
    using System.Linq;
    using Allors.Database.Meta.Extensions;
    using Xunit;

    public class LocaleTests : DomainTest, IClassFixture<Fixture>
    {
        public LocaleTests(Fixture fixture) : base(fixture) { }

        [Fact]
        public void RequiredRoleTypes()
        {
            var @class = this.M.Locale;

            var requiredRoleTypes = @class.CompositeRoleTypeByRoleType.Values
                .Where(v => v.IsRequired())
                .Select(v => v.RoleType).ToArray();

            Assert.Equal(2, requiredRoleTypes.Length);

            Assert.Contains(@class.Language, requiredRoleTypes);
            Assert.Contains(@class.Country, requiredRoleTypes);
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
            var locale = new Locales(this.Transaction).FindBy(this.M.Locale.Name, Locales.DutchNetherlandsName);

            Assert.Equal("nl-NL", locale.Name);
        }
    }
}
