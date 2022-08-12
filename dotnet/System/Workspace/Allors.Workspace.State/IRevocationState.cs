namespace Allors.Workspace.State
{
    using System.Collections.Generic;

    public interface IRevocationState
    {
        long Version { get; }

        ISet<string> Tags { get; }
    }
}
