// <copyright file="DelegateAccessTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain.Tests
{
    using Meta;
    using Xunit;
    using Permission = Domain.Permission;
    using AccessClass = Domain.AccessClass;

    public class DelegateAccessGrantTests : DomainTest, IClassFixture<Fixture>
    {
        public DelegateAccessGrantTests(Fixture fixture) : base(fixture) { }

        public override Config Config => new Config { SetupSecurity = true };

        [Fact]
        public void WithSecurityTokenAndDelegateWithoutSecurityToken()
        {
            var user = this.BuildPerson("user");

            var delegatedAccessClass = this.Transaction.Build<AccessClass>();
            var accessClass = this.Transaction.Build<AccessClass>(v => v.DelegatedAccess = delegatedAccessClass);

            var securityToken = this.BuildSecurityToken();
            var permission = this.FindPermission(this.M.AccessClass.Property, Operations.Read);
            var role = this.BuildRole("Role", permission);

            var grant = this.BuildGrant(user, role);
            securityToken.AddGrant(grant);

            accessClass.AddSecurityToken(securityToken);

            this.Transaction.Derive();
            this.Transaction.Commit();

            // Use default security from Singleton
            var acl = new DatabaseAccessControl(this.Security, user)[accessClass];
            Assert.True(acl.CanRead(this.M.AccessClass.Property));
            Assert.True(acl.CanRead(this.M.AccessClass.Property));

            Assert.False(acl.CanRead(this.M.AccessClass.AnotherProperty));
            Assert.False(acl.CanRead(this.M.AccessClass.AnotherProperty));
        }

        [Fact]
        public void WithoutSecurityTokenAndDelegateWithoutSecurityToken()
        {
            var user = this.BuildPerson("user");

            var delegatedAccessClass = this.Transaction.Build<AccessClass>();
            var accessClass = this.Transaction.Build<AccessClass>(v => v.DelegatedAccess = delegatedAccessClass);

            var securityToken = this.BuildSecurityToken();
            var permission = this.FindPermission(this.M.AccessClass.Property, Operations.Read);
            var role = this.BuildRole("Role", permission);

            var grant = this.BuildGrant(user, role);
            securityToken.AddGrant(grant);

            this.Transaction.Derive();
            this.Transaction.Commit();

            // Use default security from Singleton
            var acl = new DatabaseAccessControl(this.Security, user)[accessClass];
            Assert.False(acl.CanRead(this.M.AccessClass.Property));
            Assert.False(acl.CanRead(this.M.AccessClass.Property));

            Assert.False(acl.CanRead(this.M.AccessClass.AnotherProperty));
            Assert.False(acl.CanRead(this.M.AccessClass.AnotherProperty));
        }

        [Fact]
        public void WithSecurityTokenAndDelegateWithSecurityToken()
        {
            var user = this.BuildPerson("user");

            var delegatedAccessClass = this.Transaction.Build<AccessClass>();
            var accessClass = this.Transaction.Build<AccessClass>(v => v.DelegatedAccess = delegatedAccessClass);

            var securityToken1 = this.BuildSecurityToken();
            var permission1 = this.FindPermission(this.M.AccessClass.Property, Operations.Read);
            var role1 = this.BuildRole("Role", permission1);

            securityToken1.AddGrant(this.BuildGrant(user, role1));

            var securityToken2 = this.BuildSecurityToken();
            var permission2 = this.FindPermission(this.M.AccessClass.AnotherProperty, Operations.Read);
            var role2 = this.BuildRole("Role", permission2);

            securityToken2.AddGrant(this.BuildGrant(user, role2));

            accessClass.AddSecurityToken(securityToken1);
            delegatedAccessClass.AddSecurityToken(securityToken2);

            this.Transaction.Derive();
            this.Transaction.Commit();

            // Use default security from Singleton
            var acl = new DatabaseAccessControl(this.Security, user)[accessClass];
            Assert.True(acl.CanRead(this.M.AccessClass.Property));
            Assert.True(acl.CanRead(this.M.AccessClass.Property));

            Assert.True(acl.CanRead(this.M.AccessClass.AnotherProperty));
            Assert.True(acl.CanRead(this.M.AccessClass.AnotherProperty));
        }

        [Fact]
        public void WithoutSecurityTokenAndDelegateWithSecurityToken()
        {
            var user = this.BuildPerson("user");

            var delegatedAccessClass = this.Transaction.Build<AccessClass>();
            var accessClass = this.Transaction.Build<AccessClass>(v => v.DelegatedAccess = delegatedAccessClass);

            var securityToken = this.BuildSecurityToken();
            var permission = this.FindPermission(this.M.AccessClass.Property, Operations.Read);
            var role = this.BuildRole("Role", permission);

            var grant = this.BuildGrant(user, role);
            securityToken.AddGrant(grant);

            delegatedAccessClass.AddSecurityToken(securityToken);

            this.Transaction.Derive();
            this.Transaction.Commit();

            // Use default security from Singleton
            var acl = new DatabaseAccessControl(this.Security, user)[accessClass];
            Assert.True(acl.CanRead(this.M.AccessClass.Property));
            Assert.True(acl.CanRead(this.M.AccessClass.Property));

            Assert.False(acl.CanRead(this.M.AccessClass.AnotherProperty));
            Assert.False(acl.CanRead(this.M.AccessClass.AnotherProperty));
        }
    }
}
