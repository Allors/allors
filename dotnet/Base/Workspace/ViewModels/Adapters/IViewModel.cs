namespace Workspace.ViewModels.Controllers;

using Allors.Workspace;
using Features;

public partial interface IViewModel<T> : IPropertyChange where T : IObject
{
    T Model { get; }
}
