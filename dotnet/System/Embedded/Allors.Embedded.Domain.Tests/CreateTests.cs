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
                v.Name.Value = "Acme";
                v.Owner.Value = createPerson(v => v.Name.Value = "Jane");
            });

            var jane = acme.Owner;

            Assert.That(acme.Name.Value, Is.EqualTo("Acme"));
            Assert.That(jane.Value.Name.Value, Is.EqualTo("Jane"));

            Assert.That(jane.Value.OrganizationWhereOwner.Value, Is.EqualTo(acme));
        }
    }
}
