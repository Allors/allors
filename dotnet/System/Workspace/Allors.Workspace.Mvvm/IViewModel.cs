namespace Allors.Workspace.Mvvm.Adapters;

using System.ComponentModel;

public interface IViewModel
{
    void OnPropertyChanged(PropertyChangedEventArgs e);
}
