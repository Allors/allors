// <copyright file="AccessControlTests.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the AccessControlTests type.</summary>

namespace Allors.Database.Domain.Tests
{
    using System.Collections;
    using Allors.Database.Configuration.Derivations.Default;
    using Xunit;

    public class AccessControlTests : DomainTest, IClassFixture<Fixture>
    {
        public AccessControlTests(Fixture fixture) : base(fixture) { }

        public override Config Config => base.Config with { SetupSecurity = true };

        [Fact]
        public void GivenNoAccessControlWhenCreatingAnAccessControlWithoutARoleThenAccessControlIsInvalid()
        {
            var userGroup = this.BuildUserGroup("UserGroup");
            var securityToken = this.BuildSecurityToken();

            var grant = this.BuildGrant(userGroup);

            securityToken.AddGrant(grant);

            var validation = this.Transaction.Derive(false);

            Assert.True(validation.HasErrors);
            Assert.Single(validation.Errors);

            var derivationError = validation.Errors[0];

            Assert.Single(derivationError.Relations);
            Assert.Equal(typeof(DerivationErrorRequired), derivationError.GetType());
            Assert.Equal(this.M.Grant.Role.RelationType, derivationError.Relations[0].RelationType);
        }

        [Fact]
        public void GivenNoAccessControlWhenCreatingAAccessControlWithoutAUserOrUserGroupThenAccessControlIsInvalid()
        {
            var securityToken = this.BuildSecurityToken();
            var role = this.BuildRole("Role");

            var grant = this.Transaction.Build<Grant>(v => v.Role = role);
            securityToken.AddGrant(grant);

            var validation = this.Transaction.Derive(false);

            Assert.True(validation.HasErrors);
            Assert.Single(validation.Errors);

            var derivationError = validation.Errors[0];

            Assert.Equal(2, derivationError.Relations.Length);
            Assert.Equal(typeof(DerivationErrorAtLeastOne), derivationError.GetType());
            Assert.True(new ArrayList(derivationError.RoleTypes).Contains(this.M.Grant.Subjects));
            Assert.True(new ArrayList(derivationError.RoleTypes).Contains(this.M.Grant.SubjectGroups));
        }
    }
}
