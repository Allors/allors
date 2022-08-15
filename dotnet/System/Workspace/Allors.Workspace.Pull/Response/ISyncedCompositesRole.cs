namespace Allors.Workspace.Sync.Response
{
    using System.Collections.Generic;

    public interface ISyncedCompositesRole : ISyncedRole
    {
        new ISet<long> Value { get; }
    }
}
