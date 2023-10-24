namespace Avalonia.Views;

using global::ReactiveUI;
using Markup.Xaml;
using ReactiveUI;
using ViewModels;

public partial class PersonControl : ReactiveUserControl<PersonManualControlViewModel>
{
    public PersonControl()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}
