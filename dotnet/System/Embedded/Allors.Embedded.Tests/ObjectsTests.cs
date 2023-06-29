namespace Allors.Embedded.Tests
{
    using System;
    using System.Linq;
    using Allors.Embedded.Tests.Domain;

    public abstract class ObjectsTests : Tests
    {
        [Test]
        public void Filter()
        {
            T Create<T>(params Action<T>[] builders)
                 where T : EmbeddedObject
            {
                return this.Population.New<T>(builders);
            }

            Action<Person> FirstName(string firstName)
            {
                return (obj) => obj.FirstName.Value = firstName;
            }

            Action<Person> LastName(string lastName)
            {
                return (obj) => obj.LastName.Value = lastName;
            }

            Person NewPerson(string firstName, string lastName)
            {
                return Create(FirstName(firstName), LastName(lastName));
            }

            var jane = NewPerson("Jane", "Doe");
            var john = NewPerson("John", "Doe");
            var jenny = NewPerson("Jenny", "Doe");

            var lastNameDoe = this.Population.Objects.OfType<Person>().Where(v => v.LastName.Value == "Doe").ToArray();

            Assert.That(lastNameDoe.Length, Is.EqualTo(3));
            Assert.Contains(jane, lastNameDoe);
            Assert.Contains(john, lastNameDoe);
            Assert.Contains(jenny, lastNameDoe);

            var lessThanFourLetterFirstNames = this.Population.Objects.OfType<Person>().Where(v => v.FirstName.Value.Length < 4).ToArray();

            Assert.IsEmpty(lessThanFourLetterFirstNames);

            var fourLetterFirstNames = this.Population.Objects.OfType<Person>().Where(v => v.FirstName.Value.Length == 4).ToArray();

            Assert.That(fourLetterFirstNames.Length, Is.EqualTo(2));
            Assert.Contains(jane, fourLetterFirstNames);
            Assert.Contains(john, fourLetterFirstNames);

            var fiveLetterFirstNames = this.Population.Objects.OfType<Person>().Where(v => v.FirstName.Value.Length == 5).ToArray();
            Assert.That(fiveLetterFirstNames.Length, Is.EqualTo(1));
            Assert.Contains(jenny, fiveLetterFirstNames);
        }
    }
}
