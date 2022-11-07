namespace Allors.Workspace.Response
{
    using Allors.Workspace.Request;

    public interface IInvocationResponse
    {
        IInvocationRequest InvocationRequest { get; }

        IRecord Output { get; }
    }
}
