namespace Allors.Embedded.Tests
{
    using Allors.Embedded.Meta;
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
                v.Name = "Acme";
                v.Owner = newPerson(v => v.Name = "Jane");
            });

            var jane = acme.Owner;

            Assert.AreEqual("Acme", acme.Name);
            Assert.AreEqual("Jane", jane.Name);

            Assert.AreEqual(acme, jane.OrganizationWhereOwner);
        }
    }
}
