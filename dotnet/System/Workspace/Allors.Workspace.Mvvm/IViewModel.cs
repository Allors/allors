namespace Allors.Workspace.Mvvm;

using Adapters;
using Allors.Workspace;

public partial interface IViewModel<T> : IPropertyChange where T : IObject
{
    T Model { get; }
}
