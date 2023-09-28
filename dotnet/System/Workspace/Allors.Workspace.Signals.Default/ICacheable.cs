namespace Allors.Workspace.Signals.Default;

public interface ICacheable
{
    void InvalidateCache();

    void Cache();
}
