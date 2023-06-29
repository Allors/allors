namespace Allors.Embedded.Default.Tests
{
    using Allors.Embedded.Tests.Domain;

    public class UnitRoleTests : Embedded.Tests.UnitRoleTests
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
