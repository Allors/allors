namespace Allors.Embedded.Domain.Memory
{
    using System;
    using System.Linq;
    using Embedded.Meta;

    public class CompositesAssociation<TAssociation> : Association, ICompositesAssociation<TAssociation> where TAssociation : IEmbeddedObject
    {
        public CompositesAssociation(IEmbeddedObject @object, EmbeddedAssociationType associationType) : base(@object, associationType)
        {
        }

        public TAssociation[] Value
        {
            get
            {
                return ((IEmbeddedObject[])this.Object.Population.GetAssociationValue(this.Object, this.AssociationType))?.Cast<TAssociation>().ToArray() ?? Array.Empty<TAssociation>();
            }
        }
    }
}
