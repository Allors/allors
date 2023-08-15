namespace Allors.Workspace.Mvvm.Adapters;

using System;
using System.ComponentModel;
using Allors.Workspace;

public class RoleAdapter<TValue> : IDisposable
{
    public RoleAdapter(IPropertyChange viewModel, IUnitRole<TValue> role)
    {
        this.Role = role;
        this.ChangeNotification = new WeakReference<IPropertyChange>(viewModel);

        this.Role.PropertyChanged += this.Role_PropertyChanged;
    }

    public IUnitRole<TValue> Role { get; }

    public WeakReference<IPropertyChange> ChangeNotification { get; }

    public TValue Value
    {
        get => this.Role.Value;
        set => this.Role.Value = value;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        this.Role.PropertyChanged -= this.Role_PropertyChanged;
    }

    private void Role_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (!this.ChangeNotification.TryGetTarget(out var changeNotification))
        {
            this.Dispose();
            return;
        }

        changeNotification.OnPropertyChanged(new PropertyChangedEventArgs(this.Role.RoleType.Name));
    }
}
