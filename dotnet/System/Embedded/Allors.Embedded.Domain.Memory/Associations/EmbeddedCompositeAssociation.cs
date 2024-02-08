namespace Allors.Embedded.Domain.Memory
{
    using Embedded.Meta;

    public class EmbeddedCompositeAssociation<T> : EmbeddedAssociation, IEmbeddedCompositeAssociation<T> where T : IEmbeddedObject
    {
        public EmbeddedCompositeAssociation(IEmbeddedObject @object, EmbeddedAssociationType associationType) : base(@object, associationType)
        {
        }

        public T EmbeddedValue
        {
            get
            {
                return (T)this.EmbeddedObject.EmbeddedPopulation.EmbeddedGetAssociationValue(this.EmbeddedObject, this.EmbeddedAssociationType);
            }
        }
    }
}
