namespace Allors.Embedded.Domain.Memory
{
    using Embedded.Meta;

    public class EmbeddedUnitRole<T> : EmbeddedRole, IEmbeddedUnitRole<T>
    {
        public EmbeddedUnitRole(IEmbeddedObject @object, EmbeddedRoleType roleType) : base(@object, roleType)
        {
        }

        public T Value
        {
            get
            {
                return (T)this.EmbeddedObject.EmbeddedPopulation.EmbeddedGetRoleValue(this.EmbeddedObject, this.EmbeddedRoleType);
            }
            set
            {
                this.EmbeddedObject.EmbeddedPopulation.EmbeddedSetRoleValue(this.EmbeddedObject, this.EmbeddedRoleType, value);
            }
        }
    }
}
