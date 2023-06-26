namespace Allors.Embedded
{
    using Meta;

    public abstract class Role : IRole
    {
        protected Role(IEmbeddedObject @object, EmbeddedRoleType roleType)
        {
            this.Object = @object;
            this.RoleType = roleType;
        }

        public IEmbeddedObject Object { get; }

        public EmbeddedRoleType RoleType { get; }
    }
}
