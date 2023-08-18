namespace Allors.Workspace.Mvvm;

using Adapters;

public delegate IViewModel ViewModelFactory(IObject @object);

public partial interface IViewModelResolver
{
    IViewModel Resolve(IObject @object);
}
