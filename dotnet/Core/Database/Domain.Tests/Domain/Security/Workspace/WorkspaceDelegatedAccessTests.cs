// <copyright file="DelegateAccessTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain.Tests
{
    using Xunit;

    public class WorkspaceDelegatedAccessTests : DomainTest, IClassFixture<Fixture>
    {
        private const string WorkspaceName = "Default";

        public WorkspaceDelegatedAccessTests(Fixture fixture) : base(fixture) { }

        public override Config Config => new Config { SetupSecurity = true };

        [Fact]
        public void WithSecurityTokenAndDelegateWithoutSecurityToken()
        {
            var user = this.BuildPerson("user");

            var delegatedAccess = this.Transaction.Build<DelegatedAccess>();
            var accessClass = this.Transaction.Build<AccessClass>(v => v.AccessDelegation = delegatedAccess);

            var securityToken = this.BuildSecurityToken();
            var permission = this.FindPermission(this.M.AccessClass.Property, Operations.Read);
            var role = this.BuildRole("Role", permission);

            var grant = this.BuildGrant(user, role);
            securityToken.AddGrant(grant);

            accessClass.AddSecurityToken(securityToken);

            this.Transaction.Derive();
            this.Transaction.Commit();

            // Use default security from Singleton
            var acl = new WorkspaceAccessControl(this.Security, user, null, WorkspaceName)[accessClass];
            Assert.True(acl.CanRead(this.M.AccessClass.Property));
            Assert.True(acl.CanRead(this.M.AccessClass.Property));

            Assert.False(acl.CanRead(this.M.AccessClass.AnotherProperty));
            Assert.False(acl.CanRead(this.M.AccessClass.AnotherProperty));
        }

        [Fact]
        public void WithoutSecurityTokenAndAccessDelegationWithoutSecurityToken()
        {
            var user = this.BuildPerson("user");

            var delegatedAccess = this.Transaction.Build<DelegatedAccess>();
            var accessClass = this.Transaction.Build<AccessClass>(v => v.AccessDelegation = delegatedAccess);

            var securityToken = this.BuildSecurityToken();
            var permission = this.FindPermission(this.M.AccessClass.Property, Operations.Read);
            var role = this.BuildRole("Role", permission);

            var grant = this.BuildGrant(user, role);
            securityToken.AddGrant(grant);

            this.Transaction.Derive();
            this.Transaction.Commit();

            // Use default security from Singleton
            var acl = new WorkspaceAccessControl(this.Security, user, null, WorkspaceName)[accessClass];
            Assert.False(acl.CanRead(this.M.AccessClass.Property));
            Assert.False(acl.CanRead(this.M.AccessClass.Property));

            Assert.False(acl.CanRead(this.M.AccessClass.AnotherProperty));
            Assert.False(acl.CanRead(this.M.AccessClass.AnotherProperty));
        }

        [Fact]
        public void WithSecurityTokenAndAccessDelegationWithSecurityToken()
        {
            var user = this.BuildPerson("user");

            var delegatedAccess = this.Transaction.Build<DelegatedAccess>();
            var accessClass = this.Transaction.Build<AccessClass>(v => v.AccessDelegation = delegatedAccess);

            var securityToken1 = this.BuildSecurityToken();
            var permission1 = this.FindPermission(this.M.AccessClass.Property, Operations.Read);
            var role1 = this.BuildRole("Role", permission1);

            securityToken1.AddGrant(this.BuildGrant(user, role1));

            var securityToken2 = this.BuildSecurityToken();
            var permission2 = this.FindPermission(this.M.AccessClass.AnotherProperty, Operations.Read);
            var role2 = this.BuildRole("Role", permission2);

            securityToken2.AddGrant(this.BuildGrant(user, role2));

            accessClass.AddSecurityToken(securityToken1);
            delegatedAccess.AddDelegatedSecurityToken(securityToken2);

            this.Transaction.Derive();
            this.Transaction.Commit();

            // Use default security from Singleton
            var acl = new WorkspaceAccessControl(this.Security, user, null, WorkspaceName)[accessClass];
            Assert.True(acl.CanRead(this.M.AccessClass.Property));
            Assert.True(acl.CanRead(this.M.AccessClass.Property));

            Assert.True(acl.CanRead(this.M.AccessClass.AnotherProperty));
            Assert.True(acl.CanRead(this.M.AccessClass.AnotherProperty));
        }

        [Fact]
        public void WithoutSecurityTokenAndAccessDelegationWithSecurityToken()
        {
            var user = this.BuildPerson("user");

            var delegatedAccess = this.Transaction.Build<DelegatedAccess>();
            var accessClass = this.Transaction.Build<AccessClass>(v => v.AccessDelegation = delegatedAccess);

            var securityToken = this.BuildSecurityToken();
            var permission = this.FindPermission(this.M.AccessClass.Property, Operations.Read);
            var role = this.BuildRole("Role", permission);

            var grant = this.BuildGrant(user, role);
            securityToken.AddGrant(grant);

            delegatedAccess.AddDelegatedSecurityToken(securityToken);

            this.Transaction.Derive();
            this.Transaction.Commit();

            // Use default security from Singleton
            var acl = new WorkspaceAccessControl(this.Security, user, null, WorkspaceName)[accessClass];
            Assert.True(acl.CanRead(this.M.AccessClass.Property));
            Assert.True(acl.CanRead(this.M.AccessClass.Property));

            Assert.False(acl.CanRead(this.M.AccessClass.AnotherProperty));
            Assert.False(acl.CanRead(this.M.AccessClass.AnotherProperty));
        }

        [Fact]
        public void WithRevocationAndDelegateWithoutRevocation()
        {
            var user = this.BuildPerson("user");

            var delegatedAccess = this.Transaction.Build<DelegatedAccess>();
            var accessClass = this.Transaction.Build<AccessClass>(v => v.AccessDelegation = delegatedAccess);

            var securityToken = this.BuildSecurityToken();
            var permission = this.FindPermission(this.M.AccessClass.Property, Operations.Read);
            var role = this.BuildRole("Role", permission);

            var grant = this.BuildGrant(user, role);
            securityToken.AddGrant(grant);

            accessClass.AddSecurityToken(securityToken);

            var revocation = this.BuildRevocation(permission);
            accessClass.AddRevocation(revocation);

            this.Transaction.Derive();
            this.Transaction.Commit();

            // Use default security from Singleton
            var acl = new WorkspaceAccessControl(this.Security, user, null, WorkspaceName)[accessClass];
            Assert.False(acl.CanRead(this.M.AccessClass.Property));
            Assert.False(acl.CanRead(this.M.AccessClass.Property));
        }

        [Fact]
        public void WithoutRevocationAndDelegateWithoutRevocation()
        {
            var user = this.BuildPerson("user");

            var delegatedAccess = this.Transaction.Build<DelegatedAccess>();
            var accessClass = this.Transaction.Build<AccessClass>(v => v.AccessDelegation = delegatedAccess);

            var securityToken = this.BuildSecurityToken();
            var permission = this.FindPermission(this.M.AccessClass.Property, Operations.Read);
            var role = this.BuildRole("Role", permission);

            var grant = this.BuildGrant(user, role);
            securityToken.AddGrant(grant);

            accessClass.AddSecurityToken(securityToken);

            var revocation = this.BuildRevocation(permission);

            this.Transaction.Derive();
            this.Transaction.Commit();

            // Use default security from Singleton
            var acl = new WorkspaceAccessControl(this.Security, user, null, WorkspaceName)[accessClass];
            Assert.True(acl.CanRead(this.M.AccessClass.Property));
            Assert.True(acl.CanRead(this.M.AccessClass.Property));
        }

        [Fact]
        public void WithRevocationAndDelegateWithRevocation()
        {
            var user = this.BuildPerson("user");

            var delegatedAccess = this.Transaction.Build<DelegatedAccess>();
            var accessClass = this.Transaction.Build<AccessClass>(v => v.AccessDelegation = delegatedAccess);

            var securityToken = this.BuildSecurityToken();
            var permission = this.FindPermission(this.M.AccessClass.Property, Operations.Read);
            var role = this.BuildRole("Role", permission);

            var grant = this.BuildGrant(user, role);
            securityToken.AddGrant(grant);

            accessClass.AddSecurityToken(securityToken);

            var revocation = this.BuildRevocation(permission);

            accessClass.AddRevocation(revocation);
            delegatedAccess.AddDelegatedRevocation(revocation);

            this.Transaction.Derive();
            this.Transaction.Commit();

            // Use default security from Singleton
            var acl = new WorkspaceAccessControl(this.Security, user, null, WorkspaceName)[accessClass];
            Assert.False(acl.CanRead(this.M.AccessClass.Property));
            Assert.False(acl.CanRead(this.M.AccessClass.Property));
        }

        [Fact]
        public void WithoutRevocationAndDelegateWithRevocation()
        {
            var user = this.BuildPerson("user");

            var delegatedAccess = this.Transaction.Build<DelegatedAccess>();
            var accessClass = this.Transaction.Build<AccessClass>(v => v.AccessDelegation = delegatedAccess);

            var securityToken = this.BuildSecurityToken();
            var permission = this.FindPermission(this.M.AccessClass.Property, Operations.Read);
            var role = this.BuildRole("Role", permission);

            var grant = this.BuildGrant(user, role);
            securityToken.AddGrant(grant);

            accessClass.AddSecurityToken(securityToken);

            var revocation = this.BuildRevocation(permission);

            delegatedAccess.AddDelegatedRevocation(revocation);

            this.Transaction.Derive();
            this.Transaction.Commit();

            // Use default security from Singleton
            var acl = new WorkspaceAccessControl(this.Security, user, null, WorkspaceName)[accessClass];
            Assert.False(acl.CanRead(this.M.AccessClass.Property));
            Assert.False(acl.CanRead(this.M.AccessClass.Property));
        }
    }
}
