namespace Allors.Workspace.Response
{
    using Allors.Workspace.Request;

    public interface IResponseInvocation
    {
        string Name { get; }

        IResponseRecord Output { get; }
    }
}
