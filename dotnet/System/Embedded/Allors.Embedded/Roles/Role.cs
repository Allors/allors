namespace Allors.Embedded
{
    using Meta;

    public abstract class Role : IRole
    {
        protected Role(EmbeddedObject @object, IEmbeddedRoleType roleType)
        {
            this.Object = @object;
            this.RoleType = roleType;
        }

        public EmbeddedObject Object { get; }

        public IEmbeddedRoleType RoleType { get; }
    }
}
