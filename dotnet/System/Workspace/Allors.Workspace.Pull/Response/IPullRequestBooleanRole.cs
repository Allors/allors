namespace Allors.Workspace.Sync.Response
{
    public interface IPullRequestBooleanRole : IPullRequestUnitRole
    {
        new bool Value { get; }
    }
}
