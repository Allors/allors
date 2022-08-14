namespace Allors.Workspace.Pull
{
    using System.Collections.Generic;

    public interface IPull
    {
        IDictionary<long, IPullObject> ObjectById { get; }
    }
}
