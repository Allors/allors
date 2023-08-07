namespace Allors.Workspace.Mvvm.Adapters;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Allors.Workspace;
using Data;

public class PathAdapter<T> : IDisposable
{
    public PathAdapter(IStrategy strategy, Node path, WeakReference<IPropertyChange> propertyChange, string propertyName)
    {
        this.Strategy = strategy;
        this.Path = path;
        this.PropertyChange = propertyChange;
        this.PropertyName = propertyName;
    }

    public IStrategy Strategy { get; }

    public Node Path { get; }

    public WeakReference<IPropertyChange> PropertyChange { get; }

    public string PropertyName { get; }

    public IObjectFactory ObjectFactory { get; set; }

    public IRelationEnd[] Bindings { get; private set; }

    public IRelationEnd RelationEnd
    {
        get
        {
            this.Bind();
            return this.Bindings[this.Bindings.Length - 1];
        }
    }

    public bool Complete { get; private set; }

    public T Value
    {
        get
        {
            this.Bind();

            if (!this.Complete)
            {
                return default;
            }

            if (this.RelationEnd is IRole role)
            {
                return (T)role.Value;
            }

            var association = (IAssociation)this.RelationEnd;
            return (T)association.Value;
        }

        set
        {
            if (!this.Complete || this.RelationEnd is not IRole role)
            {
                return;
            }

            role.Value = value;
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        this.Unbind();
    }

    private void Bind()
    {
        if (this.Bindings == null)
        {
            var bindings = new List<IRelationEnd>();
            this.Path.Flatten(this.Strategy, bindings, out var complete);

            this.Complete = complete;

            // TODO: check that last binding not IsMany

            this.Bindings = bindings.ToArray();

            for (int i = 0; i < this.Bindings.Length; i++)
            {
                var binding = this.Bindings[i];

                if (!this.Complete || i != this.Bindings.Length - 1)
                {
                    binding.PropertyChanged += this.RebindAndNotifyOnPropertyChanged;
                }
                else
                {
                    binding.PropertyChanged += this.NotifyOnPropertyChanged;
                }
            }
        }
    }

    private void Unbind()
    {
        if (this.Bindings != null)
        {
            for (int i = 0; i < this.Bindings.Length; i++)
            {
                var binding = this.Bindings[i];

                if (i != this.Bindings.Length - 1)
                {
                    binding.PropertyChanged -= this.RebindAndNotifyOnPropertyChanged;
                }
                else
                {
                    binding.PropertyChanged -= this.NotifyOnPropertyChanged;
                }
            }

            this.Bindings = null;
        }
    }

    private void RebindAndNotifyOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        this.Unbind();
        this.Bind();

        this.NotifyOnPropertyChanged(sender, e);
    }

    private void NotifyOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (!this.PropertyChange.TryGetTarget(out var propertyChange))
        {
            this.Unbind();
            return;
        }

        if (this.Complete)
        {
            propertyChange.OnPropertyChanged(new PropertyChangedEventArgs(this.PropertyName));
        }
    }
}
