namespace Allors.Workspace.Signals.Tests;

using System.Collections.Concurrent;
using Mvvm;
using Mvvm.Adapters;

public class Resolver : IViewModelResolver
{
    private readonly ViewModelFactory factory;
    private readonly IDictionary<IObject, IViewModel> viewModelByObject;

    public Resolver(ViewModelFactory factory)
    {
        this.factory = factory;
        this.viewModelByObject = new ConcurrentDictionary<IObject, IViewModel>();
    }

    public IViewModel Resolve(IObject @object)
    {
        if (!this.viewModelByObject.TryGetValue(@object, out var viewModel))
        {
            viewModel = this.factory(@object);
            this.viewModelByObject[@object] = viewModel;
        }

        return viewModel;
    }
}