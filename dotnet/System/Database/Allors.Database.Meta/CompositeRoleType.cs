namespace Allors.Database.Meta;

public class CompositeRoleType : ICompositeRoleType
{
    public CompositeRoleType(IComposite composite, RoleType roleType)
    {
        this.Extensions = new MetaExtension();
        this.Composite = composite;
        this.RoleType = roleType;
    }

    public dynamic Extensions { get; }

    public IComposite Composite { get; }

    public IRoleType RoleType { get; }
}
