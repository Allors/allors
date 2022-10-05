﻿// <copyright file="CountryTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain.Tests
{
    using System.Linq;
    using Allors.Database.Meta.Extensions;
    using Xunit;

    public class CountryTests : DomainTest, IClassFixture<Fixture>
    {
        public CountryTests(Fixture fixture)
            : base(fixture) { }

        [Fact]
        public void RequiredRoleTypes()
        {
            var @class = this.M.Country;

            var requiredRoleTypes = @class.CompositeRoleTypeByRoleType.Values
                .Where(v => v.IsRequired())
                .Select(v => v.RoleType).ToArray();

            Assert.Equal(2, requiredRoleTypes.Length);

            Assert.Contains(@class.IsoCode, requiredRoleTypes);
            Assert.Contains(@class.Name, requiredRoleTypes);
        }
    }
}
