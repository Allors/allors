using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.Npm.NpmTasks;

partial class Build
{
    private Target AllorsDotnetSystemSharedTests => _ => _
        .Executes(() => DotNetTest(s => s
            .SetProjectFile(Paths.AllorsDotnetSystemSharedTests)
            .AddLoggers("trx;LogFileName=AllorsDotnetSystemSharedTests.trx")
            .SetResultsDirectory(Paths.ArtifactsTests)));

    private Target AllorsDotnetSystemRepositoryModelTests => _ => _
        .Executes(() => DotNetTest(s => s
            .SetProjectFile(Paths.AllorsDotnetSystemRepositoryModelTests)
            .AddLoggers("trx;LogFileName=AllorsDotnetSystemRepositoryModelTests.trx")
            .SetResultsDirectory(Paths.ArtifactsTests)));
    
    private Target AllorsDotnetSystemEmbeddedTests => _ => _
        .DependsOn(AllorsDotnetBaseGenerate)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Paths.AllorsDotnetSystemEmbeddedMetaTests)
                .AddLoggers("trx;LogFileName=AllorsDotnetSystemEmbeddedTests.trx")
                .SetResultsDirectory(Paths.ArtifactsTests));
        });

    private Target AllorsDotnetSystemEmbeddedDomainMemoryTests => _ => _
        .DependsOn(AllorsDotnetBaseGenerate)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Paths.AllorsDotnetSystemEmbeddedDomainMemoryTests)
                .AddLoggers("trx;LogFileName=AllorsDotnetSystemEmbeddedDomainMemoryTests.trx")
                .SetResultsDirectory(Paths.ArtifactsTests));
        });

    private Target AllorsDotnetSystemAdaptersGenerate => _ => _
        .After(Clean)
        .Executes(() =>
        {
            DotNetRun(s => s
                .SetProjectFile(Paths.AllorsDotnetSystemRepositoryGenerate)
                .SetApplicationArguments(
                    $"{Paths.AllorsDotnetSystemDatabaseTestsAdaptersRepository} {Paths.AllorsDotnetSystemRepositoryTemplatesMetaConfigurationCs} {Paths.AllorsDotnetSystemDatabaseTestsAdaptersAllorsAdaptersGenerated}"));
            DotNetRun(s => s
                .SetProcessWorkingDirectory(Paths.AllorsDotnetSystemDatabaseTestsAdapters)
                .SetProjectFile(Paths.AllorsDotnetSystemDatabaseTestsAdaptersGenerate));
        });

    private Target AllorsDotnetSystemDatabaseAdaptersTestMemory => _ => _
        .DependsOn(AllorsDotnetSystemAdaptersGenerate)
        .Executes(() => DotNetTest(s => s
            .SetProjectFile(Paths.AllorsDotnetSystemDatabaseTestsAdaptersTests)
            .SetFilter("FullyQualifiedName~Allors.Database.Adapters.Memory")
            .AddLoggers("trx;LogFileName=AllorsDotnetSystemDatabaseAdaptersTestMemory.trx")
            .SetResultsDirectory(Paths.ArtifactsTests)));

    private Target AllorsDotnetSystemDatabaseAdaptersSqlClientTests => _ => _
        .DependsOn(AllorsDotnetSystemAdaptersGenerate)
        .Executes(() =>
        {
            using (new SqlLocalDB())
            {
                DotNetTest(s => s
                    .SetProjectFile(Paths.AllorsDotnetSystemDatabaseTestsAdaptersTests)
                    .SetFilter("FullyQualifiedName~Allors.Database.Adapters.Sql.SqlClient")
                    .AddLoggers("trx;LogFileName=AllorsDotnetSystemDatabaseAdaptersSqlClientTests.trx")
                    .SetResultsDirectory(Paths.ArtifactsTests));
            }
        });

    private Target AllorsDotnetSystemDatabaseAdaptersNpgsqlTests => _ => _
        .DependsOn(AllorsDotnetSystemAdaptersGenerate)
        .Executes(() =>
        {
            DotNetTest(s => s
                 .SetProjectFile(Paths.AllorsDotnetSystemDatabaseTestsAdaptersTests)
                 .SetFilter("FullyQualifiedName~Allors.Database.Adapters.Sql.Npgsql")
                 .AddLoggers("trx;LogFileName=AllorsDotnetSystemDatabaseAdaptersNpgsqlTests.trx")
                 .SetResultsDirectory(Paths.ArtifactsTests));
        });

    
    private Target AllorsDotnetSystemWorkspaceAdaptersDirectTests => _ => _
        .DependsOn(AllorsDotnetBasePublishServer)
        .DependsOn(AllorsDotnetBasePublishCommands)
        .DependsOn(AllorsDotnetBaseResetDatabase)
        .Executes(() =>
        {
            DotNet("Commands.dll Populate", Paths.ArtifactsCommands);

            {
                DotNetTest(s => s
                    .SetProjectFile(Paths.AllorsDotnetSystemWorkspaceAdaptersDirectTests)
                    .AddLoggers("trx;LogFileName=AllorsDotnetSystemWorkspaceAdaptersDirectTests.trx")
                    .SetResultsDirectory(Paths.ArtifactsTests));
            }
        });

    private Target AllorsDotnetSystemWorkspaceAdaptersJsonNewtonsoftTests => _ => _
        .DependsOn(AllorsDotnetBasePublishServer)
        .DependsOn(AllorsDotnetBasePublishCommands)
        .DependsOn(AllorsDotnetBaseResetDatabase)
        .Executes(async () =>
        {
            DotNet("Commands.dll Populate", Paths.ArtifactsCommands);
            {
                using var server = new Server(Paths.ArtifactsServer);
                await server.Ready();

                DotNetTest(s => s
                    .SetProjectFile(Paths.AllorsDotnetSystemWorkspaceAdaptersJsonNewtonsoftTests)
                    .AddLoggers("trx;LogFileName=AllorsDotnetSystemWorkspaceAdaptersJsonNewtonsoftTests.trx")
                    .SetResultsDirectory(Paths.ArtifactsTests));
            }
        });

    private Target AllorsDotnetSystemWorkspaceAdaptersJsonSystemTextTests => _ => _
        .DependsOn(AllorsDotnetBasePublishServer)
        .DependsOn(AllorsDotnetBasePublishCommands)
        .DependsOn(AllorsDotnetBaseResetDatabase)
        .Executes(async () =>
        {
            DotNet("Commands.dll Populate", Paths.ArtifactsCommands);


            {
                using var server = new Server(Paths.ArtifactsServer);
                await server.Ready();

                DotNetTest(s => s
                    .SetProjectFile(Paths.AllorsDotnetSystemWorkspaceAdaptersJsonSystemTextTests)
                    .AddLoggers("trx;LogFileName=AllorsDotnetSystemWorkspaceAdaptersJsonSystemTextTests.trx")
                    .SetResultsDirectory(Paths.ArtifactsTests));
            }

        });

    private Target AllorsDotnetSystemWorkspaceSignalsTests => _ => _
        .DependsOn(AllorsDotnetBaseGenerate)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Paths.AllorsDotnetSystemWorkspaceSignalsTests)
                .AddLoggers("trx;LogFileName=AllorsDotnetSystemWorkspaceSignalsTests.trx")
                .SetResultsDirectory(Paths.ArtifactsTests));
        });
}
