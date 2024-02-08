namespace Allors.Embedded.Domain.Memory.Tests
{
    using Domain;

    public class EmbeddedUnitRoleTests : Domain.Tests.EmbeddedUnitRoleTests
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
