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
