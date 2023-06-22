namespace Allors.Embedded.Tests
{
    using System;
    using System.Linq;
    using Allors.Embedded.Meta;
    using Allors.Embedded.Tests.Domain;

    public class ObjectsTests : Tests
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
                return (obj) => obj.FirstName = firstName;
            }

            Action<Person> LastName(string lastName)
            {
                return (obj) => obj.LastName = lastName;
            }

            Person NewPerson(string firstName, string lastName)
            {
                return Create(FirstName(firstName), LastName(lastName));
            }

            var jane = NewPerson("Jane", "Doe");
            var john = NewPerson("John", "Doe");
            var jenny = NewPerson("Jenny", "Doe");

            var lastNameDoe = this.Population.Objects.OfType<Person>().Where(v => v.LastName == "Doe").ToArray();

            Assert.AreEqual(3, lastNameDoe.Length);
            Assert.Contains(jane, lastNameDoe);
            Assert.Contains(john, lastNameDoe);
            Assert.Contains(jenny, lastNameDoe);

            var lessThanFourLetterFirstNames = this.Population.Objects.OfType<Person>().Where(v => v.FirstName.Length < 4).ToArray();

            Assert.IsEmpty(lessThanFourLetterFirstNames);

            var fourLetterFirstNames = this.Population.Objects.OfType<Person>().Where(v => v.FirstName.Length == 4).ToArray();

            Assert.AreEqual(2, fourLetterFirstNames.Length);
            Assert.Contains(jane, fourLetterFirstNames);
            Assert.Contains(john, fourLetterFirstNames);

            var fiveLetterFirstNames = this.Population.Objects.OfType<Person>().Where(v => v.FirstName.Length == 5).ToArray();
            Assert.AreEqual(1, fiveLetterFirstNames.Length);
            Assert.Contains(jenny, fiveLetterFirstNames);
        }
    }
}
