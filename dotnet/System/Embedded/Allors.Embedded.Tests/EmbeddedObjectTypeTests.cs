namespace Allors.Embedded.Tests
{
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
                v.Same.Value = "c1";
            });

            var c2 = newC2(v =>
            {
                v.Same.Value = "c2";
            });

            Assert.That(c1.Same.Value, Is.EqualTo("c1"));
            Assert.That(c2.Same.Value, Is.EqualTo("c2"));
        }
    }
}
