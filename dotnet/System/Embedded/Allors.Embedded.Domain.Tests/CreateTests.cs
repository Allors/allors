namespace Allors.Embedded.Domain.Tests
{
    public abstract class CreateTests : Tests
    {
        [Test]
        public void Create()
        {
            Create<Organization> createOrganization = this.Population.EmbeddedCreateObject;
            Create<Person> createPerson = this.Population.EmbeddedCreateObject;

            var acme = createOrganization(v =>
            {
                v.Name.EmbeddedValue = "Acme";
                v.Owner.EmbeddedValue = createPerson(v => v.Name.EmbeddedValue = "Jane");
            });

            var jane = acme.Owner;

            Assert.That(acme.Name.EmbeddedValue, Is.EqualTo("Acme"));
            Assert.That(jane.EmbeddedValue.Name.EmbeddedValue, Is.EqualTo("Jane"));

            Assert.That(jane.EmbeddedValue.OrganizationWhereOwner.EmbeddedValue, Is.EqualTo(acme));
        }
    }
}
