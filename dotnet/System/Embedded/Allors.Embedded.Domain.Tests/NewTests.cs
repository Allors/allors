namespace Allors.Embedded.Tests
{
    using Allors.Embedded.Tests.Domain;

    public class NewTests : Tests
    {
        private EmbeddedPopulation population = null!;

        public override EmbeddedPopulation Population => population;

        [SetUp]
        public override void SetUp()
        {
            this.population = new EmbeddedPopulation();

            base.SetUp();
        }

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
