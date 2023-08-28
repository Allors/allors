namespace Allors.Workspace.Signals.Default;

using System;

public class Effect : IEffect, IConsumer
{
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
}
