namespace Workspace.ViewModels.Controllers;

using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Features;

public abstract partial class ViewModel : ObservableObject, IPropertyChange
{
	public new void OnPropertyChanged(PropertyChangedEventArgs e) => base.OnPropertyChanged(e);
}
