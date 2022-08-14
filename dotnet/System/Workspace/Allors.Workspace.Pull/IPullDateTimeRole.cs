namespace Allors.Workspace.Pull
{
    using System;

    public interface IPullDateTimeRole : IPullUnitRole
    {
        new DateTime Value { get; }
    }
}
