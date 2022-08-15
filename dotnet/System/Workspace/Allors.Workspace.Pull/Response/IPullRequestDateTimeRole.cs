namespace Allors.Workspace.Sync.Response
{
    using System;

    public interface IPullRequestDateTimeRole : IPullRequestUnitRole
    {
        new DateTime Value { get; }
    }
}
