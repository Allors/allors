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

    private Target TypescriptSystemWorkspaceMeta => _ => _
    .After(TypescriptInstall)
    .DependsOn(AllorsDotnetBaseGenerate)
    .DependsOn(EnsureDirectories)
    .Executes(() => NpmRun(s => s
        .AddProcessEnvironmentVariable("npm_config_loglevel", "error")
        .SetProcessWorkingDirectory(Paths.Typescript)
        .SetCommand("system-workspace-meta:test")));


    private Target TypescriptSystemWorkspaceMetaJson => _ => _
        .After(TypescriptInstall)
        .DependsOn(AllorsDotnetBaseGenerate)
        .DependsOn(EnsureDirectories)
        .Executes(() => NpmRun(s => s
            .AddProcessEnvironmentVariable("npm_config_loglevel", "error")
            .SetProcessWorkingDirectory(Paths.Typescript)
            .SetCommand("system-workspace-meta-json:test")));

    private Target TypescriptSystemWorkspaceAdapters => _ => _
        .After(TypescriptInstall)
        .DependsOn(AllorsDotnetBaseGenerate)
        .DependsOn(EnsureDirectories)
        .Executes(() => NpmRun(s => s
            .AddProcessEnvironmentVariable("npm_config_loglevel", "error")
            .SetProcessWorkingDirectory(Paths.Typescript)
            .SetCommand("system-workspace-adapters:test")));

    private Target TypescriptSystemWorkspaceAdaptersJson => _ => _
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
                .SetCommand("system-workspace-adapters-json:test"));
        });

    private Target TypescriptWorkspaceTests => _ => _
         .After(TypescriptInstall)
         .DependsOn(TypescriptSystemWorkspaceMeta)
         .DependsOn(TypescriptSystemWorkspaceMetaJson)
         .DependsOn(TypescriptSystemWorkspaceAdapters);

    private Target TypescriptWorkspaceAdaptersJsonTests => _ => _
        .After(TypescriptInstall)
        .DependsOn(TypescriptSystemWorkspaceAdaptersJson);
}
