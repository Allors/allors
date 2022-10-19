namespace Allors.Workspace.Meta
{
    public class CompositeRoleType : ICompositeRoleType
    {
        public CompositeRoleType(IComposite composite, RoleType roleType)
        {
            this.Attributes = new MetaExtension();
            this.Composite = composite;
            this.RoleType = roleType;
        }

        public dynamic Attributes { get; }

        public IComposite Composite { get; }

        public IRoleType RoleType { get; }
    }
}
