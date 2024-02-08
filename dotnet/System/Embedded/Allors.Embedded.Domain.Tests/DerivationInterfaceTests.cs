namespace Allors.Embedded.Domain.Tests
{
    using System.Linq;
    
    public abstract class DerivationInterfaceTests : Tests
    {
        [Test]
        public void Derivation()
        {
            this.Population.EmbeddedDerivationById["UppercaseName"] = new UppercaseNameDerivation();

            var john = this.Population.EmbeddedCreateObject<Person>();
            john.Name.EmbeddedValue = "John Doe";

            this.Population.EmbeddedDerive();

            Assert.That(john.UppercasedName.EmbeddedValue, Is.EqualTo("JOHN DOE"));

            var acme = this.Population.EmbeddedCreateObject<Organization>();
            acme.Name.EmbeddedValue = "Acme";

            this.Population.EmbeddedDerive();

            Assert.That(acme.UppercasedName.EmbeddedValue, Is.EqualTo("ACME"));
        }

        public class UppercaseNameDerivation : IEmbeddedDerivation
        {
            public void EmbeddedDerive(IEmbeddedChangeSet changeSet)
            {
                var names = changeSet.EmbeddedChangedRoles<INamed>("Name");

                if (names.Any())
                {
                    foreach (var named in names.Keys.Cast<INamed>())
                    {
                        // Dummy updates ...
                        named.Name.EmbeddedValue = named.Name.EmbeddedValue;

                        named.UppercasedName.EmbeddedValue = $"{named.Name.EmbeddedValue?.ToUpperInvariant()}";
                    }
                }
            }
        }
    }
}
