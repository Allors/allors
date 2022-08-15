namespace Allors.Workspace.Sync.Response
{
    using System.Collections.Generic;

    public interface ISynced
    {
        IDictionary<long, ISyncedObject> ObjectById { get; }
    }
}
