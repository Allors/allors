namespace Allors.Workspace.State
{
    using System;
    using Allors.Workspace.Sync.Response;
    using System.Collections.Generic;

    public interface IPullRequestObject
    {
        string Tag { get; }

        long Id { get; }

        long Version { get; }

        Guid SecurityFingerprint { get; }

        IEnumerable<IPullRequestGrant> Grants { get; }

        IEnumerable<IPullRequestRevocation> Revocations { get; }

        IEnumerable<IPullRequestRole> Roles { get; }
    }
}
