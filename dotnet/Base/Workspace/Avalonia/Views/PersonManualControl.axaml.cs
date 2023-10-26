namespace Avalonia.Views;

using global::ReactiveUI;
using Markup.Xaml;
using ReactiveUI;
using ViewModels;

public partial class PersonManualControl : ReactiveUserControl<PersonManualControlViewModel>
{
    public PersonManualControl()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}
