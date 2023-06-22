namespace Allors.Embedded.Tests
{
    using Domain;

    public abstract class Tests
    {
        public EmbeddedPopulation Population { get; private set; }

        [SetUp]
        public void Setup()
        {
            this.Population = new EmbeddedPopulation(
                v =>
                {
                    v.AddUnit<Organisation, string>(nameof(Organisation.Name));
                    v.AddUnit<Organisation, Person>(nameof(Organisation.Owner));
                    v.AddUnit<Person, string>(nameof(Person.Name));
                    // Special
                    v.AddUnit<C1, string>(nameof(C1.Same));
                    v.AddUnit<C2, string>(nameof(C2.Same));
                });
        }
    }
}
