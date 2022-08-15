namespace Allors.Workspace.Sync.Response
{
    public interface ISyncedBinaryRole : ISyncedUnitRole
    {
        new byte[] Value { get; }
    }
}
