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
            john.Name.Value = "John Doe";

            this.Population.EmbeddedDerive();

            Assert.That(john.UppercasedName.Value, Is.EqualTo("JOHN DOE"));

            var acme = this.Population.EmbeddedCreateObject<Organization>();
            acme.Name.Value = "Acme";

            this.Population.EmbeddedDerive();

            Assert.That(acme.UppercasedName.Value, Is.EqualTo("ACME"));
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
                        named.Name.Value = named.Name.Value;

                        named.UppercasedName.Value = $"{named.Name.Value?.ToUpperInvariant()}";
                    }
                }
            }
        }
    }
}
