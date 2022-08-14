namespace Allors.Workspace.Pull
{
    public interface IPullBinaryRole : IPullUnitRole
    {
        new byte[] Value { get; }
    }
}
