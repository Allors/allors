namespace Allors.Embedded
{
    using Meta;

    public class CompositeAssociation<TAssociation> : Association where TAssociation : IEmbeddedObject
    {
        public CompositeAssociation(EmbeddedObject @object, IEmbeddedAssociationType associationType) : base(@object, associationType)
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
