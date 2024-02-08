namespace Allors.Embedded.Domain.Memory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Embedded.Meta;

    public class EmbeddedCompositesRole<T> : EmbeddedRole, IEmbeddedCompositesRole<T> where T : IEmbeddedObject
    {
        public EmbeddedCompositesRole(IEmbeddedObject @object, EmbeddedRoleType roleType) : base(@object, roleType)
        {
        }

        public IReadOnlyCollection<T> EmbeddedValue
        {
            get
            {
                return ((IEmbeddedObject[])this.EmbeddedObject.EmbeddedPopulation.EmbeddedGetRoleValue(this.EmbeddedObject, this.EmbeddedRoleType))?.Cast<T>().ToArray() ?? Array.Empty<T>();
            }
            set
            {
                this.EmbeddedObject.EmbeddedPopulation.EmbeddedSetRoleValue(this.EmbeddedObject, this.EmbeddedRoleType, value);
            }
        }

        public void EmbeddedAdd(T value)
        {
            this.EmbeddedObject.EmbeddedPopulation.EmbeddedAddRoleValue(this.EmbeddedObject, this.EmbeddedRoleType, value);
        }

        public void EmbeddedRemove(T value)
        {
            this.EmbeddedObject.EmbeddedPopulation.EmbeddedRemoveRoleValue(this.EmbeddedObject, this.EmbeddedRoleType, value);
        }
    }
}
