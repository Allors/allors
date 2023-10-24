namespace Avalonia.Views;

using global::ReactiveUI;
using Markup.Xaml;
using ReactiveUI;
using ViewModels;

public partial class HomeControl : ReactiveUserControl<HomeControlViewModel>
{
    public HomeControl()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}
