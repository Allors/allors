namespace Allors.Workspace.Signals.Default;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class Effect : IEffect
{
    private IDictionary<ISignal, object> valueBySignal;

    public Effect(Action<IDependencyTracker> dependencies, Action action)
    {
        this.Dependencies = dependencies;
        this.Action = action;
    }

    public Action<IDependencyTracker> Dependencies { get; }

    public Action Action { get; }

    public void Dispose()
    {
    }

    public void Raise()
    {
        

    }
}
