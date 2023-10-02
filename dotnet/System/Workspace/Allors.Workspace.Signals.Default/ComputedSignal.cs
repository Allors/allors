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

    private object previousValue;
    private long previousValueVersion;

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
        if (operand == null)
        {
            return;
        }

        this.trackedOperands.Add(operand);
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
        this.trackedOperands = new HashSet<IOperand>();

        var newValue = this.expression(this);
        var newValueVersion = (newValue as IOperand)?.WorkspaceVersion ??
                              (newValue as IObject)?.Strategy.Version ??
                              (newValue as IStrategy)?.Version ??
                              0;

        if (!Equals(newValue, this.value))
        {
            this.value = newValue;
            ++this.workspaceVersion;
        }
        else
        {
            if (newValueVersion != this.previousValueVersion)
            {
                ++this.workspaceVersion;
            }
        }
        
        this.previousValue = newValue;
        this.previousValueVersion = newValueVersion;

        this.dispatcher.UpdateTracked(this, this.trackedOperands);

        this.trackedOperands = null;
        this.isInvalid = false;
    }
}
