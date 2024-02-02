namespace Allors.Workspace.Signals;

using System;
using System.Collections.Generic;

public class Effect : IEffect
{
    private readonly HashSet<INotifyChanged> changeables;

    public Effect(Action action, params Action<Effect>[] builders)
    {
        this.changeables = new HashSet<INotifyChanged>();
        this.Action = action;

        foreach (var builder in builders)
        {
            builder(this);
        }
    }

    public Effect(Action<INotifyChanged> action, params Action<Effect>[] builders)
    {
        this.changeables = new HashSet<INotifyChanged>();
        this.ActionWithArgument = action;

        foreach (var builder in builders)
        {
            builder(this);
        }
    }

    public Action Action { get; }

    public Action<INotifyChanged> ActionWithArgument { get; }

    public void Add(INotifyChanged notifyChanged)
    {
        if (this.changeables.Add(notifyChanged))
        {
            notifyChanged.Changed += this.ChangeableChanged;
        }
    }

    public void Dispose()
    {
        foreach (var changeable in this.changeables)
        {
            changeable.Changed -= this.ChangeableChanged;
        }
    }

    private void ChangeableChanged(object sender, EventArgs e)
    {
        this.Action?.Invoke();
        this.ActionWithArgument?.Invoke((INotifyChanged)sender);
    }
}
