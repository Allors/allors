namespace Workspace.WinForms.ViewModels.Controllers;

using System.ComponentModel;
using Allors.Workspace;
using CommunityToolkit.Mvvm.ComponentModel;
using Features;

public abstract partial class ViewModel<T> : ObservableObject, IViewModel<T> where T : IObject
{
    public abstract T Model { get; }

    public new void OnPropertyChanged(PropertyChangedEventArgs e) => base.OnPropertyChanged(e);
}
