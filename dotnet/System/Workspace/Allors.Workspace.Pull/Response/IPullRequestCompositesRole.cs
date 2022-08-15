namespace Allors.Workspace.Sync.Response
{
    using System.Collections.Generic;

    public interface IPullRequestCompositesRole : IPullRequestRole
    {
        new ISet<long> Value { get; }
    }
}
