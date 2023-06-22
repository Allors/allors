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
                    v.AddUnit<C1, string>(nameof(C1.String));
                });
        }
    }
}
