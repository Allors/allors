namespace Allors.Embedded.Meta.Tests
{

    public abstract class Tests
    {
        public EmbeddedMeta Meta { get; private set; }

        [SetUp]
        public virtual void SetUp()
        {
            this.Meta = new EmbeddedMeta();
        }
    }
}
