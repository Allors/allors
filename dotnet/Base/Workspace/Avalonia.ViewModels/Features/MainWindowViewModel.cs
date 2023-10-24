namespace Avalonia.ViewModels;

using System.Reactive;
using global::ReactiveUI;

public class MainWindowViewModel : ReactiveObject, IScreen
{
    public RoutingState Router { get; } = new RoutingState();

    public ReactiveCommand<Unit, IRoutableViewModel> GoNext { get; }

    public ReactiveCommand<Unit, IRoutableViewModel> GoBack => Router.NavigateBack;

    public MainWindowViewModel()
    {
        GoNext = ReactiveCommand.CreateFromObservable(
            () => Router.Navigate.Execute(new FirstViewModel(this))
        );
    }
}
