namespace Allors.Workspace.Sync.Response
{
    public interface ISyncedDecimalRole : ISyncedUnitRole
    {
        new decimal Value { get; }
    }
}
