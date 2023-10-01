namespace Allors.Workspace.Signals.Default;

using System;

public class ValueSignal<T> : IValueSignal<T>, IDownstream
{
    private long workspaceVersion;
    private T value;

    public ValueSignal(T value)
    {
        this.Value = value;
    }

    object ISignal.Value => this.Value;

    public T Value
    {
        get => this.value;
        set
        {
            if (!Equals(value, this.value))
            {
                this.value = value;
                ++this.workspaceVersion;
                this.OnChanged();
            }
        }
    }

    object IValueSignal.Value { get; set; }

    public long WorkspaceVersion => this.workspaceVersion;

    public WeakReference<IUpstream>[] Upstreams { get; set; }

    public void TrackedBy(IUpstream newUpstream)
    {
        this.Upstreams = this.Upstreams.Update(newUpstream);
    }

    private void OnChanged()
    {
        var upstreams = this.Upstreams;
        foreach (var weakReference in upstreams)
        {
            weakReference.TryGetTarget(out var upstream);
            upstream?.Invalidate();
        }
    }
}
