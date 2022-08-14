namespace Allors.Workspace.Pull
{
    using System.Collections.Generic;

    public interface IPullRevocation
    {
        long Version { get; }

        ISet<string> Tags { get; }
    }
}
