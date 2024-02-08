namespace Allors.Embedded.Domain.Memory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Embedded.Meta;

    public class EmbeddedCompositesAssociation<T> : EmbeddedAssociation, IEmbeddedCompositesAssociation<T> where T : IEmbeddedObject
    {
        public EmbeddedCompositesAssociation(IEmbeddedObject @object, EmbeddedAssociationType associationType) : base(@object, associationType)
        {
        }

        public IReadOnlyCollection<T> EmbeddedValue
        {
            get
            {
                return ((IEmbeddedObject[])this.EmbeddedObject.EmbeddedPopulation.EmbeddedGetAssociationValue(this.EmbeddedObject, this.EmbeddedAssociationType))?.Cast<T>().ToArray() ?? Array.Empty<T>();
            }
        }
    }
}
