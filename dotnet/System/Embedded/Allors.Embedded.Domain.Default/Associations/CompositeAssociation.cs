namespace Allors.Embedded
{
    using Meta;

    public class CompositeAssociation<TAssociation> : Association, ICompositeAssociation<TAssociation> where TAssociation : IEmbeddedObject
    {
        public CompositeAssociation(IEmbeddedObject @object, EmbeddedAssociationType associationType) : base(@object, associationType)
        {
        }

        public TAssociation Value
        {
            get
            {
                return (TAssociation)this.Object.Population.GetAssociationValue(this.Object, this.AssociationType);
            }
        }
    }
}
