namespace Allors.Workspace.Sync.Response
{
    using System;

    public interface ISyncedDateTimeRole : ISyncedUnitRole
    {
        new DateTime Value { get; }
    }
}
