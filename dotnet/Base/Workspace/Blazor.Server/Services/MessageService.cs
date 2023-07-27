using Workspace.Blazor.ViewModels.Services;

namespace Workspace.Blazor.Server.Services;

public class MessageService : IMessageService
{
    public void Show(string text, string caption) => Console.WriteLine($"{caption}\n{text}");

    public bool? ShowDialog(string message, string title)
    {
        return false;
    }
}
