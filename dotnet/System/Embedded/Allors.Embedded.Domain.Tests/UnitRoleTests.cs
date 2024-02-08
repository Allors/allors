namespace Allors.Embedded.Tests
{
    using Domain;

    public class UnitRoleTests : Tests
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
        public void String()
        {
            var john = this.Population.Create<Person>();
            var jane = this.Population.Create<Person>();
            
            john.Name.Value = "John";
            jane.Name.Value = "Jane";

            Assert.Multiple(() =>
            {
                Assert.That(john.Name.Value, Is.EqualTo("John"));
                Assert.That(jane.Name.Value, Is.EqualTo("Jane"));
            });
        }
    }
}
