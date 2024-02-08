namespace Allors.Embedded.Tests
{
    using System;
    using System.Linq;
    using Domain;

    public class DerivationOverrideTests : Tests
    {
        private EmbeddedPopulation population = null!;

        public override EmbeddedPopulation Population => population;

        [SetUp]
        public override void SetUp()
        {
            this.population = new EmbeddedPopulation();

            base.SetUp();
        }

        [Test]
        public void Derivation()
        {
            this.Population.DerivationById["FullName"] = new FullNameDerivation();
            this.Population.DerivationById["Greeting"] = new GreetingDerivation();

            var john = this.Population.Create<Person>();
            john.FirstName.Value = "John";
            john.LastName.Value = "Doe";

            this.Population.Derive();

            Assert.That(john.Greeting.Value, Is.EqualTo("Hello John Doe!"));
        }

        public class FullNameDerivation : IEmbeddedDerivation
        {
            public void Derive(IEmbeddedChangeSet changeSet)
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
            public void Derive(IEmbeddedChangeSet changeSet)
            {
                var fullNames = changeSet.ChangedRoles<Person>("FullName");

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
