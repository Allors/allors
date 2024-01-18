namespace Allors.Workspace.Signals;

using System;
using System.Collections.Concurrent;

public class NamedEffect : IEffect
{
    private readonly ConcurrentDictionary<ICacheable, string> nameByCacheable;

    public NamedEffect(Action<string> action, params Action<NamedEffect>[] builders)
    {
        this.nameByCacheable = new ConcurrentDictionary<ICacheable, string>();
        this.Action = action;

        foreach (var builder in builders)
        {
            builder(this);
        }
    }

    public Action<string> Action { get; }

    public Action<string, ICacheable> ActionWithArgument { get; }

    public void Add((ICacheable, string) namedCacheable)
    {
        (ICacheable cacheable, string name) = namedCacheable;
        this.Add(cacheable, name);
    }

    public void Add(ICacheable cacheable, string? name = null)
    {
        name ??= cacheable switch
        {
            IRole role => role.RoleType.Name,
            IAssociation association => association.AssociationType.Name,
            IMethod method => method.MethodType.Name,
            _ => throw new ArgumentNullException(nameof(name)),
        };

        if (this.nameByCacheable.TryAdd(cacheable, name))
        {
            cacheable.InvalidationRequested += this.Cacheable_InvalidationRequested;
        }
    }

    public void Dispose()
    {
        foreach (var cacheable in this.nameByCacheable.Keys)
        {
            cacheable.InvalidationRequested -= this.Cacheable_InvalidationRequested;
        }
    }

    private void Cacheable_InvalidationRequested(object sender, InvalidationRequestedEventArgs e)
    {
        var cacheable = e.Cacheable;

        if (this.nameByCacheable.TryGetValue(cacheable, out var name))
        {
            this.Action?.Invoke(name);
            this.ActionWithArgument?.Invoke(name, cacheable);
        }
    }
}
