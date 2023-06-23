namespace Allors.Embedded
{
    using System;
    using System.Linq;
    using Meta;

    public class CompositesAssociation<TAssociation> : Association, ICompositesAssociation<TAssociation> where TAssociation : IEmbeddedObject
    {
        public CompositesAssociation(IEmbeddedObject @object, IEmbeddedAssociationType associationType) : base(@object, associationType)
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
