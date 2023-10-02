namespace Allors.Workspace.Signals.Default;

using System;

public class ValueSignal<T> : IValueSignal<T>, IDownstream
{
    private readonly Dispatcher dispatcher;
    private long workspaceVersion;
    private T value;

    public ValueSignal(Dispatcher dispatcher, T value)
    {
        this.dispatcher = dispatcher;
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

    public void OnChanged()
    {
        this.Upstreams.Invalidate();
    }
}
