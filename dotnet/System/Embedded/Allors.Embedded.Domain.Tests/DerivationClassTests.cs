namespace Allors.Embedded.Tests
{
    using System;
    using System.Linq;
    using Allors.Embedded.Tests.Domain;

    public abstract class DerivationClassTests : Tests
    {
        [Test]
        public void String()
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

            Assert.That(john.FullName.Value, Is.EqualTo("John Doe"));
            Assert.That(jane.FullName.Value, Is.EqualTo("Jane Doe Chained"));
        }


        [Test]
        public void ArrayOfString()
        {
            this.Population.DerivationById["Aliases"] = new AliasesDerivation();

            var acme = this.Population.New<Organization>();

            this.Population.Derive();

            Assert.That(acme.DisplayAliases.Value, Is.EqualTo("Nada"));

            acme.Aliases.Value = new string[] { "Bim", "Bam", "Bom" };

            this.Population.Derive();

            Assert.That(acme.DisplayAliases.Value, Is.EqualTo("Bim, Bam, Bom"));

            acme.Aliases.Value = null;

            this.Population.Derive();

            Assert.That(acme.DisplayAliases.Value, Is.EqualTo("Nada"));
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
            private readonly IEmbeddedDerivation derivation;

            public GreetingDerivation(IEmbeddedDerivation derivation)
            {
                this.derivation = derivation;
            }

            public void Derive(IEmbeddedChangeSet changeSet)
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

        public class AliasesDerivation : IEmbeddedDerivation
        {
            public void Derive(IEmbeddedChangeSet changeSet)
            {
                var created = changeSet.NewObjects.OfType<Organization>();
                var changed = changeSet.ChangedRoles<Organization>("Aliases").Select(v => v.Key).Distinct().Cast<Organization>();

                foreach (var organization in created.Union(changed))
                {
                    // Dummy updates ...
                    organization.Aliases.Value = organization.Aliases.Value;

                    organization.DisplayAliases.Value = organization.Aliases.Value?.Length > 0 ? string.Join(", ", organization.Aliases.Value) : "Nada";
                }
            }
        }
    }
}
