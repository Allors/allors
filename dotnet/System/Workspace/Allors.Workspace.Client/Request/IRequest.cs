namespace Allors.Workspace.Request
{
    using System.Collections.Generic;

    public interface IRequest
    {
        ISet<VersionedObjectId> RequiredVersions { get; }

        IReadOnlyList<IRequestInvocation> Invocations { get; }
    }
}
