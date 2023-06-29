namespace Allors.Embedded.Meta.Tests
{
    public class EmbeddedObjectTypeTests : Tests
    {
        [Test]
        public void SameName()
        {
            this.Meta.AddUnit<Person, string>("Same");

            Assert.Throws<ArgumentException>(() => this.Meta.AddUnit<Person, string>("Same"));
        }
    }
}
