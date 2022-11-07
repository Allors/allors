namespace Allors.Workspace.Request
{
    using System.Collections.Generic;

    public interface IRequest
    {
        IConnection Connection { get; }

        IReadOnlyList<IRequestInvocation> Invocations { get; }
    }
}
