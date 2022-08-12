namespace Allors.Workspace.State
{
    using System.Collections.Generic;

    public interface IGrantState
    {
        long Version { get; }

        ISet<string> ReadTags { get; }

        ISet<string> WriteTags { get; }

        ISet<string> ExecuteTags { get; }
    }
}
