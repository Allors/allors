namespace Avalonia.Views;

using System;
using System.Net.Http;
using Splat;
using Allors.Workspace;
using Allors.Workspace.Adapters;
using Allors.Workspace.Configuration;
using Allors.Workspace.Meta.Static;
using Configuration = Allors.Workspace.Adapters.Json.Configuration;
public static class AppBuilderExtensions
{
    public const string Url = "http://localhost:5000/allors/";

    public static AppBuilder UseAllors(this AppBuilder builder)
    {
        return builder.AfterPlatformServicesSetup(_ =>
            {
                var metaPopulation = new MetaBuilder().Build();
                var objectFactory = new ReflectionObjectFactory(metaPopulation, typeof(Allors.Workspace.Domain.Person));
                var serviceBuilder = () => new WorkspaceServices(objectFactory, metaPopulation);
                var configuration = new Configuration("Default", metaPopulation);
                var idGenerator = new IdGenerator();

                var httpClient = new HttpClient { BaseAddress = new Uri(Url), Timeout = TimeSpan.FromMinutes(30) };
                var connection = new Allors.Workspace.Adapters.Json.SystemText.Connection(configuration, serviceBuilder, httpClient, idGenerator);

                Locator.CurrentMutable.RegisterConstant(connection, typeof(Connection));
                Locator.CurrentMutable.RegisterConstant(connection, typeof(Allors.Workspace.Adapters.Json.SystemText.Connection));
                Locator.CurrentMutable.Register(() => connection.CreateWorkspace(), typeof(IWorkspace));
            }
        );
    }
}
