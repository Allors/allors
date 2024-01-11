namespace Allors.Workspace.Signals.Default;

using System;

public class ValueSignal<T> : IValueSignal<T>, IDownstream
{
    private readonly Dispatcher dispatcher;
    private long version;
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
                ++this.version;
                this.OnChanged();
            }
        }
    }

    object IValueSignal.Value { get; set; }


    public event ChangedEventHandler Changed;
    
    public long Version => this.version;

    public WeakReference<IUpstream>[] Upstreams { get; set; }

    public void TrackedBy(IUpstream newUpstream)
    {
        this.Upstreams = this.Upstreams.Update(newUpstream);
    }

    public void OnChanged()
    {
        this.Upstreams.Invalidate();

        this.dispatcher.HandleEffects();
    }
}
