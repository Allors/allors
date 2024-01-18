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

    private HashSet<ICacheable> cacheables;
    private ICacheable? valueCacheable;
    private T? cache;
    private bool isCacheInvalid;

    public ComputedSignal(Func<ITracker, T> expression)
    {
        this.expression = expression;
        this.cacheables = new();
        this.isCold = true;
        this.isCacheInvalid = true;
    }

    object ISignal.Value => this.Value;

    public event InvalidationRequestedEventHandler InvalidationRequested
    {
        add
        {
            this.CustomChanged += value;
            this.isCold = false;
        }
        remove
        {
            this.CustomChanged -= value;
            if (this.CustomChanged == null)
            {
                this.isCold = true;
                this.isCacheInvalid = true;

                foreach (var cacheable in this.cacheables)
                {
                    cacheable.InvalidationRequested -= this.Cacheable_InvalidationRequested;
                }

                if (this.valueCacheable != null)
                {
                    this.valueCacheable.InvalidationRequested -= this.ValueCacheable_InvalidationRequested;
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

            if (this.isCacheInvalid)
            {
                this.Validate();
            }

            return this.cache;
        }
    }

    void ITracker.Track(ICacheable? signal)
    {
        if (signal == null)
        {
            return;
        }

        this.cacheables.Add(signal);
    }

    private void Cacheable_InvalidationRequested(object sender, InvalidationRequestedEventArgs e)
    {
        this.Invalidate();
    }

    private void ValueCacheable_InvalidationRequested(object sender, InvalidationRequestedEventArgs e)
    {
        this.Invalidate();
    }

    private void Invalidate()
    {
        this.isCacheInvalid = true;
       
        var handlers = this.CustomChanged;
        handlers?.Invoke(this, new InvalidationRequestedEventArgs(this));
    }

    private void Validate()
    {
        var oldCacheables = this.cacheables;
        var oldValueCacheable = this.valueCacheable;

        this.cacheables = new HashSet<ICacheable>();
        this.valueCacheable = null;

        var newValue = this.expression(this);

        if (!Equals(newValue, this.cache))
        {
            this.cache = newValue;
        }

        this.valueCacheable = newValue as ICacheable;
        if (!Equals(oldValueCacheable, this.valueCacheable) && (oldValueCacheable != null || this.valueCacheable != null))
        {
            if (oldValueCacheable != null)
            {
                oldValueCacheable.InvalidationRequested -= this.ValueCacheable_InvalidationRequested;
            }

            if (this.valueCacheable != null)
            {
                this.valueCacheable.InvalidationRequested += this.ValueCacheable_InvalidationRequested;
            }
        }

        var cacheablesToRemove = oldCacheables.Except(this.cacheables);
        foreach (var cacheable in cacheablesToRemove)
        {
            if (cacheable != this.valueCacheable)
            {
                cacheable.InvalidationRequested -= this.ValueCacheable_InvalidationRequested;
            }
        }

        var cacheablesToAdd = this.cacheables.Except(oldCacheables);
        foreach (var cacheable in cacheablesToAdd)
        {
            if (cacheable != this.valueCacheable)
            {
                cacheable.InvalidationRequested += this.ValueCacheable_InvalidationRequested;
            }
        }

        this.isCold = false;
        this.isCacheInvalid = false;
    }

    private class NoopTracker : ITracker
    {
        public void Track(ICacheable? signal)
        {
        }
    }
}
