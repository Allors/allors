namespace Allors.Database.Meta;

public class ConcreteRoleType : IConcreteRoleType
{
    public ConcreteRoleType(IClass @class, RoleType roleType)
    {
        this.Extensions = new MetaExtension();
        this.Class = @class;
        this.RoleType = roleType;
    }

    public dynamic Extensions { get; }

    public IClass Class { get; internal set; }

    public IRoleType RoleType { get; }

    public bool IsRequired { get; set; }

    public bool IsUnique { get; set; }
}
