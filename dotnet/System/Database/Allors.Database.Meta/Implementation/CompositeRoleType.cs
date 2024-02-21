namespace Allors.Database.Meta;

public sealed class CompositeRoleType : ICompositeRoleType
{
    public CompositeRoleType(IComposite composite, IRoleType roleType)
    {
        this.Attributes = new MetaExtension();
        this.Composite = composite;
        this.RoleType = roleType;
    }

    public dynamic Attributes { get; }

    public IComposite Composite { get; }

    public IRoleType RoleType { get; }
}
