namespace Allors.Embedded
{
    using Meta;

    public abstract class Association : IAssociation
    {
        protected Association(IEmbeddedObject @object, IEmbeddedAssociationType associationType)
        {
            this.Object = @object;
            this.AssociationType = associationType;
        }

        public IEmbeddedObject Object { get; }

        public IEmbeddedAssociationType AssociationType { get; }
    }
}
