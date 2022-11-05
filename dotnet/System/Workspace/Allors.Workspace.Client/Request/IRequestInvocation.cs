namespace Allors.Workspace.Request
{
    using Allors.Workspace.Meta;

    public interface IRequestInvocation
    {
        string Name { get; }

        long ObjectId { get; }

        IMethodType MethodType { get; }

        IRequestRecord Input { get; }
    }
}
