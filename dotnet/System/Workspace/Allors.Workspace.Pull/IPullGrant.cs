namespace Allors.Workspace.Pull
{
    using System.Collections.Generic;

    public interface IPullGrant
    {
        long Version { get; }

        ISet<string> ReadTags { get; }

        ISet<string> WriteTags { get; }

        ISet<string> ExecuteTags { get; }
    }
}
