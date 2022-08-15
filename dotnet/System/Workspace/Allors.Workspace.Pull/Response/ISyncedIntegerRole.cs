namespace Allors.Workspace.Sync.Response
{
    public interface ISyncedIntegerRole : ISyncedUnitRole
    {
        new int Value { get; }
    }
}
