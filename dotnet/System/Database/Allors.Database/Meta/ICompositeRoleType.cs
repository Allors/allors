namespace Allors.Database.Meta;

public interface ICompositeRoleType : IMetaExtensible
{
    public IComposite Composite { get; }

    public IRoleType RoleType { get; }
}
