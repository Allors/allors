namespace Allors.Database.Meta;

public interface IConcreteRoleType : IMetaExtensible
{
    public IClass Class { get; }

    public IRoleType RoleType { get; }
}
