namespace Allors.Workspace.Sync.Response
{
    using System.Collections.Generic;

    public interface ISyncedRevocation
    {
        long Version { get; }

        ISet<string> Tags { get; }
    }
}
