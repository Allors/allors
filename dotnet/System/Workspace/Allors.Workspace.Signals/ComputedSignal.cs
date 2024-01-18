namespace Allors.Workspace.Signals;

using System;
using System.Collections.Generic;
using System.Linq;

public sealed class ComputedSignal<T> : ISignal<T>, ITracker
{
    private static readonly NoopTracker ColdTracker = new();

    private readonly Func<ITracker, T> expression;

    private event ChangedEventHandler CustomChanged;

    private ComputedSignalState state;

    private HashSet<INotifyChanged> changeNotifiers;
    private INotifyChanged? valueChangeNotifier;
    private T? cache;
    private bool isCacheInvalid;

    public ComputedSignal(Func<ITracker, T> expression)
    {
        this.expression = expression;
        this.changeNotifiers = new();
        this.state = ComputedSignalState.Cold;
        this.isCacheInvalid = false;
    }

    object ISignal.Value => this.Value;

    public event ChangedEventHandler Changed
    {
        add
        {
            this.CustomChanged += value;
            if (this.state == ComputedSignalState.Cold)
            {
                this.Cache();
            }
        }
        remove
        {
            this.CustomChanged -= value;
            if (this.CustomChanged == null)
            {
                this.state = ComputedSignalState.Cold;

                foreach (var signal in this.changeNotifiers)
                {
                    signal.Changed -= this.ChangeNotifierTrackedChanged;
                }

                if (this.valueChangeNotifier != null)
                {
                    this.valueChangeNotifier.Changed -= this.ValueChangeNotifierTrackedChanged;
                }
            }
        }
    }

    public T? Value
    {
        get
        {
            if (this.state == ComputedSignalState.Cold)
            {
                return this.expression(ColdTracker);
            }

            if (this.state == ComputedSignalState.Hot)
            {
                this.Cache();
            }

            // HotAndCached
            return this.cache;
        }
    }

    void ITracker.Track(INotifyChanged? signal)
    {
        if (signal == null)
        {
            return;
        }

        this.changeNotifiers.Add(signal);
    }

    private void ChangeNotifierTrackedChanged(object sender, ChangedEventArgs e)
    {
        this.OnTrackedChanged();
    }

    private void ValueChangeNotifierTrackedChanged(object sender, ChangedEventArgs e)
    {
        this.isCacheInvalid = true;
        this.OnTrackedChanged();
    }

    private void OnTrackedChanged()
    {
        this.state = ComputedSignalState.Hot;

        var handlers = this.CustomChanged;
        handlers?.Invoke(this, new ChangedEventArgs(this));
    }

    private void Cache()
    {
        var oldChangeNotifiers = this.changeNotifiers;
        var oldValueChangeNotifier = this.valueChangeNotifier;

        this.changeNotifiers = new HashSet<INotifyChanged>();
        this.valueChangeNotifier = null;

        var newValue = this.expression(this);

        if (!Equals(newValue, this.cache) || this.isCacheInvalid)
        {
            this.cache = newValue;
        }

        this.valueChangeNotifier = newValue as INotifyChanged;
        if (!Equals(oldValueChangeNotifier, this.valueChangeNotifier) && (oldValueChangeNotifier != null || this.valueChangeNotifier != null))
        {
            if (oldValueChangeNotifier != null)
            {
                oldValueChangeNotifier.Changed -= this.ValueChangeNotifierTrackedChanged;
            }

            if (this.valueChangeNotifier != null)
            {
                this.valueChangeNotifier.Changed += this.ValueChangeNotifierTrackedChanged;
            }
        }

        var changeNotifiersToRemove = oldChangeNotifiers.Except(this.changeNotifiers);
        foreach (var changeNotifier in changeNotifiersToRemove)
        {
            if (changeNotifier != this.valueChangeNotifier)
            {
                changeNotifier.Changed -= this.ValueChangeNotifierTrackedChanged;
            }
        }

        var changeNotifiersToAdd = this.changeNotifiers.Except(oldChangeNotifiers);
        foreach (var changeNotifier in changeNotifiersToAdd)
        {
            if (changeNotifier != this.valueChangeNotifier)
            {
                changeNotifier.Changed += this.ValueChangeNotifierTrackedChanged;
            }
        }

        this.state = ComputedSignalState.HotAndCached;
        this.isCacheInvalid = false;
    }

    private class NoopTracker : ITracker
    {
        public void Track(INotifyChanged? signal)
        {
        }
    }

    private enum ComputedSignalState
    {
        Cold,
        Hot,
        HotAndCached
    }
}
