namespace Allors.Embedded.Domain.Memory
{
    using Embedded.Meta;

    public class EmbeddedCompositeRole<T> : EmbeddedRole, IEmbeddedCompositeRole<T> where T : IEmbeddedObject
    {
        public EmbeddedCompositeRole(IEmbeddedObject @object, EmbeddedRoleType roleType) : base(@object, roleType)
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
