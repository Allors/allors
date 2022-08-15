namespace Allors.Workspace.State
{
    using System.Collections.Generic;

    public interface IPullResponse
    {
        IEnumerable<IPullObject> Objects { get; }
    }
}
