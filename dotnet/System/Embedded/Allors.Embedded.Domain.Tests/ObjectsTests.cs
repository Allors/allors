namespace Allors.Embedded.Domain.Tests
{
    using System;
    using System.Linq;
    
    public abstract class ObjectsTests : Tests
    {
        [Test]
        public void Filter()
        {
            T Create<T>(params Action<T>[] builders)
                 where T : EmbeddedObject
            {
                return this.Population.EmbeddedCreateObject<T>(builders);
            }

            Action<Person> FirstName(string firstName)
            {
                return (obj) => obj.FirstName.EmbeddedValue = firstName;
            }

            Action<Person> LastName(string lastName)
            {
                return (obj) => obj.LastName.EmbeddedValue = lastName;
            }

            Person NewPerson(string firstName, string lastName)
            {
                return Create(FirstName(firstName), LastName(lastName));
            }

            var jane = NewPerson("Jane", "Doe");
            var john = NewPerson("John", "Doe");
            var jenny = NewPerson("Jenny", "Doe");

            var lastNameDoe = this.Population.EmbeddedObjects.OfType<Person>().Where(v => v.LastName.EmbeddedValue == "Doe").ToArray();

            Assert.That(lastNameDoe.Length, Is.EqualTo(3));
            Assert.That(lastNameDoe, Does.Contain(jane));
            Assert.That(lastNameDoe, Does.Contain(john));
            Assert.That(lastNameDoe, Does.Contain(jenny));

            var lessThanFourLetterFirstNames = this.Population.EmbeddedObjects.OfType<Person>().Where(v => v.FirstName.EmbeddedValue.Length < 4).ToArray();

            Assert.That(lessThanFourLetterFirstNames, Is.Empty);

            var fourLetterFirstNames = this.Population.EmbeddedObjects.OfType<Person>().Where(v => v.FirstName.EmbeddedValue.Length == 4).ToArray();

            Assert.That(fourLetterFirstNames.Length, Is.EqualTo(2));
            Assert.That(fourLetterFirstNames, Does.Contain(jane));
            Assert.That(fourLetterFirstNames, Does.Contain(john));

            var fiveLetterFirstNames = this.Population.EmbeddedObjects.OfType<Person>().Where(v => v.FirstName.EmbeddedValue.Length == 5).ToArray();
            Assert.That(fiveLetterFirstNames.Length, Is.EqualTo(1));
            Assert.That(fiveLetterFirstNames, Does.Contain(jenny));
        }
    }
}
