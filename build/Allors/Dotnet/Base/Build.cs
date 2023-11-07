using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.Npm;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.Npm.NpmTasks;

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
            DotNetRun(s => s
                .SetProjectFile(Paths.AllorsDotnetSystemRepositoryGenerate)
                .SetApplicationArguments(
                    $"{Paths.AllorsDotnetBaseRepository} {Paths.AllorsDotnetSystemRepositoryTemplatesMetaCs} {Paths.AllorsDotnetBaseDatabaseMetaGenerated}"));
            DotNetRun(s => s
                .SetProjectFile(Paths.AllorsDotnetSystemRepositoryGenerate)
                .SetApplicationArguments(
                    $"{Paths.AllorsDotnetBaseRepository} {Paths.AllorsDotnetSystemRepositoryTemplatesMetaConfigurationCs} {Paths.AllorsDotnetBaseDatabaseMetaConfigurationGenerated}"));
            DotNetRun(s => s
                .SetProcessWorkingDirectory(Paths.AllorsDotnetBase)
                .SetProjectFile(Paths.AllorsDotnetBaseDatabaseGenerate));
        });

    private Target AllorsDotnetBaseDatabaseDomainTests => _ => _
        .DependsOn(AllorsDotnetBaseGenerate)
        .Executes(() => DotNetTest(s => s
            .SetProjectFile(Paths.AllorsDotnetBaseDatabaseDomainTests)
            .AddLoggers("trx;LogFileName=BaseDatabaseDomain.trx")
            .SetResultsDirectory(Paths.ArtifactsTests)));
}
