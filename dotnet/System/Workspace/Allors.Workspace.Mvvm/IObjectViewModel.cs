namespace Allors.Workspace.Mvvm;

using Adapters;
using Allors.Workspace;

public partial interface IObjectViewModel<out T> : IViewModel where T : IObject
{
    T Model { get; }
}
