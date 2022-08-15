namespace Allors.Workspace.Sync.Response
{
    public interface ISyncedFloatRole : ISyncedUnitRole
    {
        new double Value { get; }
    }
}
