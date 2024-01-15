namespace Allors.Workspace.Signals.Default;

public interface IUpstream : ITracker
{
    void Invalidate();
}
