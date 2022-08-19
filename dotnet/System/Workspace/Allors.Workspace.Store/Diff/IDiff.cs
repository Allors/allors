namespace Allors.Workspace
{
    using Meta;

    public interface IDiff
    {
        RelationType RelationType { get; }

        IStrategy Association { get; }
    }
}
