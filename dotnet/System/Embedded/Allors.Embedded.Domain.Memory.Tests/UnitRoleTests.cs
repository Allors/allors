namespace Allors.Embedded.Domain.Memory.Tests
{
    using Domain;

    public class UnitRoleTests : Domain.Tests.UnitRoleTests
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
