namespace Allors.Workspace.Signals;

using System;
using System.Collections.Generic;

public class Effect : IEffect
{
    private readonly HashSet<IChangeable> changeables;

    public Effect(Action action, params Action<Effect>[] builders)
    {
        this.changeables = new HashSet<IChangeable>();
        this.Action = action;

        foreach (var builder in builders)
        {
            builder(this);
        }
    }

    public Effect(Action<IChangeable> action, params Action<Effect>[] builders)
    {
        this.changeables = new HashSet<IChangeable>();
        this.ActionWithArgument = action;

        foreach (var builder in builders)
        {
            builder(this);
        }
    }

    public Action Action { get; }

    public Action<IChangeable> ActionWithArgument { get; }

    public void Add(IChangeable changeable)
    {
        if (this.changeables.Add(changeable))
        {
            changeable.Changed += this.ChangeableChanged;
        }
    }

    public void Dispose()
    {
        foreach (var changeable in this.changeables)
        {
            changeable.Changed -= this.ChangeableChanged;
        }
    }

    private void ChangeableChanged(object sender, ChangedEventArgs e)
    {
        this.Action?.Invoke();
        this.ActionWithArgument?.Invoke(e.Changeable);
    }
}
