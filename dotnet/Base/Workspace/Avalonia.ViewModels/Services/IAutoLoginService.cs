namespace Avalonia.ViewModels;

using System.Reactive;

public interface IAutoLoginService
{
    public Task<Unit> Login(string username);
}
