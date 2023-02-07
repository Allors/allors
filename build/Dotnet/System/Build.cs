using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.Npm.NpmTasks;

partial class Build
{
    private Target DotnetSystemSharedTest => _ => _
        .Executes(() => DotNetTest(s => s
            .SetProjectFile(Paths.DotnetSystemSharedTests)
            .SetFilter("FullyQualifiedName~Allors.Shared")
            .AddLoggers("trx;LogFileName=Shared.trx")
            .SetResultsDirectory(Paths.ArtifactsTests)));

    private Target DotnetSystemRepositoryModelTest => _ => _
        .Executes(() => DotNetTest(s => s
            .SetProjectFile(Paths.DotnetSystemRepositoryModelTests)
            .SetFilter("FullyQualifiedName~Allors.Repository.Model")
            .AddLoggers("trx;LogFileName=RepositoryModel.trx")
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

    private Target DotnetSystemAdaptersTestMemory => _ => _
        .DependsOn(DotnetSystemAdaptersGenerate)
        .Executes(() => DotNetTest(s => s
            .SetProjectFile(Paths.DotnetSystemAdaptersStaticTests)
            .SetFilter("FullyQualifiedName~Allors.Database.Adapters.Memory")
            .AddLoggers("trx;LogFileName=AdaptersMemory.trx")
            .SetResultsDirectory(Paths.ArtifactsTests)));

    private Target DotnetSystemAdaptersTestSqlClient => _ => _
        .DependsOn(DotnetSystemAdaptersGenerate)
        .Executes(() =>
        {
            using (new SqlLocalDB())
            {
                DotNetTest(s => s
                    .SetProjectFile(Paths.DotnetSystemAdaptersStaticTests)
                    .SetFilter("FullyQualifiedName~Allors.Database.Adapters.Sql.SqlClient")
                    .AddLoggers("trx;LogFileName=AdaptersSqlClient.trx")
                    .SetResultsDirectory(Paths.ArtifactsTests));
            }
        });

    private Target DotnetSystemAdaptersTestNpgsql => _ => _
        .DependsOn(DotnetSystemAdaptersGenerate)
        .Executes(() =>
        {
            DotNetTest(s => s
                 .SetProjectFile(Paths.DotnetSystemAdaptersStaticTests)
                 .SetFilter("FullyQualifiedName~Allors.Database.Adapters.Sql.Npgsql")
                 .AddLoggers("trx;LogFileName=AdaptersNpgsql.trx")
                 .SetResultsDirectory(Paths.ArtifactsTests));
        });

    private Target DotnetSystemAdaptersTest => _ => _
        .DependsOn(Clean)
        .DependsOn(DotnetSystemAdaptersTestMemory)
        .DependsOn(DotnetSystemAdaptersTestSqlClient)
        .DependsOn(DotnetSystemAdaptersTestNpgsql);

    private Target DotnetSystemTest => _ => _
        .DependsOn(Clean)
        .DependsOn(DotnetSystemSharedTest)
        .DependsOn(DotnetSystemRepositoryModelTest);
}
