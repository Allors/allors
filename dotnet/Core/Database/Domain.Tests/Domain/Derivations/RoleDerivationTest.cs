// <copyright file="DerivationNodesTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//
// </summary>

namespace Allors.Database.Domain.Tests
{
    using Xunit;

    public class RoleDerivationTest : DomainTest, IClassFixture<Fixture>
    {
        public RoleDerivationTest(Fixture fixture) : base(fixture) { }

        [Fact]
        public void RemoveRole()
        {
            var organization = this.BuildOrganization("Acme");

            var jane = this.Transaction.Build<Person>(v =>
            {
                v.FirstName = "Jane";
                v.LastName = "Doe";
            });

            this.Transaction.Derive();

            organization.Owner = jane;

            this.Transaction.Derive();

            Assert.True(jane.Owning);

            organization.RemoveOwner();

            this.Transaction.Derive();

            Assert.False(jane.Owning);
        }
    }
}
