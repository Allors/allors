namespace Allors.Workspace.Sync.Response
{
    public interface IPullRequestDecimalRole : IPullRequestUnitRole
    {
        new decimal Value { get; }
    }
}
