namespace Allors.Workspace.Signals.Default;

using System;

public class ComputedSignal<T> : IComputedSignal<T>, ITracker, ICacheable
{
    private readonly Func<ITracker, T> expression;

    private T value;
    private bool isCached;
    private long workspaceVersion;

    private long previousOperandWorkspaceVersion;

    public ComputedSignal(Func<ITracker, T> expression)
    {
        this.expression = expression;
        this.isCached = false;
    }

    object ISignal.Value => this.Value;

    public long WorkspaceVersion => this.workspaceVersion;

    public T Value
    {
        get
        {
            this.Cache();
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

    public void Cache()
    {
        if (!this.isCached)
        {
            var newValue = this.expression(this);

            if (newValue is IOperand operand)
            {
                if (!Equals(newValue, this.value))
                {
                    ++this.workspaceVersion;
                    this.value = newValue;
                }
                else if (operand.WorkspaceVersion != this.previousOperandWorkspaceVersion)
                {
                    ++this.workspaceVersion;
                }
            }
            else
            {
                if (!Equals(newValue, this.value))
                {
                    this.value = newValue;
                    ++this.workspaceVersion;
                }
            }

            this.isCached = true;
        }
    }
}
