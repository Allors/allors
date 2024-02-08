namespace Allors.Embedded.Domain.Memory.Tests
{
    using System.Linq;
    using Domain.Memory;

    public class DerivationInterfaceTests : Domain.Tests.DerivationInterfaceTests
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
