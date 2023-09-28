namespace Allors.Workspace.Signals.Default;

using System;

public interface IUpstream : ITracker
{
    IDownstream Downstreams { get; set; }
}
