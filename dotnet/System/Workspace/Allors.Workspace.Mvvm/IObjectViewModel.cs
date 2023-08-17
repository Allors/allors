namespace Allors.Workspace.Mvvm;

using Adapters;
using Allors.Workspace;

public partial interface IObjectViewModel<T> : IViewModel where T : IObject
{
    T Model { get; }
}
