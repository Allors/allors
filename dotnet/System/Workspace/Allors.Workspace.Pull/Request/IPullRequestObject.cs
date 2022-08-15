namespace Allors.Workspace.Pull.Request
{
    using System;

    public interface IPullRequestObject
    {
        long Id { get; }

        long Version { get; }

        Guid SecurityFingerprint { get; }
    }
}
