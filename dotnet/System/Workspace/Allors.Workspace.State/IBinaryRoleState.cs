namespace Allors.Workspace.State
{
    public interface IBinaryRoleState : IUnitRoleState
    {
        new byte[] Role { get; }
    }
}
