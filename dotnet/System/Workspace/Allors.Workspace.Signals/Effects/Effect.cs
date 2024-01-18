namespace Allors.Workspace.Signals;

using System;
using System.Collections.Generic;

public class Effect : IEffect
{
    private readonly HashSet<ICacheable> cacheables;

    public Effect(Action action, params Action<Effect>[] builders)
    {
        this.cacheables = new HashSet<ICacheable>();
        this.Action = action;

        foreach (var builder in builders)
        {
            builder(this);
        }
    }

    public Effect(Action<ICacheable> action, params Action<Effect>[] builders)
    {
        this.cacheables = new HashSet<ICacheable>();
        this.ActionWithArgument = action;

        foreach (var builder in builders)
        {
            builder(this);
        }
    }

    public Action Action { get; }

    public Action<ICacheable> ActionWithArgument { get; }

    public void Add(ICacheable cacheable)
    {
        if (this.cacheables.Add(cacheable))
        {
            cacheable.InvalidationRequested += this.Cacheable_InvalidationRequested;
        }
    }

    public void Dispose()
    {
        foreach (var cacheable in this.cacheables)
        {
            cacheable.InvalidationRequested -= this.Cacheable_InvalidationRequested;
        }
    }

    private void Cacheable_InvalidationRequested(object sender, InvalidationRequestedEventArgs e)
    {
        this.Action?.Invoke();
        this.ActionWithArgument?.Invoke(e.Cacheable);
    }
}
