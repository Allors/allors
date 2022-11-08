namespace Allors.Workspace.Request
{
    using System.Collections.Generic;
    using Allors.Workspace.Meta;

    public interface IInvocationRequest
    {

        long ObjectId { get; }

        IMethodType MethodType { get; }

        IRecord Input { get; }

        IReadOnlyList<IPull> Pulls { get; }
    }
}
