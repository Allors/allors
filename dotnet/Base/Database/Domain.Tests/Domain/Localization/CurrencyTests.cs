// <copyright file="CurrencyTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the CurrencyTests type.</summary>

namespace Allors.Database.Domain.Tests
{
    using System.Linq;
    using Allors.Database.Meta.Extensions;
    using Xunit;

    public class CurrencyTests : DomainTest, IClassFixture<Fixture>
    {
        public CurrencyTests(Fixture fixture) : base(fixture) { }

        [Fact]
        public void RequiredRoleTypes()
        {
            var @class = this.M.Currency;

            var requiredRoleTypes = @class.CompositeRoleTypeByRoleType.Values
                .Where(v => v.Required())
                .Select(v => v.RoleType).ToArray();

            Assert.Equal(3, requiredRoleTypes.Length);

            Assert.Contains(@class.UniqueId, requiredRoleTypes);
            Assert.Contains(@class.IsoCode, requiredRoleTypes);
            Assert.Contains(@class.Name, requiredRoleTypes);
        }
    }
}
