namespace Allors.Embedded.Tests
{
    using System;
    using System.Linq;
    using Domain;

    public class DerivationOverrideTests : Tests
    {
        [Test]
        public void Derivation()
        {
            this.Population.DerivationById["FullName"] = new FullNameDerivation();
            this.Population.DerivationById["Greeting"] = new GreetingDerivation();

            var john = this.Population.New<Person>();
            john.FirstName = "John";
            john.LastName = "Doe";

            this.Population.Derive();

            Assert.AreEqual("Hello John Doe!", john.Greeting);
        }

        public class FullNameDerivation : IEmbeddedDerivation
        {
            public void Derive(EmbeddedChangeSet changeSet)
            {
                var firstNames = changeSet.ChangedRoles<Person>("FirstName");
                var lastNames = changeSet.ChangedRoles<Person>("LastName");

                if (firstNames?.Any() == true || lastNames?.Any() == true)
                {
                    var people = firstNames.Union(lastNames).Select(v => v.Key).Distinct();

                    foreach (dynamic person in people)
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
            public void Derive(EmbeddedChangeSet changeSet)
            {
                var fullNames = changeSet.ChangedRoles<Person>("FullName");

                if (fullNames?.Any() == true)
                {
                    var people = fullNames.Select(v => v.Key).Distinct();

                    foreach (dynamic person in people)
                    {
                        person.Greeting = $"Hello {person.FullName}!";
                    }
                }
            }
        }
    }
}
