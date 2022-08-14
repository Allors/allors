namespace Allors.Workspace.Pull
{
    public interface IPullCompositeRole : IPullRole
    {
        new long Value { get; }
    }
}
