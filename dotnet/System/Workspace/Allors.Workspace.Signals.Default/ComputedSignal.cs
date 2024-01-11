namespace Allors.Workspace.Signals.Default;

using System;
using System.Collections.Generic;

public class ComputedSignal<T> : IComputedSignal<T>, IUpstream, IDownstream
{
    private readonly Dispatcher dispatcher;
    private readonly Func<ITracker, T> expression;

    private T value;
    private long version;
    private bool isInvalid;

    private object previousValue;
    private long previousValueVersion;

    private ISet<ISignal> trackedSignals;

    public ComputedSignal(Dispatcher dispatcher, Func<ITracker, T> expression)
    {
        this.dispatcher = dispatcher;
        this.expression = expression;
        this.isInvalid = true;
    }

    object ISignal.Value => this.Value;

    public event ChangedEventHandler Changed;

    public long Version
    {
        get
        {
            if (this.isInvalid)
            {
                this.Validate();
            }

            return this.version;
        }
    }

    public T Value
    {
        get
        {
            if (this.isInvalid)
            {
                this.Validate();
            }

            return this.value;
        }
    }

    public WeakReference<IUpstream>[] Upstreams { get; set; }

    public void Track(ISignal signal)
    {
        if (signal == null)
        {
            return;
        }

        this.trackedSignals.Add(signal);
    }

    public void TrackedBy(IUpstream newUpstream)
    {
        this.Upstreams = this.Upstreams.Update(newUpstream);
    }

    public void Invalidate()
    {
        this.isInvalid = true;
        this.Upstreams.Invalidate();
    }

    private void Validate()
    {
        this.trackedSignals = new HashSet<ISignal>();

        var newValue = this.expression(this);
        var newValueVersion = (newValue as IOperand)?.Version ??
                              (newValue as IObject)?.Strategy.Version ??
                              (newValue as IStrategy)?.Version ??
                              0;

        if (!Equals(newValue, this.value))
        {
            this.value = newValue;
            ++this.version;
        }
        else
        {
            if (newValueVersion != this.previousValueVersion)
            {
                ++this.version;
            }
        }
        
        this.previousValue = newValue;
        this.previousValueVersion = newValueVersion;

        this.dispatcher.UpdateTracked(this, this.trackedSignals);

        this.trackedSignals = null;
        this.isInvalid = false;
    }
}
