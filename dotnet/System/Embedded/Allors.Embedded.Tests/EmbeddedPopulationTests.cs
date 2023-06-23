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
                v.Name.Value = "Acme";
                v.Owner = newPerson(v => v.Name.Value = "Jane");
            });

            var jane = acme.Owner;

            Assert.AreEqual("Acme", acme.Name.Value);
            Assert.AreEqual("Jane", jane.Name.Value);

            Assert.AreEqual(acme, jane.OrganizationWhereOwner);
        }
    }
}
