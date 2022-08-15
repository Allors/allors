namespace Allors.Workspace.Sync.Response
{
    public interface IPullRequestCompositeRole : IPullRequestRole
    {
        new long Value { get; }
    }
}
