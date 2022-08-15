namespace Allors.Workspace.Sync.Response
{
    using System.Collections.Generic;

    public interface IPullRequestRevocation
    {
        long Version { get; }

        ISet<string> Tags { get; }
    }
}
