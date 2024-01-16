namespace Allors.Workspace.Signals;

using System;
using System.Collections.Generic;
using System.Linq;

public class Effect : IEffect
{
    private readonly HashSet<INotifyChanged> changeNotifiers;

    public Effect(Action action, INotifyChanged changeNotifier) : this(action, [changeNotifier])
    {
    }

    public Effect(Action action, params INotifyChanged[] changeNotifiers)
    {
        this.changeNotifiers = new HashSet<INotifyChanged>();
        this.Action = action;

        foreach (var changeNotifier in changeNotifiers.Where(v => v != null))
        {
            this.Add(changeNotifier);
        }
    }

    public Effect(Action<IChangedEventSource> action, INotifyChanged changeNotifier) : this(action, [changeNotifier])
    {
    }

    public Effect(Action<IChangedEventSource> action, params INotifyChanged[] changeNotifiers)
    {
        this.changeNotifiers = new HashSet<INotifyChanged>();
        this.ActionWithArgument = action;

        foreach (var changeNotifier in changeNotifiers)
        {
            this.Add(changeNotifier);
        }
    }

    public IEnumerable<INotifyChanged> ChangeNotifiers => this.changeNotifiers.ToArray();

    public Action Action { get; }

    public Action<IChangedEventSource> ActionWithArgument { get; }

    public void Add(INotifyChanged changeNotifier)
    {
        if (this.changeNotifiers.Add(changeNotifier))
        {
            changeNotifier.Changed += ChangeNotifierOnChanged;
        }
    }

    public void Remove(INotifyChanged changeNotifier)
    {
        if (this.changeNotifiers.Remove(changeNotifier))
        {
            changeNotifier.Changed -= ChangeNotifierOnChanged;
        }
    }

    public void Dispose()
    {
        foreach (var changeNotifier in this.changeNotifiers)
        {
            changeNotifier.Changed -= ChangeNotifierOnChanged;
        }
    }

    private void ChangeNotifierOnChanged(object sender, ChangedEventArgs e)
    {
        this.Action?.Invoke();
        this.ActionWithArgument?.Invoke(e.Source);
    }
}
