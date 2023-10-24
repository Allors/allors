namespace Avalonia.Views;

using global::ReactiveUI;
using Markup.Xaml;
using ReactiveUI;
using ViewModels;

public partial class FirstView : ReactiveUserControl<FirstViewModel>
{
    public FirstView()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}
