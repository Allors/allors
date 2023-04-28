using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.Npm.NpmTasks;

partial class Build
{
    private Target DotnetSystemSharedTests => _ => _
        .Executes(() => DotNetTest(s => s
            .SetProjectFile(Paths.DotnetSystemSharedTests)
            .AddLoggers("trx;LogFileName=DotnetSystemSharedTests.trx")
            .SetResultsDirectory(Paths.ArtifactsTests)));

    private Target DotnetSystemRepositoryModelTests => _ => _
        .Executes(() => DotNetTest(s => s
            .SetProjectFile(Paths.DotnetSystemRepositoryModelTests)
            .AddLoggers("trx;LogFileName=DotnetSystemRepositoryModelTests.trx")
            .SetResultsDirectory(Paths.ArtifactsTests)));

    private Target DotnetSystemAdaptersGenerate => _ => _
        .After(Clean)
        .Executes(() =>
        {
            DotNetRun(s => s
                .SetProjectFile(Paths.DotnetSystemRepositoryGenerate)
                .SetApplicationArguments(
                    $"{Paths.DotnetSystemAdaptersRepository} {Paths.DotnetSystemRepositoryTemplatesMetaConfigurationCs} {Paths.DotnetSystemAdaptersMetaConfigurationGenerated}"));
            DotNetRun(s => s
                .SetProjectFile(Paths.DotnetSystemRepositoryGenerate)
                .SetApplicationArguments(
                    $"{Paths.DotnetSystemAdaptersRepository} {Paths.DotnetSystemRepositoryTemplatesMetaCs} {Paths.DotnetSystemAdaptersMetaGenerated}"));
            DotNetRun(s => s
                .SetProcessWorkingDirectory(Paths.DotnetSystemAdapters)
                .SetProjectFile(Paths.DotnetSystemAdaptersGenerate));
        });

    private Target DotnetSystemDatabaseAdaptersTestMemory => _ => _
        .DependsOn(DotnetSystemAdaptersGenerate)
        .Executes(() => DotNetTest(s => s
            .SetProjectFile(Paths.DotnetSystemAdaptersStaticTests)
            .SetFilter("FullyQualifiedName~Allors.Database.Adapters.Memory")
            .AddLoggers("trx;LogFileName=DotnetSystemAdaptersTestMemory.trx")
            .SetResultsDirectory(Paths.ArtifactsTests)));

    private Target DotnetSystemDatabaseAdaptersSqlClientTests => _ => _
        .DependsOn(DotnetSystemAdaptersGenerate)
        .Executes(() =>
        {
            using (new SqlLocalDB())
            {
                DotNetTest(s => s
                    .SetProjectFile(Paths.DotnetSystemAdaptersStaticTests)
                    .SetFilter("FullyQualifiedName~Allors.Database.Adapters.Sql.SqlClient")
                    .AddLoggers("trx;LogFileName=DotnetSystemAdaptersTestSqlClient.trx")
                    .SetResultsDirectory(Paths.ArtifactsTests));
            }
        });

    private Target DotnetSystemDatabaseAdaptersNpgsqlTests => _ => _
        .DependsOn(DotnetSystemAdaptersGenerate)
        .Executes(() =>
        {
            DotNetTest(s => s
                 .SetProjectFile(Paths.DotnetSystemAdaptersStaticTests)
                 .SetFilter("FullyQualifiedName~Allors.Database.Adapters.Sql.Npgsql")
                 .AddLoggers("trx;LogFileName=DotnetSystemAdaptersTestNpgsql.trx")
                 .SetResultsDirectory(Paths.ArtifactsTests));
        });



    private Target DotnetSystemWorkspaceAdaptersDirectTests => _ => _
        .DependsOn(DotnetCorePublishServer)
        .DependsOn(DotnetCorePublishCommands)
        .DependsOn(DotnetCoreResetDatabase)
        .Executes(() =>
        {
            DotNet("Commands.dll Populate", Paths.ArtifactsCoreCommands);

            {
                DotNetTest(s => s
                    .SetProjectFile(Paths.DotnetSystemWorkspaceAdaptersDirectTests)
                    .AddLoggers("trx;LogFileName=DotnetCoreWorkspaceAdaptersDirectTests.trx")
                    .SetResultsDirectory(Paths.ArtifactsTests));
            }
        });

    private Target DotnetSystemWorkspaceAdaptersJsonNewtonsoftTests => _ => _
        .DependsOn(DotnetCorePublishServer)
        .DependsOn(DotnetCorePublishCommands)
        .DependsOn(DotnetCoreResetDatabase)
        .Executes(async () =>
        {
            DotNet("Commands.dll Populate", Paths.ArtifactsCoreCommands);
            {
                using var server = new Server(Paths.ArtifactsCoreServer);
                await server.Ready();

                DotNetTest(s => s
                    .SetProjectFile(Paths.DotnetSystemWorkspaceAdaptersJsonNewtonsoftTests)
                    .AddLoggers("trx;LogFileName=DotnetCoreWorkspaceAdaptersJsonNewtonsoftTests.trx")
                    .SetResultsDirectory(Paths.ArtifactsTests));
            }
        });

    private Target DotnetSystemWorkspaceAdaptersJsonSystemTextTests => _ => _
        .DependsOn(DotnetCorePublishServer)
        .DependsOn(DotnetCorePublishCommands)
        .DependsOn(DotnetCoreResetDatabase)
        .Executes(async () =>
        {
            DotNet("Commands.dll Populate", Paths.ArtifactsCoreCommands);


            {
                using var server = new Server(Paths.ArtifactsCoreServer);
                await server.Ready();

                DotNetTest(s => s
                    .SetProjectFile(Paths.DotnetSystemWorkspaceAdaptersJsonSystemTextTests)
                    .AddLoggers("trx;LogFileName=DotnetCoreWorkspaceAdaptersJsonSystemTextTests.trx")
                    .SetResultsDirectory(Paths.ArtifactsTests));
            }

        });
    
}
