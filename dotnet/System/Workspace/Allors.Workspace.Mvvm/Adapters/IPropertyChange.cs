namespace Allors.Workspace.Mvvm.Adapters;

using System.ComponentModel;

public interface IPropertyChange
{
    void OnPropertyChanged(PropertyChangedEventArgs e);
}
