namespace Workspace.ViewModels.WinForms
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Allors.Workspace;
    using Allors.Workspace.Adapters;
    using Allors.Workspace.Meta;
    using Allors.Workspace.Meta.Static;
    using Features;
    using Forms;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Services;
    using ViewModels.Services;
    using Configuration = Allors.Workspace.Adapters.Json.Configuration;

    internal static partial class Program
    {
        public const string Url = "http://localhost:5000/allors/";

        public static IServiceProvider ServiceProvider { get; private set; }

        static IHostBuilder CreateHostBuilder()
        {
            var metaPopulation = new MetaBuilder().Build();
            var objectFactory = new ReflectionObjectFactory(metaPopulation, typeof(Allors.Workspace.Domain.Person));
            var serviceBuilder = () => new WorkspaceServices(objectFactory, metaPopulation);
            var configuration = new Configuration("Default", metaPopulation);
            var idGenerator = new IdGenerator();

            var httpClient = new HttpClient { BaseAddress = new Uri(Url), Timeout = TimeSpan.FromMinutes(30) };
            var connection = new Allors.Workspace.Adapters.Json.SystemText.Connection(configuration, serviceBuilder, httpClient, idGenerator);

            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // Allors
                    services.AddSingleton(connection);
                    services.AddSingleton<Connection>(connection);
                    services.AddSingleton<IDatabaseService, DatabaseService>();
                    services.AddScoped(v => v.GetService<IDatabaseService>().CreateWorkspace());

                    // Services
                    services.AddSingleton<IMdiService, MdiService>();
                    services.AddSingleton<IMessageService, MessageService>();

                    // Controllers
                    services.AddScoped<MainFormViewModel>();
                    services.AddScoped<PersonFormViewModel>();

                    // Forms
                    services.AddSingleton<MainForm>();
                    services.AddTransient<PersonForm>();
                });
        }
    }
}
