namespace Allors.Embedded.Tests
{
    using Domain;

    public class UnitRoleTests : Tests
    {
        [Test]
        public void String()
        {
            var c1a = this.Population.New<C1>();
            c1a.String = "a string";

            Assert.AreEqual("a string", c1a.String);
        }
    }
}
