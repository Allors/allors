namespace Avalonia.Views;

using global::ReactiveUI;
using Markup.Xaml;
using ReactiveUI;
using ViewModels;

public partial class PersonGeneratorControl : ReactiveUserControl<PersonGeneratorControlViewModel>
{
    public PersonGeneratorControl()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}
