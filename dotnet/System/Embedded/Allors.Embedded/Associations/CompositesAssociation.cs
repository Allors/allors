namespace Allors.Embedded
{
    using System;
    using System.Linq;
    using Meta;

    public class CompositesAssociation<TAssociation> : Association where TAssociation : IEmbeddedObject
    {
        public CompositesAssociation(EmbeddedObject @object, IEmbeddedAssociationType associationType) : base(@object, associationType)
        {
        }

        public TAssociation[] Value
        {
            get
            {
                return ((EmbeddedObject[])this.Object.Population.GetAssociationValue(this.Object, this.AssociationType))?.Cast<TAssociation>().ToArray() ?? Array.Empty<TAssociation>();
            }
        }
    }
}
