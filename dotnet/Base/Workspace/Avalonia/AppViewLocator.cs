using System;
using Avalonia.ViewModels;
using ReactiveUI;

namespace Avalonia.Views;

public class AppViewLocator : IViewLocator
{
    public IViewFor ResolveView<T>(T viewModel, string contract = null) => viewModel switch
    {
        HomeControlViewModel context => new HomeControl { DataContext = context },
        PersonManualControlViewModel context => new PersonControl { DataContext = context },
        _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
    };
}
