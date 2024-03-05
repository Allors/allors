// <copyright file="DatabaseAccessControlListTests.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain.Tests
{
    using System.Linq;
    using Xunit;

    public class DatabaseAccessControlListsTests : DomainTest, IClassFixture<Fixture>
    {
        public DatabaseAccessControlListsTests(Fixture fixture) : base(fixture) { }

        public override Config Config => base.Config with { SetupSecurity = true };

        [Fact]
        public void GivenAnAuthenticationPopulationWhenCreatingAnAccessListForGuestThenPermissionIsDenied()
        {
            this.Transaction.Derive();
            this.Transaction.Commit();

            foreach (var transaction in new[] { this.Transaction })
            {
                transaction.Commit();

                var guest = this.Transaction.Extent<User>().FindBy(this.M.User.UserName, "guest@example.com");
                var acls = new DatabaseAccessControl(this.Security, guest);
                foreach (var aco in transaction.Extent(this.M.Organization.Composite))
                {
                    // When
                    var accessList = acls[aco];

                    // Then
                    Assert.False(accessList.CanExecute(this.M.Organization.JustDoIt));
                }

                transaction.Rollback();
            }
        }

        [Fact]
        public void GivenAUserAndAnAccessControlledObjectWhenGettingTheAccessListThenUserHasAccessToThePermissionsInTheRole()
        {
            var permission = this.FindPermission(this.M.Organization.Name, Operations.Read);
            var role = this.BuildRole("Role", permission);
            var person = this.BuildPerson("John", "Doe");
            this.BuildGrant(person, role);

            this.Transaction.Derive();
            this.Transaction.Commit();

            foreach (var transaction in new[] { this.Transaction })
            {
                transaction.Commit();

                var organization = this.BuildOrganization("Organization");

                var token = this.BuildSecurityToken();
                organization.AddSecurityToken(token);

                var accessControl = (Grant)transaction.Instantiate(role.GrantsWhereRole.First());
                token.AddGrant(accessControl);

                this.Transaction.Derive();

                Assert.False(this.Transaction.Derive(false).HasErrors);

                var acl = new DatabaseAccessControl(this.Security, person)[organization];

                Assert.True(acl.CanRead(this.M.Organization.Name));

                transaction.Rollback();
            }
        }

        [Fact]
        public void GivenAUserGroupAndAnAccessControlledObjectWhenGettingTheAccessListThenUserHasAccessToThePermissionsInTheRole()
        {
            var permission = this.FindPermission(this.M.Organization.Name, Operations.Read);
            var role = this.BuildRole("Role", permission);
            var person = this.BuildPerson("John", "Doe");
            this.BuildUserGroup("Group", person);

            this.BuildGrant(person, role);

            this.Transaction.Derive();
            this.Transaction.Commit();

            foreach (var transaction in new[] { this.Transaction })
            {
                transaction.Commit();

                var organization = this.BuildOrganization("Organization");

                var token = this.BuildSecurityToken();
                organization.AddSecurityToken(token);

                var accessControl = (Grant)transaction.Instantiate(role.GrantsWhereRole.First());
                token.AddGrant(accessControl);

                Assert.False(this.Transaction.Derive(false).HasErrors);

                var acl = new DatabaseAccessControl(this.Security, person)[organization];

                Assert.True(acl.CanRead(this.M.Organization.Name));

                transaction.Rollback();
            }
        }

        [Fact]
        public void GivenAnotherUserAndAnAccessControlledObjectWhenGettingTheAccessListThenUserHasAccessToThePermissionsInTheRole()
        {
            var readOrganizationName = this.FindPermission(this.M.Organization.Name, Operations.Read);
            var databaseRole = this.BuildRole("Role", readOrganizationName);

            Assert.False(this.Transaction.Derive(false).HasErrors);

            var person = this.BuildPerson("John", "Doe");
            var anotherPerson = this.BuildPerson("Jane", "Doe");

            this.Transaction.Derive();
            this.Transaction.Commit();

            this.BuildGrant(anotherPerson, databaseRole);
            this.Transaction.Commit();

            foreach (var transaction in new[] { this.Transaction })
            {
                transaction.Commit();

                var organization = this.BuildOrganization("Organization");

                var token = this.BuildSecurityToken();
                organization.AddSecurityToken(token);

                var role = transaction.Extent<Role>().FindBy(this.M.Role.Name, "Role");
                var accessControl = (Grant)transaction.Instantiate(role.GrantsWhereRole.First());
                token.AddGrant(accessControl);

                Assert.False(this.Transaction.Derive(false).HasErrors);

                var acl = new DatabaseAccessControl(this.Security, person)[organization];

                Assert.False(acl.CanRead(this.M.Organization.Name));

                transaction.Rollback();
            }
        }

        [Fact]
        public void GivenAnotherUserGroupAndAnAccessControlledObjectWhenGettingTheAccessListThenUserHasAccessToThePermissionsInTheRole()
        {
            var readOrganizationName = this.FindPermission(this.M.Organization.Name, Operations.Read);
            var databaseRole = this.BuildRole("Role", readOrganizationName);

            var person = this.BuildPerson("John", "Doe");
            this.BuildUserGroup("Group", person);
            var anotherUserGroup = this.BuildUserGroup("AnotherGroup");

            this.Transaction.Derive();
            this.Transaction.Commit();

            this.BuildGrant(anotherUserGroup, databaseRole);

            this.Transaction.Commit();

            foreach (var transaction in new[] { this.Transaction })
            {
                transaction.Commit();

                var organization = this.BuildOrganization("Organization");

                var token = this.BuildSecurityToken();
                organization.AddSecurityToken(token);

                var role = transaction.Extent<Role>().FindBy(this.M.Role.Name, "Role");
                var accessControl = (Grant)transaction.Instantiate(role.GrantsWhereRole.First());
                token.AddGrant(accessControl);

                Assert.False(this.Transaction.Derive(false).HasErrors);

                var acl = new DatabaseAccessControl(this.Security, person)[organization];

                Assert.False(acl.CanRead(this.M.Organization.Name));

                transaction.Rollback();
            }
        }

        [Fact]
        public void GivenAnAccessListWhenRemovingUserFromACLThenUserHasNoAccessToThePermissionsInTheRole()
        {
            var permission = this.FindPermission(this.M.Organization.Name, Operations.Read);
            var role = this.BuildRole("Role", permission);
            var person = this.BuildPerson("John", "Doe");
            var person2 = this.BuildPerson("Jane", "Doe");
            this.BuildGrant(person, role);

            this.Transaction.Derive();
            this.Transaction.Commit();

            foreach (var transaction in new[] { this.Transaction })
            {
                transaction.Commit();

                var organization = this.BuildOrganization("Organization");

                var token = this.BuildSecurityToken();
                organization.AddSecurityToken(token);

                var accessControl = (Grant)transaction.Instantiate(role.GrantsWhereRole.First());
                token.AddGrant(accessControl);

                this.Transaction.Derive();

                var acl = new DatabaseAccessControl(this.Security, person)[organization];

                accessControl.RemoveSubject(person);
                accessControl.AddSubject(person2);

                this.Transaction.Derive();
                this.Transaction.Commit();

                acl = new DatabaseAccessControl(this.Security, person)[organization];

                Assert.False(acl.CanRead(this.M.Organization.Name));

                transaction.Rollback();
            }
        }

        [Fact]
        public void Revocations()
        {
            var readOrganizationName = this.FindPermission(this.M.Organization.Name, Operations.Read);
            var databaseRole = this.BuildRole("Role", readOrganizationName);
            var person = this.BuildPerson("John", "Doe");
            this.BuildGrant(person, databaseRole);

            this.Transaction.Derive();
            this.Transaction.Commit();

            foreach (var transaction in new[] { this.Transaction })
            {
                transaction.Commit();

                var organization = this.BuildOrganization("Organization");

                var token = this.BuildSecurityToken();
                organization.AddSecurityToken(token);

                var role = transaction.Extent<Role>().FindBy(this.M.Role.Name, "Role");
                var accessControl = (Grant)transaction.Instantiate(role.GrantsWhereRole.First());
                token.AddGrant(accessControl);

                Assert.False(this.Transaction.Derive(false).HasErrors);

                var acl = new DatabaseAccessControl(this.Security, person)[organization];

                Assert.True(acl.CanRead(this.M.Organization.Name));

                var revocation = this.BuildRevocation(readOrganizationName);

                organization.AddRevocation(revocation);

                acl = new DatabaseAccessControl(this.Security, person)[organization];

                Assert.False(acl.CanRead(this.M.Organization.Name));

                transaction.Rollback();
            }
        }
    }
}
