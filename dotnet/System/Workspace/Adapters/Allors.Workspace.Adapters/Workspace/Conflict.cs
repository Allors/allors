namespace Allors.Workspace.Adapters;

using Meta;

public class Conflict : IConflict
{
    public Conflict(IStrategy association, IRoleType roleType, object role)
    {
        this.Association = association.Object;
        this.RoleType = roleType;
        this.Role = role;
    }

    public IObject Association { get; }

    public IRoleType RoleType { get; }

    public object Role { get; }
}
