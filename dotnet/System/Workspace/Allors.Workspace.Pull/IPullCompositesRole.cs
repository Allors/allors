namespace Allors.Workspace.Pull
{
    using System.Collections.Generic;

    public interface IPullCompositesRole : IPullRole
    {
        new ISet<long> Value { get; }
    }
}
