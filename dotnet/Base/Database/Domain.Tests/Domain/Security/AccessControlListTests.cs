// <copyright file="AccessControlListTests.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
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

        public override Config Config => base.Config with { SetupSecurity = true };

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
                foreach (Object aco in (IObject[])transaction.Extent(this.M.Organization))
                {
                    // When
                    var accessList = acls[aco];

                    // Then
                    Assert.False(accessList.CanExecute(this.M.Organization.JustDoIt));
                }

                transaction.Rollback();
            }
        }
    }
}
