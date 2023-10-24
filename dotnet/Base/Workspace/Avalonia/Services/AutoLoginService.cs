namespace Avalonia.Views;

using System;
using Avalonia.ViewModels;
using System.Reactive;
using System.Threading.Tasks;
using Allors.Workspace.Adapters.Json.SystemText;
using Splat;

public class AutoLoginService : IAutoLoginService
{
    public async Task<Unit> Login(string username)
    {
        var connection = Locator.Current.GetService<Connection>();
        await connection.Login(new Uri("http://localhost:5000/allors/TestAuthentication/Token"), "jane@example.com", "jane");
        return Unit.Default;
    }
}
