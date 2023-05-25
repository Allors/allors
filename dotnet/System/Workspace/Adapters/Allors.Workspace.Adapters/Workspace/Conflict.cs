namespace Allors.Workspace.Adapters;

using Meta;

public class Conflict : IConflict
{
    public Conflict(IStrategy association, IRoleType roleType, object role)
    {
        this.Association = association;
        this.RoleType = roleType;
        this.Role = role;
    }

    public IStrategy Association { get; }

    public IRoleType RoleType { get; }

    public object Role { get; }
}
