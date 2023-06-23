namespace Allors.Embedded
{
    using Meta;

    public abstract class Role : IRole
    {
        protected Role(IEmbeddedObject @object, IEmbeddedRoleType roleType)
        {
            this.Object = @object;
            this.RoleType = roleType;
        }

        public IEmbeddedObject Object { get; }

        public IEmbeddedRoleType RoleType { get; }
    }
}
