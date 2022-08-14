namespace Allors.Workspace.Pull
{
    public interface IPullDecimalRole : IPullUnitRole
    {
        new decimal Value { get; }
    }
}
