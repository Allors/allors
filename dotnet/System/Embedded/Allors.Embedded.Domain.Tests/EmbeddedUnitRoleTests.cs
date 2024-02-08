namespace Allors.Embedded.Domain.Tests
{
    using Domain;

    public abstract class EmbeddedUnitRoleTests : Tests
    {
        [Test]
        public void String()
        {
            var john = this.Population.EmbeddedCreateObject<Person>();
            var jane = this.Population.EmbeddedCreateObject<Person>();

            john.Name.EmbeddedValue = "John";
            jane.Name.EmbeddedValue = "Jane";

            Assert.Multiple(() =>
            {
                Assert.That(john.Name.EmbeddedValue, Is.EqualTo("John"));
                Assert.That(jane.Name.EmbeddedValue, Is.EqualTo("Jane"));
            });
        }
    }
}
