namespace Allors.Embedded.Tests
{
    using Domain;

    public class UnitRoleTests : Tests
    {
        [Test]
        public void String()
        {
            var john = this.Population.New<Person>();
            var jane = this.Population.New<Person>();
            
            john.Name = "John";
            jane.Name = "Jane";

            Assert.Multiple(() =>
            {
                Assert.That(john.Name, Is.EqualTo("John"));
                Assert.That(jane.Name, Is.EqualTo("Jane"));
            });
        }
    }
}
