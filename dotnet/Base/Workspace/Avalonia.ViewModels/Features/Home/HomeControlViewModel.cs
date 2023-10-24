namespace Avalonia.ViewModels;

using global::ReactiveUI;

public class HomeControlViewModel : ReactiveObject, IRoutableViewModel
{
    public IScreen HostScreen { get; }

    public string UrlPathSegment { get; } = "Home";

    public HomeControlViewModel(IScreen screen) => HostScreen = screen;
}
