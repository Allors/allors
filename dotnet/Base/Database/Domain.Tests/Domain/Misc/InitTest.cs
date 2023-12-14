// <copyright file="InitTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain.Tests
{
    using Xunit;

    public class InitTest : DomainTest, IClassFixture<Fixture>
    {
        public InitTest(Fixture fixture) : base(fixture) { }

        [Fact]
        public void Init()
        {
            var allors = this.BuildOrganization("Allors");
            var acme = this.BuildOrganization("Acme");
            var person = this.Transaction.Build<Person>(v => v.LastName = "Hesius");

            allors.Manager = person;

            var derivation = this.Transaction.Derive();

            Assert.Contains(person, allors.Employees);
            Assert.DoesNotContain(person, acme.Employees);

            allors.RemoveManager();
            acme.Manager = person;

            derivation = this.Transaction.Derive();

            Assert.Contains(person, allors.Employees);
            Assert.DoesNotContain(person, acme.Employees);
        }
    }
}
