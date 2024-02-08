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
            john.FirstName.Value = "John";
            john.LastName.Value = "Doe";

            this.Population.EmbeddedDerive();

            Assert.That(john.Greeting.Value, Is.EqualTo("Hello John Doe!"));
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
                        person.FirstName.Value = person.FirstName.Value;
                        person.LastName.Value = person.LastName.Value;

                        person.DerivedAt.Value = DateTime.Now;

                        person.FullName.Value = $"{person.FirstName.Value} {person.LastName.Value}";
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
                        person.Greeting.Value = $"Hello {person.FullName.Value}!";
                    }
                }
            }
        }
    }
}
