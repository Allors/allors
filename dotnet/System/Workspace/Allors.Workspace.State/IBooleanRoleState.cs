namespace Allors.Workspace.State
{
    public interface IBooleanRoleState : IUnitRoleState
    {
        new bool Role { get; }
    }
}
