namespace Allors.Embedded
{
    using Meta;

    public class UnitRole<T> : Role, IUnitRole<T>
    {
        public UnitRole(IEmbeddedObject @object, EmbeddedRoleType roleType) : base(@object, roleType)
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
