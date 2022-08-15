namespace Allors.Workspace.Sync.Response
{
    using System;

    public interface ISyncedUniqueRole : ISyncedUnitRole
    {
        new Guid Value { get; }
    }
}
