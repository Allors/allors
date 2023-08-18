namespace Allors.Workspace.Mvvm.Adapters;

using System;
using System.ComponentModel;
using Allors.Workspace;

public class CompositeRoleViewModelAdapter<TObject, TViewModel> : IDisposable
    where TObject : class, IObject
    where TViewModel : class, IObjectViewModel<TObject>
{
    private readonly WeakReference<IViewModel> weakViewModel;
    private readonly IViewModelResolver resolver;
    private readonly ICompositeRole<TObject> role;
    private readonly string name;

    private TViewModel cache;

    public CompositeRoleViewModelAdapter(IViewModel viewModel, IViewModelResolver resolver, ICompositeRole<TObject> role, string name = null)
    {
        this.weakViewModel = new WeakReference<IViewModel>(viewModel);
        this.resolver = resolver;
        this.role = role;
        this.name = name;

        this.role.PropertyChanged += this.Role_PropertyChanged;
    }

    public TViewModel Value
    {
        get => cache ??= (TViewModel)this.resolver.Resolve( this.role.Value);
        set => this.role.Value = value.Model;
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

        this.cache = null;

        changeNotification.OnPropertyChanged(new PropertyChangedEventArgs(this.Name));
    }
}
