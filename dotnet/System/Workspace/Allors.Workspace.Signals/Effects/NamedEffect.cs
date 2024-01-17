namespace Allors.Workspace.Signals;

using System;
using System.Collections.Concurrent;

public class NamedEffect : IEffect
{
    private readonly ConcurrentDictionary<INotifyChanged, string> nameByChangeNotifier;

    public NamedEffect(Action<string> action, params Action<NamedEffect>[] builders)
    {
        this.nameByChangeNotifier = new ConcurrentDictionary<INotifyChanged, string>();
        this.Action = action;

        foreach (var builder in builders)
        {
            builder(this);
        }
    }

    public Action<string> Action { get; }

    public Action<string, INotifyChanged> ActionWithArgument { get; }

    public void Add((INotifyChanged, string) namedChangeNotifier)
    {
        (INotifyChanged changeNotifier, string name) = namedChangeNotifier;
        this.Add(changeNotifier, name);
    }

    public void Add(INotifyChanged changeNotifier, string? name = null)
    {
        name ??= changeNotifier switch
        {
            IRole role => role.RoleType.Name,
            IAssociation association => association.AssociationType.Name,
            IMethod method => method.MethodType.Name,
            _ => throw new ArgumentNullException(nameof(name)),
        };

        if (this.nameByChangeNotifier.TryAdd(changeNotifier, name))
        {
            changeNotifier.Changed += ChangeNotifierOnChanged;
        }
    }

    public void Dispose()
    {
        foreach (var changeNotifier in this.nameByChangeNotifier.Keys)
        {
            changeNotifier.Changed -= ChangeNotifierOnChanged;
        }
    }

    private void ChangeNotifierOnChanged(object sender, ChangedEventArgs e)
    {
        var changeNotifier = e.Source;

        if (this.nameByChangeNotifier.TryGetValue(changeNotifier, out var name))
        {
            this.Action?.Invoke(name);
            this.ActionWithArgument?.Invoke(name, changeNotifier);
        }
    }
}
