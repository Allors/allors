namespace Allors.Embedded.Tests
{
    using System;
    using System.Linq;
    using Allors.Embedded.Tests.Domain;

    public class DerivationTests : Tests
    {
        [Test]
        public void Derivation()
        {

            this.Population.DerivationById["FullName"] = new FullNameDerivation();

            var john = this.Population.New<Person>();
            john.FirstName.Value = "John";
            john.LastName.Value = "Doe";

            this.Population.Derive();

            Assert.That(john.FullName.Value, Is.EqualTo("John Doe"));

            this.Population.DerivationById["FullName"] = new GreetingDerivation(this.Population.DerivationById["FullName"]);

            var jane = this.Population.New<Person>();
            jane.FirstName.Value = "Jane";
            jane.LastName.Value = "Doe";

            this.Population.Derive();

            Assert.That(jane.FullName.Value, Is.EqualTo("Jane Doe Chained"));
        }

        public class FullNameDerivation : IEmbeddedDerivation
        {
            public void Derive(EmbeddedChangeSet changeSet)
            {
                var firstNames = changeSet.ChangedRoles<Person>("FirstName");
                var lastNames = changeSet.ChangedRoles<Person>("LastName");

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
            private readonly IEmbeddedDerivation derivation;

            public GreetingDerivation(IEmbeddedDerivation derivation)
            {
                this.derivation = derivation;
            }

            public void Derive(EmbeddedChangeSet changeSet)
            {
                this.derivation.Derive(changeSet);

                var firstNames = changeSet.ChangedRoles<Person>("FirstName");
                var lastNames = changeSet.ChangedRoles<Person>("LastName");

                if (firstNames?.Any() == true || lastNames?.Any() == true)
                {
                    var people = firstNames.Union(lastNames).Select(v => v.Key).Distinct();

                    foreach (var person in people.Cast<Person>())
                    {
                        person.FullName.Value = $"{person.FullName.Value} Chained";
                    }
                }
            }
        }
    }
}
