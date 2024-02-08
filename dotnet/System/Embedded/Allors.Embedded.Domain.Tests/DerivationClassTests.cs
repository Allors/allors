namespace Allors.Embedded.Domain.Tests
{
    using System;
    using System.Linq;
    
    public abstract class DerivationClassTests : Tests
    {
        [Test]
        public void String()
        {
            this.Population.EmbeddedDerivationById["FullName"] = new FullNameDerivation();

            var john = this.Population.EmbeddedCreateObject<Person>();
            john.FirstName.EmbeddedValue = "John";
            john.LastName.EmbeddedValue = "Doe";

            this.Population.EmbeddedDerive();

            Assert.That(john.FullName.EmbeddedValue, Is.EqualTo("John Doe"));

            this.Population.EmbeddedDerivationById["FullName"] = new GreetingDerivation(this.Population.EmbeddedDerivationById["FullName"]);

            var jane = this.Population.EmbeddedCreateObject<Person>();
            jane.FirstName.EmbeddedValue = "Jane";
            jane.LastName.EmbeddedValue = "Doe";

            this.Population.EmbeddedDerive();

            Assert.That(john.FullName.EmbeddedValue, Is.EqualTo("John Doe"));
            Assert.That(jane.FullName.EmbeddedValue, Is.EqualTo("Jane Doe Chained"));
        }


        [Test]
        public void ArrayOfString()
        {
            this.Population.EmbeddedDerivationById["Aliases"] = new AliasesDerivation();

            var acme = this.Population.EmbeddedCreateObject<Organization>();

            this.Population.EmbeddedDerive();

            Assert.That(acme.DisplayAliases.EmbeddedValue, Is.EqualTo("Nada"));

            acme.Aliases.EmbeddedValue = new string[] { "Bim", "Bam", "Bom" };

            this.Population.EmbeddedDerive();

            Assert.That(acme.DisplayAliases.EmbeddedValue, Is.EqualTo("Bim, Bam, Bom"));

            acme.Aliases.EmbeddedValue = null;

            this.Population.EmbeddedDerive();

            Assert.That(acme.DisplayAliases.EmbeddedValue, Is.EqualTo("Nada"));
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
            private readonly IEmbeddedDerivation derivation;

            public GreetingDerivation(IEmbeddedDerivation derivation)
            {
                this.derivation = derivation;
            }

            public void EmbeddedDerive(IEmbeddedChangeSet changeSet)
            {
                this.derivation.EmbeddedDerive(changeSet);

                var firstNames = changeSet.EmbeddedChangedRoles<Person>("FirstName");
                var lastNames = changeSet.EmbeddedChangedRoles<Person>("LastName");

                if (firstNames?.Any() == true || lastNames?.Any() == true)
                {
                    var people = firstNames.Union(lastNames).Select(v => v.Key).Distinct();

                    foreach (var person in people.Cast<Person>())
                    {
                        person.FullName.EmbeddedValue = $"{person.FullName.EmbeddedValue} Chained";
                    }
                }
            }
        }

        public class AliasesDerivation : IEmbeddedDerivation
        {
            public void EmbeddedDerive(IEmbeddedChangeSet changeSet)
            {
                var created = changeSet.EmbeddedCreatedObjects.OfType<Organization>();
                var changed = changeSet.EmbeddedChangedRoles<Organization>("Aliases").Select(v => v.Key).Distinct().Cast<Organization>();

                foreach (var organization in created.Union(changed))
                {
                    // Dummy updates ...
                    organization.Aliases.EmbeddedValue = organization.Aliases.EmbeddedValue;

                    organization.DisplayAliases.EmbeddedValue = organization.Aliases.EmbeddedValue?.Length > 0 ? string.Join(", ", organization.Aliases.EmbeddedValue) : "Nada";
                }
            }
        }
    }
}
