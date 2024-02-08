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
