namespace Allors.Workspace.Sync.Response
{
    public interface ISyncedBooleanRole : ISyncedUnitRole
    {
        new bool Value { get; }
    }
}
