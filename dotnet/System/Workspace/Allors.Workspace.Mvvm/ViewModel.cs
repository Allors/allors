namespace Allors.Workspace.Mvvm;

using System.ComponentModel;
using Allors.Workspace;
using CommunityToolkit.Mvvm.ComponentModel;

public abstract partial class ViewModel<T> : ObservableObject, ICompositeViewModel<T> where T : IObject
{
    public abstract T Model { get; }

    public new void OnPropertyChanged(PropertyChangedEventArgs e) => base.OnPropertyChanged(e);
}
