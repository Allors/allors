namespace Allors.Database.Meta
{
    using Embedded;
    using Embedded.Meta;

    public abstract class EmbeddedObject : IEmbeddedObject
    {
        protected EmbeddedObject(IEmbeddedPopulation embeddedPopulation, EmbeddedObjectType embeddedObjectType)
        {
            this.EmbeddedPopulation = embeddedPopulation;
            this.EmbeddedObjectType = embeddedObjectType;
        }

        public IEmbeddedPopulation EmbeddedPopulation { get; }

        public EmbeddedObjectType EmbeddedObjectType { get; }
    }
}
