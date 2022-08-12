namespace Allors.Workspace.State
{
    public interface IFloatState : IUnitRoleState
    {
        new double Role { get; }
    }
}
