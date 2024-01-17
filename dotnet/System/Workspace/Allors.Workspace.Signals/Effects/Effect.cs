namespace Allors.Workspace.Signals;

using System;
using System.Collections.Generic;
using System.Linq;

public class Effect : IEffect
{
    private readonly HashSet<INotifyChanged> changeNotifiers;

    public Effect(Action action, params Action<Effect>[] builders)
    {
        this.changeNotifiers = new HashSet<INotifyChanged>();
        this.Action = action;

        foreach (var builder in builders)
        {
            builder(this);
        }
    }

    public Effect(Action<INotifyChanged> action, params Action<Effect>[] builders)
    {
        this.changeNotifiers = new HashSet<INotifyChanged>();
        this.ActionWithArgument = action;

        foreach (var builder in builders)
        {
            builder(this);
        }
    }

    public Action Action { get; }

    public Action<INotifyChanged> ActionWithArgument { get; }

    public void Add(INotifyChanged changeNotifier)
    {
        if (this.changeNotifiers.Add(changeNotifier))
        {
            changeNotifier.Changed += ChangeNotifierOnChanged;
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
