namespace Allors.Embedded.Domain.Tests
{
    using System;
    using System.Linq;
    using Domain;

    public abstract class DerivationOverrideTests : Tests
    {
        [Test]
        public void Derivation()
        {
            this.Population.EmbeddedDerivationById["FullName"] = new FullNameDerivation();
            this.Population.EmbeddedDerivationById["Greeting"] = new GreetingDerivation();

            var john = this.Population.EmbeddedCreateObject<Person>();
            john.FirstName.EmbeddedValue = "John";
            john.LastName.EmbeddedValue = "Doe";

            this.Population.EmbeddedDerive();

            Assert.That(john.Greeting.EmbeddedValue, Is.EqualTo("Hello John Doe!"));
        }

        public class FullNameDerivation : IEmbeddedDerivation
        {
            public void EmbeddedDerive(IEmbeddedChangeSet changeSet)
            {
                var firstNames = changeSet.EmbeddedChangedRoles<Person>("FirstName");
                var lastNames = changeSet.EmbeddedChangedRoles<Person>("LastName");

                if (firstNames.Any() || lastNames.Any())
                {
                    var people = firstNames.Union(lastNames).Select(v => v.Key).Distinct();

                    foreach (var person in people.Cast<Person>())
                    {
                        // Dummy updates ...
                        person.FirstName.EmbeddedValue = person.FirstName.EmbeddedValue;
                        person.LastName.EmbeddedValue = person.LastName.EmbeddedValue;

                        person.DerivedAt.EmbeddedValue = DateTime.Now;

                        person.FullName.EmbeddedValue = $"{person.FirstName.EmbeddedValue} {person.LastName.EmbeddedValue}";
                    }
                }
            }
        }

        public class GreetingDerivation : IEmbeddedDerivation
        {
            public void EmbeddedDerive(IEmbeddedChangeSet changeSet)
            {
                var fullNames = changeSet.EmbeddedChangedRoles<Person>("FullName");

                if (fullNames?.Any() == true)
                {
                    var people = fullNames.Select(v => v.Key).Distinct();

                    foreach (var person in people.Cast<Person>())
                    {
                        person.Greeting.EmbeddedValue = $"Hello {person.FullName.EmbeddedValue}!";
                    }
                }
            }
        }
    }
}
