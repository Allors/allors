namespace Allors.Workspace.Sync.Response
{
    public interface ISyncedCompositeRole : ISyncedRole
    {
        new long Value { get; }
    }
}
