namespace Allors.Workspace.Mvvm;

using Adapters;
using Allors.Workspace;

public partial interface ICompositeViewModel<out T> : IViewModel where T : IObject
{
    T Model { get; }
}
