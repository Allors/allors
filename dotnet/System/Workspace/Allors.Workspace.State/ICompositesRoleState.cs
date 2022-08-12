namespace Allors.Workspace.State
{
    using System.Collections.Generic;

    public interface ICompositesRoleState : IRoleState
    {
        new ISet<long> Role { get; }
    }
}
