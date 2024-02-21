using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

partial class Build
{
    private Target AllorsDotnetBaseResetDatabase => _ => _
        .Executes(() =>
        {
            var database = "Base";
            using var sqlLocalDb = new SqlLocalDB();
            sqlLocalDb.Init(database);
        });

    private Target AllorsDotnetBaseGenerate => _ => _
        .After(Clean)
        .Executes(() =>
        {
            //DotNetRun(s => s
            //    .SetProjectFile(Paths.AllorsDotnetSystemRepositoryGenerate)
            //    .SetApplicationArguments(
            //        $"{Paths.AllorsDotnetBaseRepository} {Paths.AllorsDotnetSystemRepositoryTemplatesMetaCs} {Paths.AllorsDotnetBaseDatabaseMetaGenerated}"));
            DotNetRun(s => s
                .SetProjectFile(Paths.AllorsDotnetSystemRepositoryGenerate)
                .SetApplicationArguments(
                    $"{Paths.AllorsDotnetBaseRepository} {Paths.AllorsDotnetSystemRepositoryTemplatesMetaConfigurationCs} {Paths.AllorsDotnetBaseDatabaseMetaConfigurationGenerated}"));
            DotNetRun(s => s
                .SetProjectFile(Paths.AllorsDotnetSystemRepositoryGenerate)
                .SetApplicationArguments(
                    $"{Paths.AllorsDotnetBaseRepository} {Paths.AllorsDotnetSystemRepositoryTemplatesMetaExtensionsCs} /temp/meta.extensions"));
            DotNetRun(s => s
                .SetProcessWorkingDirectory(Paths.AllorsDotnetBase)
                .SetProjectFile(Paths.AllorsDotnetBaseDatabaseGenerate));
        });

    private Target AllorsDotnetBaseDatabaseMetaTests => _ => _
        .DependsOn(AllorsDotnetBaseGenerate)
        .Executes(() => DotNetTest(s => s
            .SetProjectFile(Paths.AllorsDotnetBaseDatabaseMetaTests)
            .AddLoggers("trx;LogFileName=AllorsDotnetBaseDatabaseMetaTests.trx")
            .SetResultsDirectory(Paths.ArtifactsTests)));

    private Target AllorsDotnetBaseDatabaseDomainTests => _ => _
        .DependsOn(AllorsDotnetBaseGenerate)
        .Executes(() => DotNetTest(s => s
            .SetProjectFile(Paths.AllorsDotnetBaseDatabaseDomainTests)
            .AddLoggers("trx;LogFileName=AllorsDotnetBaseDatabaseDomainTests.trx")
            .SetResultsDirectory(Paths.ArtifactsTests)));

    private Target AllorsDotnetBaseDatabaseServerDirectTests => _ => _
        .DependsOn(AllorsDotnetBaseGenerate)
        .Executes(() => DotNetTest(s => s
            .SetProjectFile(Paths.AllorsDotnetBaseDatabaseServerDirectTests)
            .AddLoggers("trx;LogFileName=AllorsDotnetBaseDatabaseServerDirectTests.trx")
            .SetResultsDirectory(Paths.ArtifactsTests)));

    private Target AllorsDotnetBasePublishCommands => _ => _
        .DependsOn(AllorsDotnetBaseGenerate)
        .Executes(() =>
        {
            var dotNetPublishSettings = new DotNetPublishSettings()
                .SetProcessWorkingDirectory(Paths.AllorsDotnetBaseDatabaseCommands)
                .SetOutput(Paths.ArtifactsCommands);
            DotNetPublish(dotNetPublishSettings);
        });

    private Target AllorsDotnetBasePublishServer => _ => _
        .DependsOn(AllorsDotnetBaseGenerate)
        .Executes(() =>
        {
            var dotNetPublishSettings = new DotNetPublishSettings()
                .SetProcessWorkingDirectory(Paths.AllorsDotnetBaseDatabaseServer)
                .SetOutput(Paths.ArtifactsServer);
            DotNetPublish(dotNetPublishSettings);
        });

    private Target AllorsDotnetBaseDatabaseServerJsonTests => _ => _
        .DependsOn(AllorsDotnetBaseGenerate)
        .DependsOn(AllorsDotnetBasePublishServer)
        .DependsOn(AllorsDotnetBasePublishCommands)
        .DependsOn(AllorsDotnetBaseResetDatabase)
        .Executes(async () =>
        {
            DotNet("Commands.dll Populate", Paths.ArtifactsCommands);
            using var server = new Server(Paths.ArtifactsServer);
            await server.Ready();
            DotNetTest(s => s
                .SetProjectFile(Paths.AllorsDotnetBaseDatabaseServerJsonTests)
                .AddLoggers("trx;LogFileName=AllorsDotnetBaseDatabaseServerJsonTests.trx")
                .SetResultsDirectory(Paths.ArtifactsTests));
        });

    private Target AllorsDotnetBaseWorkspaceMetaStaticTests => _ => _
        .DependsOn(AllorsDotnetBasePublishServer)
        .DependsOn(AllorsDotnetBasePublishCommands)
        .DependsOn(AllorsDotnetBaseResetDatabase)
        .Executes(() =>
        {
            DotNet("Commands.dll Populate", Paths.ArtifactsCommands);

            {
                DotNetTest(s => s
                    .SetProjectFile(Paths.AllorsDotnetBaseWorkspaceMetaTests)
                    .AddLoggers("trx;LogFileName=AllorsDotnetBaseWorkspaceMetaStaticTests.trx")
                    .SetResultsDirectory(Paths.ArtifactsTests));
            }
        });

    private Target AllorsDotnetBaseWorkspaceWinformsViewModelsTests => _ => _
        .DependsOn(AllorsDotnetBaseGenerate)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Paths.AllorsDotnetBaseWorkspaceWinformsViewModelsTests)
                .AddLoggers("trx;LogFileName=AllorsDotnetBaseWorkspaceWinformsViewModelsTests.trx")
                .SetResultsDirectory(Paths.ArtifactsTests));
        });
}
