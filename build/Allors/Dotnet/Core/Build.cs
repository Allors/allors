using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

partial class Build
{
    private Target AllorsDotnetCoreResetDatabase => _ => _
        .Executes(() =>
        {
            var database = "Core";
            using var sqlLocalDb = new SqlLocalDB();
            sqlLocalDb.Init(database);
        });

    private Target AllorsDotnetCoreGenerate => _ => _
        .After(Clean)
        .Executes(() =>
        {
            DotNetRun(s => s
                .SetProjectFile(Paths.AllorsDotnetSystemRepositoryGenerate)
                .SetApplicationArguments(
                    $"{Paths.AllorsDotnetCoreRepository} {Paths.AllorsDotnetSystemRepositoryTemplatesMetaCs} {Paths.AllorsDotnetCoreDatabaseMetaGenerated}"));
            DotNetRun(s => s
                .SetProjectFile(Paths.AllorsDotnetSystemRepositoryGenerate)
                .SetApplicationArguments(
                    $"{Paths.AllorsDotnetCoreRepository} {Paths.AllorsDotnetSystemRepositoryTemplatesMetaConfigurationCs} {Paths.AllorsDotnetCoreDatabaseMetaConfigurationGenerated}"));
            DotNetRun(s => s
                .SetProcessWorkingDirectory(Paths.AllorsDotnetCore)
                .SetProjectFile(Paths.AllorsDotnetCoreDatabaseGenerate));
        });

    private Target AllorsDotnetCoreDatabaseMetaTests => _ => _
        .DependsOn(AllorsDotnetCoreGenerate)
        .Executes(() => DotNetTest(s => s
            .SetProjectFile(Paths.AllorsDotnetCoreDatabaseMetaTests)
            .AddLoggers("trx;LogFileName=CoreDatabaseMeta.trx")
            .SetResultsDirectory(Paths.ArtifactsTests)));

    private Target AllorsDotnetCoreDatabaseDomainTests => _ => _
        .DependsOn(AllorsDotnetCoreGenerate)
        .Executes(() => DotNetTest(s => s
            .SetProjectFile(Paths.AllorsDotnetCoreDatabaseDomainTests)
            .AddLoggers("trx;LogFileName=CoreDatabaseDomain.trx")
            .SetResultsDirectory(Paths.ArtifactsTests)));

    private Target AllorsDotnetCoreDatabaseServerDirectTests => _ => _
        .DependsOn(AllorsDotnetCoreGenerate)
        .Executes(() => DotNetTest(s => s
            .SetProjectFile(Paths.AllorsDotnetCoreDatabaseServerDirectTests)
            .AddLoggers("trx;LogFileName=CoreDatabaseApi.trx")
            .SetResultsDirectory(Paths.ArtifactsTests)));

    private Target AllorsDotnetCorePublishCommands => _ => _
        .DependsOn(AllorsDotnetCoreGenerate)
        .Executes(() =>
        {
            var dotNetPublishSettings = new DotNetPublishSettings()
                .SetProcessWorkingDirectory(Paths.AllorsDotnetCoreDatabaseCommands)
                .SetOutput(Paths.ArtifactsCoreCommands);
            DotNetPublish(dotNetPublishSettings);
        });

    private Target AllorsDotnetCorePublishServer => _ => _
        .DependsOn(AllorsDotnetCoreGenerate)
        .Executes(() =>
        {
            var dotNetPublishSettings = new DotNetPublishSettings()
                .SetProcessWorkingDirectory(Paths.AllorsDotnetCoreDatabaseServer)
                .SetOutput(Paths.ArtifactsCoreServer);
            DotNetPublish(dotNetPublishSettings);
        });

    private Target AllorsDotnetCoreDatabaseServerJsonTests => _ => _
        .DependsOn(AllorsDotnetCoreGenerate)
        .DependsOn(AllorsDotnetCorePublishServer)
        .DependsOn(AllorsDotnetCorePublishCommands)
        .DependsOn(AllorsDotnetCoreResetDatabase)
        .Executes(async () =>
        {
            DotNet("Commands.dll Populate", Paths.ArtifactsCoreCommands);
            using var server = new Server(Paths.ArtifactsCoreServer);
            await server.Ready();
            DotNetTest(s => s
                .SetProjectFile(Paths.AllorsDotnetCoreDatabaseServerJsonTests)
                .AddLoggers("trx;LogFileName=CoreDatabaseServer.trx")
                .SetResultsDirectory(Paths.ArtifactsTests));
        });

    private Target AllorsDotnetCoreWorkspaceMetaStaticTests => _ => _
        .DependsOn(AllorsDotnetCorePublishServer)
        .DependsOn(AllorsDotnetCorePublishCommands)
        .DependsOn(AllorsDotnetCoreResetDatabase)
        .Executes(() =>
        {
            DotNet("Commands.dll Populate", Paths.ArtifactsCoreCommands);

            {
                DotNetTest(s => s
                    .SetProjectFile(Paths.AllorsDotnetCoreWorkspaceMetaTests)
                    .AddLoggers("trx;LogFileName=DotnetCoreWorkspaceMetaTests.trx")
                    .SetResultsDirectory(Paths.ArtifactsTests));
            }
        });
}
