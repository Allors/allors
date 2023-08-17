namespace Allors.Workspace.Mvvm;

using System;

public partial interface ICompositeViewModelResolver
{
    ICompositeViewModel<T> Resolve<T>(Type type, T composite) where T : class, IObject;
}
