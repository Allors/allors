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
            .AddLoggers("trx;LogFileName=DotnetSystemSharedTests.trx")
            .SetResultsDirectory(Paths.ArtifactsTests)));

    private Target AllorsDotnetSystemRepositoryModelTests => _ => _
        .Executes(() => DotNetTest(s => s
            .SetProjectFile(Paths.AllorsDotnetSystemRepositoryModelTests)
            .AddLoggers("trx;LogFileName=DotnetSystemRepositoryModelTests.trx")
            .SetResultsDirectory(Paths.ArtifactsTests)));

    private Target AllorsDotnetSystemAdaptersGenerate => _ => _
        .After(Clean)
        .Executes(() =>
        {
            DotNetRun(s => s
                .SetProjectFile(Paths.AllorsDotnetSystemRepositoryGenerate)
                .SetApplicationArguments(
                    $"{Paths.AllorsDotnetSystemAdaptersRepository} {Paths.AllorsDotnetSystemRepositoryTemplatesMetaConfigurationCs} {Paths.AllorsDotnetSystemAdaptersMetaConfigurationGenerated}"));
            DotNetRun(s => s
                .SetProjectFile(Paths.AllorsDotnetSystemRepositoryGenerate)
                .SetApplicationArguments(
                    $"{Paths.AllorsDotnetSystemAdaptersRepository} {Paths.AllorsDotnetSystemRepositoryTemplatesMetaCs} {Paths.AllorsDotnetSystemAdaptersMetaGenerated}"));
            DotNetRun(s => s
                .SetProcessWorkingDirectory(Paths.AllorsDotnetSystemAdapters)
                .SetProjectFile(Paths.AllorsDotnetSystemAdaptersGenerate));
        });

    private Target AllorsDotnetSystemDatabaseAdaptersTestMemory => _ => _
        .DependsOn(AllorsDotnetSystemAdaptersGenerate)
        .Executes(() => DotNetTest(s => s
            .SetProjectFile(Paths.AllorsDotnetSystemAdaptersStaticTests)
            .SetFilter("FullyQualifiedName~Allors.Database.Adapters.Memory")
            .AddLoggers("trx;LogFileName=DotnetSystemAdaptersTestMemory.trx")
            .SetResultsDirectory(Paths.ArtifactsTests)));

    private Target AllorsDotnetSystemDatabaseAdaptersSqlClientTests => _ => _
        .DependsOn(AllorsDotnetSystemAdaptersGenerate)
        .Executes(() =>
        {
            using (new SqlLocalDB())
            {
                DotNetTest(s => s
                    .SetProjectFile(Paths.AllorsDotnetSystemAdaptersStaticTests)
                    .SetFilter("FullyQualifiedName~Allors.Database.Adapters.Sql.SqlClient")
                    .AddLoggers("trx;LogFileName=DotnetSystemAdaptersTestSqlClient.trx")
                    .SetResultsDirectory(Paths.ArtifactsTests));
            }
        });

    private Target AllorsDotnetSystemDatabaseAdaptersNpgsqlTests => _ => _
        .DependsOn(AllorsDotnetSystemAdaptersGenerate)
        .Executes(() =>
        {
            DotNetTest(s => s
                 .SetProjectFile(Paths.AllorsDotnetSystemAdaptersStaticTests)
                 .SetFilter("FullyQualifiedName~Allors.Database.Adapters.Sql.Npgsql")
                 .AddLoggers("trx;LogFileName=DotnetSystemAdaptersTestNpgsql.trx")
                 .SetResultsDirectory(Paths.ArtifactsTests));
        });



    private Target AllorsDotnetSystemWorkspaceAdaptersDirectTests => _ => _
        .DependsOn(AllorsDotnetCorePublishServer)
        .DependsOn(AllorsDotnetCorePublishCommands)
        .DependsOn(AllorsDotnetCoreResetDatabase)
        .Executes(() =>
        {
            DotNet("Commands.dll Populate", Paths.ArtifactsCoreCommands);

            {
                DotNetTest(s => s
                    .SetProjectFile(Paths.AllorsDotnetSystemWorkspaceAdaptersDirectTests)
                    .AddLoggers("trx;LogFileName=DotnetCoreWorkspaceAdaptersDirectTests.trx")
                    .SetResultsDirectory(Paths.ArtifactsTests));
            }
        });

    private Target AllorsDotnetSystemWorkspaceAdaptersJsonNewtonsoftTests => _ => _
        .DependsOn(AllorsDotnetCorePublishServer)
        .DependsOn(AllorsDotnetCorePublishCommands)
        .DependsOn(AllorsDotnetCoreResetDatabase)
        .Executes(async () =>
        {
            DotNet("Commands.dll Populate", Paths.ArtifactsCoreCommands);
            {
                using var server = new Server(Paths.ArtifactsCoreServer);
                await server.Ready();

                DotNetTest(s => s
                    .SetProjectFile(Paths.AllorsDotnetSystemWorkspaceAdaptersJsonNewtonsoftTests)
                    .AddLoggers("trx;LogFileName=DotnetCoreWorkspaceAdaptersJsonNewtonsoftTests.trx")
                    .SetResultsDirectory(Paths.ArtifactsTests));
            }
        });

    private Target AllorsDotnetSystemWorkspaceAdaptersJsonSystemTextTests => _ => _
        .DependsOn(AllorsDotnetCorePublishServer)
        .DependsOn(AllorsDotnetCorePublishCommands)
        .DependsOn(AllorsDotnetCoreResetDatabase)
        .Executes(async () =>
        {
            DotNet("Commands.dll Populate", Paths.ArtifactsCoreCommands);


            {
                using var server = new Server(Paths.ArtifactsCoreServer);
                await server.Ready();

                DotNetTest(s => s
                    .SetProjectFile(Paths.AllorsDotnetSystemWorkspaceAdaptersJsonSystemTextTests)
                    .AddLoggers("trx;LogFileName=DotnetCoreWorkspaceAdaptersJsonSystemTextTests.trx")
                    .SetResultsDirectory(Paths.ArtifactsTests));
            }

        });

    private Target AllorsDotnetSystemWorkspaceSignalsTests => _ => _
        .DependsOn(AllorsDotnetCoreGenerate)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Paths.AllorsDotnetSystemWorkspaceSignalsTests)
                .AddLoggers("trx;LogFileName=AllorsDotnetSystemWorkspaceSignalsTests.trx")
                .SetResultsDirectory(Paths.ArtifactsTests));
        });


    private Target AllorsDotnetSystemWorkspaceMvvmTests => _ => _
        .DependsOn(AllorsDotnetCoreGenerate)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Paths.AllorsDotnetSystemWorkspaceMvvmTests)
                .AddLoggers("trx;LogFileName=AllorsDotnetSystemWorkspaceMvvmTests.trx")
                .SetResultsDirectory(Paths.ArtifactsTests));
        });
}
