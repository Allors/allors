using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.Npm;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.Npm.NpmTasks;

partial class Build
{
    private Target DotnetBaseResetDatabase => _ => _
        .Executes(() =>
        {
            var database = "Base";
            using var sqlLocalDb = new SqlLocalDB();
            sqlLocalDb.Init(database);
        });

    private Target DotnetBaseMerge => _ => _
        .Executes(() => DotNetRun(s => s
            .SetProjectFile(Paths.DotnetCoreDatabaseMerge)
            .SetApplicationArguments(
                $"{Paths.DotnetCoreDatabaseResourcesCore} {Paths.DotnetBaseDatabaseResourcesBase} {Paths.DotnetBaseDatabaseResources}")));
    
    private Target DotnetBaseGenerate => _ => _
        .After(Clean)
        .DependsOn(DotnetBaseMerge)
        .Executes(() =>
        {
            DotNetRun(s => s
                .SetProjectFile(Paths.DotnetSystemRepositoryGenerate)
                .SetApplicationArguments(
                    $"{Paths.DotnetBaseRepository} {Paths.DotnetSystemRepositoryTemplatesMetaCs} {Paths.DotnetBaseDatabaseMetaGenerated}"));
            DotNetRun(s => s
                .SetProjectFile(Paths.DotnetSystemRepositoryGenerate)
                .SetApplicationArguments(
                    $"{Paths.DotnetBaseRepository} {Paths.DotnetSystemRepositoryTemplatesMetaConfigurationCs} {Paths.DotnetBaseDatabaseMetaConfigurationGenerated}"));
            DotNetRun(s => s
                .SetProcessWorkingDirectory(Paths.DotnetBase)
                .SetProjectFile(Paths.DotnetBaseDatabaseGenerate));
        });

    private Target DotnetBaseDatabaseDomainTests => _ => _
        .DependsOn(DotnetBaseGenerate)
        .Executes(() => DotNetTest(s => s
            .SetProjectFile(Paths.DotnetBaseDatabaseDomainTests)
            .AddLoggers("trx;LogFileName=BaseDatabaseDomain.trx")
            .SetResultsDirectory(Paths.ArtifactsTests)));
    
    private Target DotnetBaseWorkspaceTypescriptSession => _ => _
        .DependsOn(DotnetBaseGenerate)
        .DependsOn(EnsureDirectories)
        .Executes(() => NpmRun(s => s
            .AddProcessEnvironmentVariable("npm_config_loglevel", "error")
            .SetProcessWorkingDirectory(Paths.DotnetBaseWorkspaceTypescript)
            .SetCommand("domain:test")));
}
