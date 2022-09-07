// <copyright file="DatabaseAccessControlListTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain.Tests
{
    using System.Collections.Generic;
    using Configuration;
    using Meta;
    using Xunit;

    public class WorkspaceMaskTests : DomainTest, IClassFixture<Fixture>
    {
        private string workspaceName = "Default";

        public WorkspaceMaskTests(Fixture fixture) : base(fixture) { }

        public override Config Config => new Config { SetupSecurity = true };

        [Fact]
        public void WithoutMask()
        {
            var permission = this.FindPermission(this.M.Organization.Name, Operations.Read);
            var role = this.BuildRole("Role", permission);
            var person = this.BuildPerson("John", "Doe");
            var accessControl = this.BuildGrant(person, role);

            var intialSecurityToken = new SecurityTokens(this.Transaction).InitialSecurityToken;
            intialSecurityToken.AddGrant(accessControl);

            this.Transaction.Derive();
            this.Transaction.Commit();

            var organization = this.BuildOrganization("Organization");

            var aclService = new WorkspaceAclsService(this.Security, new WorkspaceMask(this.M), person);
            var acl = aclService.Create(this.workspaceName)[organization];

            Assert.False(acl.IsMasked());
        }

        [Fact]
        public void WithMask()
        {
            var person = this.BuildPerson("John", "Doe");

            this.Transaction.Derive();
            this.Transaction.Commit();

            var organization = this.BuildOrganization("Organization");

            var aclService = new WorkspaceAclsService(this.Security, new WorkspaceMask(this.M), person);
            var acl = aclService.Create(this.workspaceName)[organization];

            var canRead = acl.CanRead(this.M.Organization.Name);

            Assert.True(acl.IsMasked());
        }

        private class WorkspaceMask : IWorkspaceMask
        {
            private readonly Dictionary<IClass, IRoleType> masks;

            public WorkspaceMask(M m) =>
                this.masks = new Dictionary<IClass, IRoleType>
                {
                    {m.Organization, m.Organization.Name},
                };

            public IDictionary<IClass, IRoleType> GetMasks(string workspaceName) => this.masks;
        }
    }
}
