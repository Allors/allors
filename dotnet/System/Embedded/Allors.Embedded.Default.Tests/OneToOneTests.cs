﻿namespace Allors.Embedded.Default.Tests
{
    public class OneToOneTests : Embedded.Tests.OneToOneTests
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
