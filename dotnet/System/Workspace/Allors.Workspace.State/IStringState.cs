namespace Allors.Workspace.State
{
    public interface IStringState : IUnitRoleState
    {
        new string Role { get; }
    }
}
