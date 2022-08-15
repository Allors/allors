namespace Allors.Workspace.Sync.Response
{
    public interface IPullRequestIntegerRole : IPullRequestUnitRole
    {
        new int Value { get; }
    }
}
