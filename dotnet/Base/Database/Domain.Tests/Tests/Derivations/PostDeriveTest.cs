// <copyright file="RequiredTest.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//
// </summary>

namespace Allors.Database.Domain.Tests
{
    using Xunit;

    public class PostDeriveTest : DomainTest, IClassFixture<Fixture>
    {
        public PostDeriveTest(Fixture fixture) : base(fixture, false) { }

        [Fact]
        public void CycleAgain()
        {
            var organization = this.BuildOrganization("Acme");

            Assert.False(organization.PostDeriveTrigger);
            Assert.False(organization.PostDeriveTriggered);

            this.Transaction.Derive(false);

            Assert.True(organization.PostDeriveTrigger);
            Assert.True(organization.PostDeriveTriggered);
        }
    }
}
