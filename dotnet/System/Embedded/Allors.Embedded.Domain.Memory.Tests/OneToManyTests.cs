namespace Allors.Embedded.Domain.Memory.Tests
{
    public class OneToManyTests : Domain.Tests.OneToManyTests
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
