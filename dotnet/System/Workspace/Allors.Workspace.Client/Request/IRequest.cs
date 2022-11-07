namespace Allors.Workspace.Request
{
    using System.Collections.Generic;

    public interface IRequest
    {
        IReadOnlyList<IInvocationRequest> InvocationRequests { get; }
    }
}
