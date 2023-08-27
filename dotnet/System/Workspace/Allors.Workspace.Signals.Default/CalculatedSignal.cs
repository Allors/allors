namespace Allors.Workspace.Signals.Default;

using System;
using System.Collections.Generic;

public class CalculatedSignal<T> : ICalculatedSignal<T>, IDependencyTracker
{
    private readonly Func<IDependencyTracker, T> expression;
    
    public CalculatedSignal(Func<IDependencyTracker, T> expression)
    {
        this.expression = expression;
    }
    
    object ISignal.Value => this.Value;

    public T Value => this.expression(this);
    
    public IEnumerable<ISignal> Signals { get; }

    public void Track(IOperand operand)
    {
    }

    public void Track(ISignal signal)
    {
    }
}
