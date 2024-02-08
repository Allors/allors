namespace Allors.Embedded.Domain.Memory.Tests
{
    using System;
    using System.Linq;
    using Domain;

    public class DerivationOverrideTests : Domain.Tests.DerivationOverrideTests
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
