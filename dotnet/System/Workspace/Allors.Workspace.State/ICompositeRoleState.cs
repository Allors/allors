namespace Allors.Workspace.State
{
    public interface ICompositeRoleState : IRoleState
    {
        new long Role { get; }
    }
}
