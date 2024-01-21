namespace Allors.Workspace.Signals;

using System;
using System.Collections.Generic;
using System.Linq;

public sealed class ComputedSignal<T> : ISignal<T>, ITracker
{
    private static readonly NoopTracker ColdTracker = new();

    private readonly Func<ITracker, T> expression;

    private event ChangedEventHandler CustomChanged;

    private bool isCold;

    private HashSet<IChangeable> changeableOperands;
    private IChangeable? changeableResult;

    private T? value;
    private bool isInvalid;

    public ComputedSignal(Func<ITracker, T> expression)
    {
        this.expression = expression;
        this.changeableOperands = new();
        this.isCold = true;
    }

    object ISignal.Value => this.Value;

    public event ChangedEventHandler Changed
    {
        add
        {
            this.CustomChanged += value;
            this.isCold = false;
            this.isInvalid = true;
        }
        remove
        {
            this.CustomChanged -= value;
            if (this.CustomChanged == null)
            {
                this.isCold = true;

                foreach (var changeableOperand in this.changeableOperands)
                {
                    changeableOperand.Changed -= this.ChangeableOperand_Changed;
                }

                if (this.changeableResult != null)
                {
                    this.changeableResult.Changed -= this.ChangeableResultChanged;
                }
            }
        }
    }

    public T? Value
    {
        get
        {
            if (this.isCold)
            {
                return this.expression(ColdTracker);
            }

            if (this.isInvalid)
            {
                this.Validate();
            }
            
            return this.value;
        }
    }

    void ITracker.Track(IChangeable? signal)
    {
        if (signal == null)
        {
            return;
        }

        this.changeableOperands.Add(signal);
    }

    private void ChangeableOperand_Changed(object sender, ChangedEventArgs e)
    {
        this.Invalidate();
    }

    private void ChangeableResultChanged(object sender, ChangedEventArgs e)
    {
        this.Invalidate();
    }

    private void Invalidate()
    {
        this.isInvalid = true;
       
        var handlers = this.CustomChanged;
        handlers?.Invoke(this, new ChangedEventArgs(this));
    }

    private void Validate()
    {
        var oldChangeableOperand = this.changeableOperands;
        var oldChangeableResult = this.changeableResult;

        this.changeableOperands = new HashSet<IChangeable>();
        this.changeableResult = null;

        var newValue = this.expression(this);

        this.value = newValue;

        this.changeableResult = newValue as IChangeable;
        if (!Equals(oldChangeableResult, this.changeableResult) && (oldChangeableResult != null || this.changeableResult != null))
        {
            if (oldChangeableResult != null)
            {
                oldChangeableResult.Changed -= this.ChangeableResultChanged;
            }

            if (this.changeableResult != null)
            {
                this.changeableResult.Changed += this.ChangeableResultChanged;
            }
        }

        var changeableOperandToRemove = oldChangeableOperand.Except(this.changeableOperands);
        foreach (var changeableOperand in changeableOperandToRemove)
        {
            if (changeableOperand != this.changeableResult)
            {
                changeableOperand.Changed -= this.ChangeableResultChanged;
            }
        }

        var changeableOperandsToAdd = this.changeableOperands.Except(oldChangeableOperand);
        foreach (var changeableOperand in changeableOperandsToAdd)
        {
            if (changeableOperand != this.changeableResult)
            {
                changeableOperand.Changed += this.ChangeableResultChanged;
            }
        }

        this.isCold = false;
        this.isInvalid = false;
    }

    private class NoopTracker : ITracker
    {
        public void Track(IChangeable? signal)
        {
        }
    }
}
