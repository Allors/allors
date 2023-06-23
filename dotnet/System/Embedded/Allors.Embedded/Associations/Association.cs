namespace Allors.Embedded
{
    using Meta;

    public abstract class Association : IAssociation
    {
        protected Association(EmbeddedObject @object, IEmbeddedAssociationType associationType)
        {
            this.Object = @object;
            this.AssociationType = associationType;
        }

        public EmbeddedObject Object { get; }

        public IEmbeddedAssociationType AssociationType { get; }
    }
}
