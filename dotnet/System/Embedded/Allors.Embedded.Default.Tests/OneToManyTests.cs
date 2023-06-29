﻿namespace Allors.Embedded.Default.Tests
{
    public class OneToManyTests : Embedded.Tests.OneToManyTests
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
