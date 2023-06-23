namespace Allors.Embedded
{
    using Meta;

    public class UnitRole<T> : Role
    {
        public UnitRole(IEmbeddedObject @object, IEmbeddedRoleType roleType) : base(@object, roleType)
        {
        }

        public T Value
        {
            get
            {
                return (T)this.Object.Population.GetRoleValue(this.Object, this.RoleType);
            }
            set
            {
                this.Object.Population.SetRoleValue(this.Object, this.RoleType, value);
            }
        }
    }
}
