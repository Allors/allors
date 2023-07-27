namespace Workspace.Blazor.ViewModels.Controllers;

using Allors.Workspace;
using Features;

public partial interface IViewModel<T> : IPropertyChange where T : IObject
{
    T Model { get; }
}
