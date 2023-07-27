namespace Workspace.Blazor.ViewModels.Features;

using System.ComponentModel;

public interface IPropertyChange
{
    void OnPropertyChanged(PropertyChangedEventArgs e);
}
