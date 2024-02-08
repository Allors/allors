namespace Allors.Embedded.Domain.Memory.Tests
{
    using Domain.Memory;

    public class ManyToManyTests : Domain.Tests.ManyToManyTests
    {
        private EmbeddedPopulation population = null!;

        public override EmbeddedPopulation Population => population;

        [SetUp]
        public override void SetUp()
        {
            this.population = new EmbeddedPopulation();

            base.SetUp();
        }
    }
}
