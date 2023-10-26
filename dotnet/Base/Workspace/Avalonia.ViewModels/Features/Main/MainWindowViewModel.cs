namespace Avalonia.ViewModels;

using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Allors.Workspace;
using global::ReactiveUI;
using Splat;

public class MainWindowViewModel : ReactiveObject, IScreen, IActivatableViewModel
{
    public MainWindowViewModel(IWorkspaceFactory workspaceFactory, IMessageService messageService, IAutoLoginService autoLoginService)
    {
        this.Activator = new ViewModelActivator();
        this.WorkspaceFactory = workspaceFactory;
        this.MessageService = messageService;

        this.AutoLogin = ReactiveCommand
            .CreateFromObservable(
                () => Observable
                    .StartAsync(() => autoLoginService.Login("jane@example.com"))
                    .TakeUntil(this.AutoLoginCancel));

        this.AutoLoginCancel = ReactiveCommand.Create(() => { });

        this.GoToPersonManual = ReactiveCommand.CreateFromObservable(
            () => Router.Navigate.Execute(new PersonManualControlViewModel(this.WorkspaceFactory.CreateWorkspace(), this.MessageService, this))
        );

        this.GoToPersonGenerator = ReactiveCommand.CreateFromObservable(
            () => Router.Navigate.Execute(new PersonGeneratorControlViewModel(this.WorkspaceFactory.CreateWorkspace(), this.MessageService, this))
        );

        this.GoBack = Router.NavigateBack;

        this.WhenActivated(disposable =>
        {
            AutoLogin.Execute().Subscribe();
            Disposable
                .Create(() => AutoLoginCancel.Execute().Subscribe())
                .DisposeWith(disposable);
        });

        this.Router.Navigate.Execute(new HomeControlViewModel(this));
    }

    public ViewModelActivator Activator { get; }

    public IWorkspaceFactory WorkspaceFactory { get; }

    public IMessageService MessageService { get; }

    public RoutingState Router { get; } = new RoutingState();

    public ReactiveCommand<Unit, IRoutableViewModel> GoToPersonManual { get; }

    public ReactiveCommand<Unit, IRoutableViewModel> GoToPersonGenerator { get; }

    public ReactiveCommand<Unit, IRoutableViewModel> GoBack { get; }

    public ReactiveCommand<Unit, Unit> AutoLogin;

    public ReactiveCommand<Unit, Unit> AutoLoginCancel { get; }
}
