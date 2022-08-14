namespace Allors.Workspace.Pull
{
    using System;

    public interface IPullUniqueRole : IPullUnitRole
    {
        new Guid Value { get; }
    }
}
