namespace Allors.Workspace.Mvvm.Adapters;

using System;
using System.ComponentModel;
using Allors.Workspace;

public class CompositeViewModelRoleAdapter<TComposite, TViewModel> : IDisposable
    where TComposite : class, IObject
    where TViewModel : class, ICompositeViewModel<TComposite>
{
    private readonly WeakReference<IViewModel> weakViewModel;
    private readonly ICompositeViewModelResolver resolver;
    private readonly ICompositeRole<TComposite> role;
    private readonly string name;

    private TViewModel cache;

    public CompositeViewModelRoleAdapter(IViewModel viewModel, ICompositeViewModelResolver resolver, ICompositeRole<TComposite> role, string name = null)
    {
        this.weakViewModel = new WeakReference<IViewModel>(viewModel);
        this.resolver = resolver;
        this.role = role;
        this.name = name;

        this.role.PropertyChanged += this.Role_PropertyChanged;
    }

    public TViewModel Value
    {
        get => cache ??= (TViewModel)this.resolver.Resolve(typeof(TViewModel), this.role.Value);
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
