namespace Allors.Workspace.Sync.Response
{
    using System;

    public interface IPullRequestUniqueRole : IPullRequestUnitRole
    {
        new Guid Value { get; }
    }
}
