namespace Allors.Workspace.Sync.Response
{
    public interface IPullRequestBinaryRole : IPullRequestUnitRole
    {
        new byte[] Value { get; }
    }
}
