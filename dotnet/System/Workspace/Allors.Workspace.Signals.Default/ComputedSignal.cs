namespace Allors.Workspace.Signals.Default;

using System;
using System.Collections.Generic;

public class ComputedSignal<T> : IComputedSignal<T>, IUpstream, IDownstream
{
    private readonly Dispatcher dispatcher;
    private readonly Func<ITracker, T> expression;

    private T value;
    private long workspaceVersion;
    private bool isInvalid;

    private ISet<IOperand> trackedOperands;

    public ComputedSignal(Dispatcher dispatcher, Func<ITracker, T> expression)
    {
        this.dispatcher = dispatcher;
        this.expression = expression;
        this.isInvalid = true;
    }

    object ISignal.Value => this.Value;

    public long WorkspaceVersion
    {
        get
        {
            if (this.isInvalid)
            {
                this.Validate();
            }

            return this.workspaceVersion;
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

    public void Track(IOperand operand)
    {
        this.trackedOperands.Add(operand);
    }

    public void TrackedBy(IUpstream newUpstream)
    {
        this.Upstreams = this.Upstreams.Update(newUpstream);
    }

    public void Invalidate()
    {
        this.isInvalid = true;

        var upstreams = this.Upstreams;
        foreach (var weakReference in upstreams)
        {
            weakReference.TryGetTarget(out var upstream);
            upstream?.Invalidate();
        }
    }

    private void Validate()
    {
        this.trackedOperands = new HashSet<IOperand>();

        var newValue = this.expression(this);

        if (!Equals(newValue, this.value))
        {
            this.value = newValue;
            ++this.workspaceVersion;
        }

        this.dispatcher.UpdateTracked(this, this.trackedOperands);

        this.trackedOperands = null;
        this.isInvalid = false;
    }
}
