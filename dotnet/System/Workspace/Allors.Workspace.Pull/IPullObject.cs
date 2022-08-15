namespace Allors.Workspace.State
{
    using Allors.Workspace.Sync.Response;
    using System.Collections.Generic;

    public interface IPullObject
    {
        string Tag { get; }

        long Id { get; }

        long Version { get; }

        IDictionary<long, ISyncedGrant> GrantById { get; }

        IDictionary<long, ISyncedRevocation> RevocationById { get; }

        IDictionary<string, ISyncedRole> RoleByTag { get; }
    }
}
