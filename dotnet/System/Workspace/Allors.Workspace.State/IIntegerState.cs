namespace Allors.Workspace.State
{
    public interface IIntegerState : IUnitRoleState
    {
        new int Role { get; }
    }
}
