namespace Allors.Workspace.Signals.Default;

using System;

public class ComputedSignal<T> : IComputedSignal<T>, IDependencyTracker, ICacheable
{
    private readonly Func<IDependencyTracker, T> expression;

    private T value;
    private bool isCached;
    private long workspaceVersion;

    public ComputedSignal(Func<IDependencyTracker, T> expression)
    {
        this.expression = expression;
        this.isCached = false;
    }

    object ISignal.Value => this.Value;

    public T Value
    {
        get
        {
            if (!this.isCached)
            {
                var newValue = this.expression(this);

                if (!Equals(newValue, this.value))
                {
                    this.value = newValue;
                    ++this.workspaceVersion;
                }

                this.isCached = true;
            }

            return this.value;
        }
    }

    public void Track(IOperand operand)
    {
    }

    public void InvalidateCache()
    {
        this.isCached = false;
    }

    public long WorkspaceVersion => this.workspaceVersion;
}
