using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Npm;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.Npm.NpmTasks;

partial class Build
{
    private Target TypescriptInstall => _ => _
        .Executes(() => NpmInstall(s => s
            .AddProcessEnvironmentVariable("npm_config_loglevel", "error")
            .SetProcessWorkingDirectory(Paths.Typescript)));

    private Target TypescriptWorkspaceSystemMeta => _ => _
    .After(TypescriptInstall)
    .DependsOn(AllorsDotnetBaseGenerate)
    .DependsOn(EnsureDirectories)
    .Executes(() => NpmRun(s => s
        .AddProcessEnvironmentVariable("npm_config_loglevel", "error")
        .SetProcessWorkingDirectory(Paths.Typescript)
        .SetCommand("workspace-system-meta:test")));

    private Target TypescriptWorkspaceSystemMetaJson => _ => _
        .After(TypescriptInstall)
        .DependsOn(AllorsDotnetBaseGenerate)
        .DependsOn(EnsureDirectories)
        .Executes(() => NpmRun(s => s
            .AddProcessEnvironmentVariable("npm_config_loglevel", "error")
            .SetProcessWorkingDirectory(Paths.Typescript)
            .SetCommand("workspace-system-meta-json:test")));

    private Target TypescriptWorkspaceSystemAdapters => _ => _
        .After(TypescriptInstall)
        .DependsOn(AllorsDotnetBaseGenerate)
        .DependsOn(EnsureDirectories)
        .Executes(() => NpmRun(s => s
            .AddProcessEnvironmentVariable("npm_config_loglevel", "error")
            .SetProcessWorkingDirectory(Paths.Typescript)
            .SetCommand("workspace-system-adapters:test")));

    private Target TypescriptWorkspaceSystemAdaptersJson => _ => _
        .After(TypescriptInstall)
        .DependsOn(EnsureDirectories)
        .DependsOn(AllorsDotnetBaseGenerate)
        .DependsOn(AllorsDotnetBasePublishServer)
        .DependsOn(AllorsDotnetBasePublishCommands)
        .DependsOn(AllorsDotnetBaseResetDatabase)
        .Executes(async () =>
        {
            DotNet("Commands.dll Populate", Paths.ArtifactsCommands);
            using var server = new Server(Paths.ArtifactsServer);
            await server.Ready();
            NpmRun(s => s
                .AddProcessEnvironmentVariable("npm_config_loglevel", "error")
                .SetProcessWorkingDirectory(Paths.Typescript)
                .SetCommand("workspace-system-adapters-json:test"));
        });

    private Target TypescriptWorkspaceTests => _ => _
         .After(TypescriptInstall)
         .DependsOn(TypescriptWorkspaceSystemMeta)
         .DependsOn(TypescriptWorkspaceSystemMetaJson)
         .DependsOn(TypescriptWorkspaceSystemAdapters);

    private Target TypescriptWorkspaceAdaptersJsonTests => _ => _
        .After(TypescriptInstall)
        .DependsOn(TypescriptWorkspaceSystemAdaptersJson);
}
