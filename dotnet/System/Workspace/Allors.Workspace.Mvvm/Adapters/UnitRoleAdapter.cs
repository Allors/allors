namespace Allors.Workspace.Mvvm.Adapters;

using System;
using System.ComponentModel;
using Allors.Workspace;

public class UnitRoleAdapter<TValue> : IDisposable
{
    private readonly WeakReference<IViewModel> weakViewModel;
    private readonly IUnitRole<TValue> role;
    private readonly string name;

    public UnitRoleAdapter(IViewModel viewModel, IUnitRole<TValue> role, string name = null)
    {
        this.weakViewModel = new WeakReference<IViewModel>(viewModel);
        this.role = role;
        this.name = name;

        this.role.PropertyChanged += this.Role_PropertyChanged;
    }


    public TValue Value
    {
        get => this.role.Value;
        set => this.role.Value = value;
    }

    private string Name => this.name ?? this.role.RoleType.Name;

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        this.role.PropertyChanged -= this.Role_PropertyChanged;
    }

    private void Role_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (!this.weakViewModel.TryGetTarget(out var changeNotification))
        {
            this.Dispose();
            return;
        }

        changeNotification.OnPropertyChanged(new PropertyChangedEventArgs(this.Name));
    }
}
