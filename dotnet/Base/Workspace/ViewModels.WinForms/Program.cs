namespace Workspace.ViewModels.WinForms
{
    using Allors.Workspace.Adapters.Json.SystemText;
    using Forms;
    using Microsoft.Extensions.DependencyInjection;

    internal static partial class Program
    {
        [STAThread]
        static async Task Main()
        {
            ApplicationConfiguration.Initialize();

            var host = CreateHostBuilder().Build();
            ServiceProvider = host.Services;

            // TODO: Login Form
            var databaseConnection = ServiceProvider.GetRequiredService<Connection>();
            await databaseConnection.Login(new Uri("http://localhost:5000/allors/TestAuthentication/Token"), "jane@example.com", "jane");

            Application.Run(ServiceProvider.GetRequiredService<MainForm>());
        }
    }
}
