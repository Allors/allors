﻿namespace Allors.Embedded.Tests
{
    using Allors.Embedded.Tests.Domain;

    public class EmbeddedPopulationTests : Tests
    {
        [Test]
        public void New()
        {
            New<Organization> newOrganization = this.Population.New;
            New<Person> newPerson = this.Population.New;

            var acme = newOrganization(v =>
            {
                v.Name.Value = "Acme";
                v.Owner.Value = newPerson(v => v.Name.Value = "Jane");
            });

            var jane = acme.Owner;

            Assert.That(acme.Name.Value, Is.EqualTo("Acme"));
            Assert.That(jane.Value.Name.Value, Is.EqualTo("Jane"));

            Assert.That(jane.Value.OrganizationWhereOwner.Value, Is.EqualTo(acme));
        }
    }
}
