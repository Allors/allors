namespace Allors.Workspace.Sync.Response
{
    public interface IPullRequestFloatRole : IPullRequestUnitRole
    {
        new double Value { get; }
    }
}
