using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

partial class Build
{
    private Target DotnetCoreResetDatabase => _ => _
        .Executes(() =>
        {
            var database = "Core";
            using var sqlLocalDb = new SqlLocalDB();
            sqlLocalDb.Init(database);
        });

    private Target DotnetCoreMerge => _ => _
        .Executes(() => DotNetRun(s => s
            .SetProjectFile(Paths.DotnetCoreDatabaseMerge)
            .SetApplicationArguments(
                $"{Paths.DotnetCoreDatabaseResourcesCore} {Paths.DotnetCoreDatabaseResourcesCustom} {Paths.DotnetCoreDatabaseResources}")));

    private Target DotnetCoreGenerate => _ => _
        .After(Clean)
        .DependsOn(DotnetCoreMerge)
        .Executes(() =>
        {
            DotNetRun(s => s
                .SetProjectFile(Paths.DotnetSystemRepositoryGenerate)
                .SetApplicationArguments(
                    $"{Paths.DotnetCoreRepository} {Paths.DotnetSystemRepositoryTemplatesMetaCs} {Paths.DotnetCoreDatabaseMetaGenerated}"));
            DotNetRun(s => s
                .SetProjectFile(Paths.DotnetSystemRepositoryGenerate)
                .SetApplicationArguments(
                    $"{Paths.DotnetCoreRepository} {Paths.DotnetSystemRepositoryTemplatesMetaConfigurationCs} {Paths.DotnetCoreDatabaseMetaConfigurationGenerated}"));
            DotNetRun(s => s
                .SetProcessWorkingDirectory(Paths.DotnetCore)
                .SetProjectFile(Paths.DotnetCoreDatabaseGenerate));
        });

    private Target DotnetCoreDatabaseMetaTests => _ => _
        .DependsOn(DotnetCoreGenerate)
        .Executes(() => DotNetTest(s => s
            .SetProjectFile(Paths.DotnetCoreDatabaseMetaTests)
            .AddLoggers("trx;LogFileName=CoreDatabaseMeta.trx")
            .SetResultsDirectory(Paths.ArtifactsTests)));

    private Target DotnetCoreDatabaseDomainTests => _ => _
        .DependsOn(DotnetCoreGenerate)
        .Executes(() => DotNetTest(s => s
            .SetProjectFile(Paths.DotnetCoreDatabaseDomainTests)
            .AddLoggers("trx;LogFileName=CoreDatabaseDomain.trx")
            .SetResultsDirectory(Paths.ArtifactsTests)));

    private Target DotnetCoreDatabaseServerDirectTests => _ => _
        .DependsOn(DotnetCoreGenerate)
        .Executes(() => DotNetTest(s => s
            .SetProjectFile(Paths.DotnetCoreDatabaseServerDirectTests)
            .AddLoggers("trx;LogFileName=CoreDatabaseApi.trx")
            .SetResultsDirectory(Paths.ArtifactsTests)));

    private Target DotnetCorePublishCommands => _ => _
        .DependsOn(DotnetCoreGenerate)
        .Executes(() =>
        {
            var dotNetPublishSettings = new DotNetPublishSettings()
                .SetProcessWorkingDirectory(Paths.DotnetCoreDatabaseCommands)
                .SetOutput(Paths.ArtifactsCoreCommands);
            DotNetPublish(dotNetPublishSettings);
        });

    private Target DotnetCorePublishServer => _ => _
        .DependsOn(DotnetCoreGenerate)
        .Executes(() =>
        {
            var dotNetPublishSettings = new DotNetPublishSettings()
                .SetProcessWorkingDirectory(Paths.DotnetCoreDatabaseServer)
                .SetOutput(Paths.ArtifactsCoreServer);
            DotNetPublish(dotNetPublishSettings);
        });

    private Target DotnetCoreDatabaseServerJsonTests => _ => _
        .DependsOn(DotnetCoreGenerate)
        .DependsOn(DotnetCorePublishServer)
        .DependsOn(DotnetCorePublishCommands)
        .DependsOn(DotnetCoreResetDatabase)
        .Executes(async () =>
        {
            DotNet("Commands.dll Populate", Paths.ArtifactsCoreCommands);
            using var server = new Server(Paths.ArtifactsCoreServer);
            await server.Ready();
            DotNetTest(s => s
                .SetProjectFile(Paths.DotnetCoreDatabaseServerJsonTests)
                .AddLoggers("trx;LogFileName=CoreDatabaseServer.trx")
                .SetResultsDirectory(Paths.ArtifactsTests));
        });

    private Target DotnetCoreWorkspaceMetaStaticTests => _ => _
        .DependsOn(DotnetCorePublishServer)
        .DependsOn(DotnetCorePublishCommands)
        .DependsOn(DotnetCoreResetDatabase)
        .Executes(() =>
        {
            DotNet("Commands.dll Populate", Paths.ArtifactsCoreCommands);

            {
                DotNetTest(s => s
                    .SetProjectFile(Paths.DotnetCoreWorkspaceMetaTests)
                    .AddLoggers("trx;LogFileName=DotnetCoreWorkspaceMetaTests.trx")
                    .SetResultsDirectory(Paths.ArtifactsTests));
            }
        });
}
