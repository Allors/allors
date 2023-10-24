namespace Avalonia.Views;

using System.Threading.Tasks;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ViewModels;

public class MessageService : IMessageService
{
    public async Task Show(string text, string caption)
    {
        var box = MessageBoxManager
            .GetMessageBoxStandard(caption, text,
                ButtonEnum.YesNo);

        await box.ShowAsync();
    }

    public async Task<bool> ShowDialog(string message, string title)
    {
        var box = MessageBoxManager
            .GetMessageBoxStandard(title, message,
                ButtonEnum.YesNo);

        return await box.ShowAsync() == ButtonResult.Yes;
    }
}
