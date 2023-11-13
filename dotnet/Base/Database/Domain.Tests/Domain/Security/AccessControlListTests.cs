// <copyright file="AccessControlListTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain.Tests
{
    using Allors;
    using Database;
    using Domain;
    using Meta;
    using Xunit;
    using Object = Domain.Object;
    using Permission = Domain.Permission;

    public class AccessControlListTests : DomainTest, IClassFixture<Fixture>
    {
        public AccessControlListTests(Fixture fixture) : base(fixture) { }

        public override Config Config => new Config { SetupSecurity = true };

        [Fact]
        public void GivenAnAuthenticationPopulationWhenCreatingAnAccessListForGuestThenPermissionIsDenied()
        {
            this.Transaction.Derive();
            this.Transaction.Commit();

            var automatedAgents = this.Transaction.Scoped<AutomatedAgentByUniqueId>();

            var transactions = new[] { this.Transaction };
            foreach (var transaction in transactions)
            {
                transaction.Commit();

                var guest = automatedAgents.Guest;
                var acls = new DatabaseAccessControl(this.Security, guest);
                foreach (Object aco in (IObject[])transaction.Extent(this.M.Organisation))
                {
                    // When
                    var accessList = acls[aco];

                    // Then
                    Assert.False(accessList.CanExecute(this.M.Organisation.JustDoIt));
                }

                transaction.Rollback();
            }
        }

        private Permission FindPermission(RoleType roleType, Operations operation)
        {
            var permissionByMeta = this.Transaction.Scoped<PermissionByMeta>();

            var objectType = (Class)roleType.AssociationType.ObjectType;
            return permissionByMeta.Get(objectType, roleType, operation);
        }
    }
}
