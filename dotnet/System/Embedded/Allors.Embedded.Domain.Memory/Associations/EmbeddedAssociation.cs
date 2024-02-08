namespace Allors.Embedded.Domain.Memory
{
    using Embedded.Meta;

    public abstract class EmbeddedAssociation : IEmbeddedAssociation
    {
        protected EmbeddedAssociation(IEmbeddedObject @object, EmbeddedAssociationType associationType)
        {
            this.EmbeddedObject = @object;
            this.EmbeddedAssociationType = associationType;
        }

        public IEmbeddedObject EmbeddedObject { get; }

        public EmbeddedAssociationType EmbeddedAssociationType { get; }
    }
}
