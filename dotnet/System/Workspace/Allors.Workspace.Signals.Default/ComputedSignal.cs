namespace Allors.Workspace.Signals.Default;

using System;

public class ComputedSignal<T> : IComputedSignal<T>, IDependencyTracker, IProducer, IConsumer
{
    private readonly Func<IDependencyTracker, T> expression;
    
    public ComputedSignal(Func<IDependencyTracker, T> expression)
    {
        this.expression = expression;
    }
    
    object ISignal.Value => this.Value;

    public T Value => this.expression(this);
    
    public void Track(IOperand operand)
    {
    }

    public void Track(ISignal signal)
    {
    }
}
