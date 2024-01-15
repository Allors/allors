namespace Allors.Workspace.Signals.Default;

using System;

public interface IDownstream
{
    WeakReference<IUpstream>[] Upstreams { get; set; }

    void TrackedBy(IUpstream newUpstream);
}
