namespace Avalonia.ViewModels;

using global::ReactiveUI;

public abstract class ViewModel : ReactiveObject
{
    public void OnEffect(string propertyName)
    {
        this.RaisePropertyChanged(propertyName);
    }
}
