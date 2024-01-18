namespace Allors.Workspace.Signals;

using System;
using System.Collections.Generic;
using System.Linq;

public sealed class ComputedSignal<T> : ISignal<T>, ITracker
{
    private static readonly NoopTracker ColdTracker = new();

    private readonly Func<ITracker, T> expression;

    private event InvalidationRequestedEventHandler CustomChanged;

    private bool isCold;

    private HashSet<ICacheable> cacheableOperands;
    private ICacheable? cacheableResult;

    private T? value;
    private bool isInvalid;

    public ComputedSignal(Func<ITracker, T> expression)
    {
        this.expression = expression;
        this.cacheableOperands = new();
        this.isCold = true;
    }

    object ISignal.Value => this.Value;

    public event InvalidationRequestedEventHandler InvalidationRequested
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

                foreach (var cacheableOperand in this.cacheableOperands)
                {
                    cacheableOperand.InvalidationRequested -= this.CacheableOperand_InvalidationRequested;
                }

                if (this.cacheableResult != null)
                {
                    this.cacheableResult.InvalidationRequested -= this.CacheableResult_InvalidationRequested;
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

    void ITracker.Track(ICacheable? signal)
    {
        if (signal == null)
        {
            return;
        }

        this.cacheableOperands.Add(signal);
    }

    private void CacheableOperand_InvalidationRequested(object sender, InvalidationRequestedEventArgs e)
    {
        this.Invalidate();
    }

    private void CacheableResult_InvalidationRequested(object sender, InvalidationRequestedEventArgs e)
    {
        this.Invalidate();
    }

    private void Invalidate()
    {
        this.isInvalid = true;
       
        var handlers = this.CustomChanged;
        handlers?.Invoke(this, new InvalidationRequestedEventArgs(this));
    }

    private void Validate()
    {
        var oldCacheableOperand = this.cacheableOperands;
        var oldCacheableResult = this.cacheableResult;

        this.cacheableOperands = new HashSet<ICacheable>();
        this.cacheableResult = null;

        var newValue = this.expression(this);

        this.value = newValue;

        this.cacheableResult = newValue as ICacheable;
        if (!Equals(oldCacheableResult, this.cacheableResult) && (oldCacheableResult != null || this.cacheableResult != null))
        {
            if (oldCacheableResult != null)
            {
                oldCacheableResult.InvalidationRequested -= this.CacheableResult_InvalidationRequested;
            }

            if (this.cacheableResult != null)
            {
                this.cacheableResult.InvalidationRequested += this.CacheableResult_InvalidationRequested;
            }
        }

        var cacheableOperandToRemove = oldCacheableOperand.Except(this.cacheableOperands);
        foreach (var cacheableOperand in cacheableOperandToRemove)
        {
            if (cacheableOperand != this.cacheableResult)
            {
                cacheableOperand.InvalidationRequested -= this.CacheableResult_InvalidationRequested;
            }
        }

        var cacheableOperandsToAdd = this.cacheableOperands.Except(oldCacheableOperand);
        foreach (var cacheableOperand in cacheableOperandsToAdd)
        {
            if (cacheableOperand != this.cacheableResult)
            {
                cacheableOperand.InvalidationRequested += this.CacheableResult_InvalidationRequested;
            }
        }

        this.isCold = false;
        this.isInvalid = false;
    }

    private class NoopTracker : ITracker
    {
        public void Track(ICacheable? signal)
        {
        }
    }
}
