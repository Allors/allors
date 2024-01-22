namespace Allors.Workspace.Signals;

using System;
using System.Collections.Concurrent;

public class NamedEffect : IEffect
{
    private readonly ConcurrentDictionary<IChangeable, string> nameByChangeable;

    public NamedEffect(Action<string> action, Action<NamedEffect> builder) : this(action, [builder])
    {
    }

    public NamedEffect(Action<string> action, params Action<NamedEffect>[] builders)
    {
        this.nameByChangeable = new ConcurrentDictionary<IChangeable, string>();
        this.Action = action;

        foreach (var builder in builders)
        {
            builder(this);
        }
    }

    public Action<string> Action { get; }

    public Action<string, IChangeable> ActionWithArgument { get; }

    public void Add((IChangeable, string) namedChangeable)
    {
        (IChangeable changeable, string name) = namedChangeable;
        this.Add(changeable, name);
    }

    public void Add(IChangeable changeable, string? name = null)
    {
        name ??= changeable switch
        {
            IRole role => role.RoleType.Name,
            IAssociation association => association.AssociationType.Name,
            IMethod method => method.MethodType.Name,
            _ => throw new ArgumentNullException(nameof(name)),
        };

        if (this.nameByChangeable.TryAdd(changeable, name))
        {
            changeable.Changed += this.ChangeableChanged;
        }
    }

    public void Dispose()
    {
        foreach (var changeable in this.nameByChangeable.Keys)
        {
            changeable.Changed -= this.ChangeableChanged;
        }
    }

    private void ChangeableChanged(object sender, ChangedEventArgs e)
    {
        var changeable = e.Changeable;

        if (this.nameByChangeable.TryGetValue(changeable, out var name))
        {
            this.Action?.Invoke(name);
            this.ActionWithArgument?.Invoke(name, changeable);
        }
    }
}
