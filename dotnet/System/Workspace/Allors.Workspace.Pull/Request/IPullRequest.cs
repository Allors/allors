namespace Allors.Workspace.Pull.Request
{
    using System.Collections.Generic;

    public interface IPullRequest
    {
        IEnumerable<IPullRequestObject> Objects { get; }
    }
}
