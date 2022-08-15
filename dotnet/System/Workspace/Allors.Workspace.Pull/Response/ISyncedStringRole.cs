namespace Allors.Workspace.Sync.Response
{
    public interface ISyncedStringRole : ISyncedUnitRole
    {
        new string Value { get; }
    }
}
