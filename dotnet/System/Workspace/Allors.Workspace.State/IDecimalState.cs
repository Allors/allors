namespace Allors.Workspace.State
{
    public interface IDecimalState : IUnitRoleState
    {
        new decimal Role { get; }
    }
}
