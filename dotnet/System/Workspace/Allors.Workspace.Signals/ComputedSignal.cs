namespace Allors.Workspace.Signals;

using System;
using System.Collections.Generic;
using System.Linq;

public sealed class ComputedSignal<T> : ISignal<T>, ITracker
{
    private readonly Func<ITracker, T> expression;

    private T value;
    private long version;
    private bool isValid;

    private HashSet<INotifyChanged> changeNotifiers;
    private INotifyChanged valueChangeNotifier;
    private bool isValueChanged;

    public ComputedSignal(Func<ITracker, T> expression)
    {
        this.expression = expression;
        this.changeNotifiers = new HashSet<INotifyChanged>();
        this.isValid = false;
        this.isValueChanged = false;
    }

    object ISignal.Value => this.Value;

    public event ChangedEventHandler Changed;

    public long Version
    {
        get
        {
            if (!this.isValid)
            {
                this.Validate();
            }

            return this.version;
        }
    }

    public T Value
    {
        get
        {
            if (!this.isValid)
            {
                this.Validate();
            }

            return this.value;
        }
    }
    
    void ITracker.Track(INotifyChanged signal)
    {
        if (signal == null)
        {
            return;
        }

        this.changeNotifiers.Add(signal);
    }

    public void Dispose()
    {
        foreach (var signal in this.changeNotifiers)
        {
            signal.Changed -= this.ChangeNotifierTrackedChanged;
        }

        if (this.valueChangeNotifier != null)
        {
            this.valueChangeNotifier.Changed -= this.ValueChangeNotifierTrackedChanged;
        }
    }

    private void ChangeNotifierTrackedChanged(object sender, ChangedEventArgs e)
    {
        this.OnTrackedChanged();
    }

    private void ValueChangeNotifierTrackedChanged(object sender, ChangedEventArgs e)
    {
        this.isValueChanged = true;
        this.OnTrackedChanged();
    }

    private void OnTrackedChanged()
    {
        this.isValid = false;

        var handlers = this.Changed;
        handlers?.Invoke(this, new ChangedEventArgs(this));
    }

    private void Validate()
    {
        var oldChangeNotifiers = this.changeNotifiers;
        var oldValueChangeNotifier = this.valueChangeNotifier;

        this.changeNotifiers = new HashSet<INotifyChanged>();
        this.valueChangeNotifier = null;

        var newValue = this.expression(this);

        if (!Equals(newValue, this.value) || this.isValueChanged)
        {
            this.value = newValue;
            ++this.version;
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
        
        this.isValid = true;
        this.isValueChanged = false;
    }
}
