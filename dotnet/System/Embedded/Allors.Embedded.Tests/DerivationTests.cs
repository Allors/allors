namespace Allors.Embedded.Tests
{
    using System;
    using System.Linq;
    using Allors.Embedded.Meta;
    using Allors.Embedded.Tests.Domain;

    public class DerivationTests : Tests
    {
        [Test]
        public void Derivation()
        {

            this.Population.DerivationById["FullName"] = new FullNameDerivation();

            var john = this.Population.New<Person>();
            john.FirstName = "John";
            john.LastName = "Doe";

            this.Population.Derive();

            Assert.AreEqual("John Doe", john.FullName);

            this.Population.DerivationById["FullName"] = new GreetingDerivation(this.Population.DerivationById["FullName"]);

            var jane = this.Population.New<Person>();
            jane.FirstName = "Jane";
            jane.LastName = "Doe";

            this.Population.Derive();

            Assert.AreEqual("Jane Doe Chained", jane.FullName);
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
                        person.FirstName = person.FirstName;
                        person.LastName = person.LastName;

                        person.DerivedAt = DateTime.Now;

                        person.FullName = $"{person.FirstName} {person.LastName}";
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
                        person.FullName = $"{person.FullName} Chained";
                    }
                }
            }
        }
    }
}
