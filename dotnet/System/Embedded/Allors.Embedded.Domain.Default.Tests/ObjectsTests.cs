namespace Allors.Embedded.Default.Tests
{
    using System;
    using System.Linq;
    using Allors.Embedded.Tests.Domain;

    public class ObjectsTests : Embedded.Tests.ObjectsTests
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
