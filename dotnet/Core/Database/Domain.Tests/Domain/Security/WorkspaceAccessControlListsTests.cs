// <copyright file="DatabaseAccessControlListTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain.Tests
{
    using System.Linq;
    using Configuration;
    using Meta;
    using Xunit;
    using Permission = Domain.Permission;
    using Permissions = Domain.Permissions;

    public class WorkspaceAccessControlListsTests : DomainTest, IClassFixture<Fixture>
    {
        private string workspaceName = "Default";

        public WorkspaceAccessControlListsTests(Fixture fixture) : base(fixture) { }

        public override Config Config => new Config { SetupSecurity = true };

        [Fact]
        public void InitialWithoutAccessControl()
        {
            var person = this.BuildPerson("John", "Doe");

            this.Transaction.Derive();
            this.Transaction.Commit();

            var organisation = this.BuildOrganisation("Organisation");

            var aclService = new WorkspaceAclsService(this.Security, new WorkspaceMask(this.M), person);
            var acl = aclService.Create(this.workspaceName)[organisation];

            Assert.False(acl.CanRead(this.M.Organisation.Name));
        }

        [Fact]
        public void Initial()
        {
            var permission = this.FindPermission(this.M.Organisation.Name, Operations.Read);
            var role = this.BuildRole("Role", permission);
            var person = this.BuildPerson("John", "Doe");
            var grant = this.BuildGrant(person, role);

            var initialSecurityToken = new SecurityTokens(this.Transaction).InitialSecurityToken;
            initialSecurityToken.AddGrant(grant);

            this.Transaction.Derive();
            this.Transaction.Commit();

            var organisation = this.BuildOrganisation("Organisation");

            var aclService = new WorkspaceAclsService(this.Security, new WorkspaceMask(this.M), person);
            var acl = aclService.Create(this.workspaceName)[organisation];

            Assert.True(acl.CanRead(this.M.Organisation.Name));
        }

        [Fact]
        public void GivenAWorkspaceAccessControlListsThenADatabaseDeniedPermissionsIsNotPresent()
        {
            var administrator = this.BuildPerson("administrator");
            var administrators = new UserGroups(this.Transaction).Administrators;
            administrators.AddMember(administrator);

            var databaseOnlyPermissions = new Permissions(this.Transaction).Extent().Where(v => v.ExistOperandType && v.OperandType.Equals(M.Person.DatabaseOnlyField));
            var databaseOnlyReadPermission = databaseOnlyPermissions.First(v => v.Operation == Operations.Read);

            var revocation = this.BuildRevocation(databaseOnlyReadPermission);

            administrator.AddRevocation(revocation);

            this.Transaction.Derive();
            this.Transaction.Commit();

            var aclService = new WorkspaceAclsService(this.Security, new WorkspaceMask(this.M), administrator);
            var acl = aclService.Create(this.workspaceName)[administrator];

            Assert.DoesNotContain(revocation.Id, acl.Revocations.Select(v => v.Id));
        }

        [Fact]
        public void GivenAWorkspaceAccessControlListsThenAWorkspaceDeniedPermissionsIsNotPresent()
        {
            var administrator = this.BuildPerson("administrator");
            var administrators = new UserGroups(this.Transaction).Administrators;
            administrators.AddMember(administrator);

            var workspacePermissions = new Permissions(this.Transaction).Extent().Where(v => v.ExistOperandType && v.OperandType.Equals(M.Person.DefaultWorkspaceField));
            var workspaceReadPermission = workspacePermissions.First(v => v.Operation == Operations.Read);
            var revocation = this.BuildRevocation(workspaceReadPermission);

            administrator.AddRevocation(revocation);

            this.Transaction.Derive();
            this.Transaction.Commit();

            var aclService = new WorkspaceAclsService(this.Security, new WorkspaceMask(this.M), administrator);
            var acl = aclService.Create(this.workspaceName)[administrator];

            Assert.Contains(revocation.Id, acl.Revocations.Select(v => v.Id));
        }

        [Fact]
        public void GivenAWorkspaceAccessControlListsThenAnotherWorkspaceDeniedPermissionsIsNotPresent()
        {
            var administrator = this.BuildPerson("administrator");
            var administrators = new UserGroups(this.Transaction).Administrators;
            administrators.AddMember(administrator);

            var workspacePermissions = new Permissions(this.Transaction).Extent().Where(v => v.ExistOperandType && v.OperandType.Equals(M.Person.DefaultWorkspaceField));
            var workspaceReadPermission = workspacePermissions.First(v => v.Operation == Operations.Read);
            var revocation = this.BuildRevocation(workspaceReadPermission);

            administrator.AddRevocation(revocation);

            this.Transaction.Derive();
            this.Transaction.Commit();

            var aclService = new WorkspaceAclsService(this.Security, new WorkspaceMask(this.M), administrator);
            var acl = aclService.Create("X")[administrator];

            Assert.DoesNotContain(revocation.Id, acl.Revocations.Select(v => v.Id));
        }
    }
}
