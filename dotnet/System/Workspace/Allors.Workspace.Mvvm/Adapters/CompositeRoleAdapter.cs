namespace Allors.Workspace.Mvvm.Adapters;

using System;
using System.ComponentModel;
using Allors.Workspace;

public class CompositeRoleAdapter<TComposite> : IDisposable
    where TComposite : class, IObject
{
    private readonly WeakReference<IViewModel> weakViewModel;
    private readonly ICompositeRole<TComposite> role;
    private readonly string name;

    public CompositeRoleAdapter(IViewModel viewModel, ICompositeRole<TComposite> role, string name = null)
    {
        this.weakViewModel = new WeakReference<IViewModel>(viewModel);
        this.role = role;
        this.name = name;

        this.role.PropertyChanged += this.Role_PropertyChanged;
    }


    public TComposite Value
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
