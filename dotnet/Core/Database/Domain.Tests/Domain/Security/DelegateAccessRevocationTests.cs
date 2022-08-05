// <copyright file="DelegateAccessTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain.Tests
{
    using Meta;
    using Xunit;
    using Permission = Domain.Permission;

    public class DelegateAccessRevocationTests : DomainTest, IClassFixture<Fixture>
    {
        public DelegateAccessRevocationTests(Fixture fixture) : base(fixture) { }

        public override Config Config => new Config { SetupSecurity = true };

        [Fact]
        public void WithRevocationAndDelegateWithoutRevocation()
        {
            var user = this.BuildPerson("user");

            var delegatedAccessClass = new AccessClassBuilder(this.Transaction);
            var accessClass = new AccessClassBuilder(this.Transaction).WithDelegatedAccess(delegatedAccessClass);

            var securityToken = this.BuildSecurityToken();
            var permission = this.FindPermission(this.M.AccessClass.Property, Operations.Read);
            var role = this.BuildRole("Role", permission);

            securityToken.AddGrant(
                new GrantBuilder(this.Transaction)
                    .WithRole(role)
                    .WithSubject(user)
                    .Build());

            accessClass.AddSecurityToken(securityToken);

            var revocation = this.BuildRevocation(permission);
            accessClass.AddRevocation(revocation);

            this.Transaction.Derive();
            this.Transaction.Commit();

            // Use default security from Singleton
            var acl = new DatabaseAccessControl(this.Security, user)[accessClass];
            Assert.False(acl.CanRead(this.M.AccessClass.Property));
            Assert.False(acl.CanRead(this.M.AccessClass.Property));
        }

        [Fact]
        public void WithoutRevocationAndDelegateWithoutRevocation()
        {
            var user = this.BuildPerson("user");

            var delegatedAccessClass = new AccessClassBuilder(this.Transaction);
            var accessClass = new AccessClassBuilder(this.Transaction).WithDelegatedAccess(delegatedAccessClass);

            var securityToken = this.BuildSecurityToken();
            var permission = this.FindPermission(this.M.AccessClass.Property, Operations.Read);
            var role = this.BuildRole("Role", permission);

            securityToken.AddGrant(
                new GrantBuilder(this.Transaction)
                    .WithRole(role)
                    .WithSubject(user)
                    .Build());

            accessClass.AddSecurityToken(securityToken);

            var revocation = this.BuildRevocation(permission);

            this.Transaction.Derive();
            this.Transaction.Commit();

            // Use default security from Singleton
            var acl = new DatabaseAccessControl(this.Security, user)[accessClass];
            Assert.True(acl.CanRead(this.M.AccessClass.Property));
            Assert.True(acl.CanRead(this.M.AccessClass.Property));
        }

        [Fact]
        public void WithRevocationAndDelegateWithRevocation()
        {
            var user = this.BuildPerson("user");

            var delegatedAccessClass = new AccessClassBuilder(this.Transaction);
            var accessClass = new AccessClassBuilder(this.Transaction).WithDelegatedAccess(delegatedAccessClass);

            var securityToken = this.BuildSecurityToken();
            var permission = this.FindPermission(this.M.AccessClass.Property, Operations.Read);
            var role = this.BuildRole("Role", permission);

            securityToken.AddGrant(
                new GrantBuilder(this.Transaction)
                    .WithRole(role)
                    .WithSubject(user)
                    .Build());

            accessClass.AddSecurityToken(securityToken);

            var revocation = this.BuildRevocation(permission);
            accessClass.AddRevocation(revocation);
            delegatedAccessClass.AddRevocation(revocation);

            this.Transaction.Derive();
            this.Transaction.Commit();

            // Use default security from Singleton
            var acl = new DatabaseAccessControl(this.Security, user)[accessClass];
            Assert.False(acl.CanRead(this.M.AccessClass.Property));
            Assert.False(acl.CanRead(this.M.AccessClass.Property));
        }

        [Fact]
        public void WithoutRevocationAndDelegateWithRevocation()
        {
            var user = this.BuildPerson("user");

            var delegatedAccessClass = new AccessClassBuilder(this.Transaction);
            var accessClass = new AccessClassBuilder(this.Transaction).WithDelegatedAccess(delegatedAccessClass);

            var securityToken = this.BuildSecurityToken();
            var permission = this.FindPermission(this.M.AccessClass.Property, Operations.Read);
            var role = this.BuildRole("Role", permission);

            securityToken.AddGrant(
                new GrantBuilder(this.Transaction)
                    .WithRole(role)
                    .WithSubject(user)
                    .Build());


            accessClass.AddSecurityToken(securityToken);

            var revocation = this.BuildRevocation(permission);
            delegatedAccessClass.AddRevocation(revocation);

            this.Transaction.Derive();
            this.Transaction.Commit();

            // Use default security from Singleton
            var acl = new DatabaseAccessControl(this.Security, user)[accessClass];
            Assert.False(acl.CanRead(this.M.AccessClass.Property));
            Assert.False(acl.CanRead(this.M.AccessClass.Property));
        }


        private Permission FindPermission(IRoleType roleType, Operations operation)
        {
            var objectType = (Class)roleType.AssociationType.ObjectType;
            return new Permissions(this.Transaction).Get(objectType, roleType, operation);
        }
    }
}
