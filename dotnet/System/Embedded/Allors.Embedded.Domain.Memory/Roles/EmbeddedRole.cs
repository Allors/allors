namespace Allors.Embedded.Domain.Memory
{
    using Embedded.Meta;

    public abstract class EmbeddedRole : IEmbeddedRole
    {
        protected EmbeddedRole(IEmbeddedObject @object, EmbeddedRoleType roleType)
        {
            this.EmbeddedObject = @object;
            this.EmbeddedRoleType = roleType;
        }

        public IEmbeddedObject EmbeddedObject { get; }

        public EmbeddedRoleType EmbeddedRoleType { get; }
    }
}
