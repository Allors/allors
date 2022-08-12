namespace Allors.Workspace.State
{
    using System;

    public interface IDateTimeState : IUnitRoleState
    {
        new DateTime Role { get; }
    }
}
