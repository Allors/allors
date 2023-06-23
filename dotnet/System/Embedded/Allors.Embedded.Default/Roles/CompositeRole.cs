namespace Allors.Embedded
{
    using Meta;

    public class CompositeRole<TRole> : Role, ICompositeRole<TRole> where TRole : IEmbeddedObject
    {
        public CompositeRole(IEmbeddedObject @object, IEmbeddedRoleType roleType) : base(@object, roleType)
        {
        }

        public TRole Value
        {
            get
            {
                return (TRole)this.Object.Population.GetRoleValue(this.Object, this.RoleType);
            }
            set
            {
                this.Object.Population.SetRoleValue(this.Object, this.RoleType, value);
            }
        }
    }
}
