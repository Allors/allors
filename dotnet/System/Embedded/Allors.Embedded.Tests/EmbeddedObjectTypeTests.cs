namespace Allors.Embedded.Tests
{
    using Allors.Embedded.Meta;
    using Allors.Embedded.Tests.Domain;

    public class EmbeddedObjectTypeTests : Tests
    {
        [Test]
        public void SameUnitTypeName()
        {
            New<C1> newC1 = this.Population.New;
            New<C2> newC2 = this.Population.New;

            var c1 = newC1(v =>
            {
                v.Same = "c1";
            });

            var c2 = newC2(v =>
            {
                v.Same = "c2";
            });

            Assert.AreEqual("c1", c1.Same);
            Assert.AreEqual("c2", c2.Same);
        }
    }
}
