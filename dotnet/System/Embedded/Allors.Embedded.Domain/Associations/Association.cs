namespace Allors.Embedded
{
    using Meta;

    public abstract class Association : IAssociation
    {
        protected Association(IEmbeddedObject @object, EmbeddedAssociationType associationType)
        {
            this.Object = @object;
            this.AssociationType = associationType;
        }

        public IEmbeddedObject Object { get; }

        public EmbeddedAssociationType AssociationType { get; }
    }
}
