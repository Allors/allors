namespace Allors.Workspace.State
{
    using System;

    public interface IUniqueState : IUnitRoleState
    {
        new Guid Role { get; }
    }
}
