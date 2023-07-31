namespace Allors.Workspace.Mvvm.Adapters;

using System;
using System.ComponentModel;
using Allors.Workspace;
using Data;
using Meta;

public class PathAdapter<T> : IDisposable
{
    public PathAdapter(IObject @object, Node path, WeakReference<IPropertyChange> propertyChange, string propertyName)
    {
        this.Object = @object;
        this.Path = path;
        this.PropertyChange = propertyChange;
        this.PropertyName = propertyName;
    }

    public IObject Object { get; }

    public Node Path { get; }

    public WeakReference<IPropertyChange> PropertyChange { get; }

    public string PropertyName { get; }
    
    public T Value
    {
        get => (T)this.Object.Strategy.Role((IRoleType)this.Path.Nodes[0].PropertyType).Value;
        set => this.Object.Strategy.Role((IRoleType)this.Path.Nodes[0].PropertyType).Value = value;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        //this.Role.PropertyChanged -= this.Role_PropertyChanged;
    }

    private void Role_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        //if (!this.ChangeNotification.TryGetTarget(out var changeNotification))
        //{
        //    this.Dispose();
        //    return;
        //}

        //changeNotification.OnPropertyChanged(new PropertyChangedEventArgs(this.Role.RoleType.Name));
    }
}
