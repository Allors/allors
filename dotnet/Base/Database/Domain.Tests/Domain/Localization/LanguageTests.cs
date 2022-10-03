// <copyright file="LanguageTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain.Tests
{
    using System.Linq;
    using Allors.Database.Meta.Extensions;
    using Xunit;

    public class LanguageTests : DomainTest, IClassFixture<Fixture>
    {
        public LanguageTests(Fixture fixture) : base(fixture) { }

        [Fact]
        public void RequiredRoleTypes()
        {
            var @class = this.M.Language;

            var requiredRoleTypes = @class.ConcreteRoleTypeByRoleType.Values
                .Where(v => v.Required())
                .Select(v => v.RoleType).ToArray();

            Assert.Equal(3, requiredRoleTypes.Length);

            Assert.Contains(@class.IsoCode, requiredRoleTypes);
            Assert.Contains(@class.Name, requiredRoleTypes);
            Assert.Contains(@class.NativeName, requiredRoleTypes);
        }
    }
}
