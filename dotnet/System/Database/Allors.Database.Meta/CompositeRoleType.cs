namespace Allors.Database.Meta;

public sealed class CompositeRoleType : IMetaExtensible
{
    public CompositeRoleType(IComposite composite, RoleType roleType)
    {
        this.Attributes = new MetaExtension();
        this.Composite = composite;
        this.RoleType = roleType;
    }

    public dynamic Attributes { get; }

    public IComposite Composite { get; }

    public RoleType RoleType { get; }
}
