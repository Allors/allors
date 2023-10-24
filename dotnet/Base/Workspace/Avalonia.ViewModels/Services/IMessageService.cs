namespace Avalonia.ViewModels;

public interface IMessageService
{
    Task Show(string message, string title);

    Task<bool> ShowDialog(string message, string title);
}
