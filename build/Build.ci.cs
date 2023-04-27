using Nuke.Common;

partial class Build
{
    private Target CiDotnetSystemTest => _ => _
        .DependsOn(DotnetSystemTest);

    private Target CiDotnetSystemAdaptersTestMemory => _ => _
        .DependsOn(DotnetSystemAdaptersTestMemory);

    private Target CiDotnetSystemAdaptersTestSqlClient => _ => _
        .DependsOn(DotnetSystemAdaptersTestSqlClient);

    private Target CiDotnetSystemAdaptersTestNpgsql => _ => _
        .DependsOn(DotnetSystemAdaptersTestNpgsql);

    private Target CiDotnetCoreDatabaseTest => _ => _
        .DependsOn(DotnetCoreDatabaseTest);

    private Target CiDotnetCoreWorkspaceDirectTest => _ => _
        .DependsOn(DotnetCoreWorkspaceDirectTest);

    private Target CiDotnetCoreWorkspaceJsonSystemTextTest => _ => _
        .DependsOn(DotnetCoreWorkspaceJsonSystemTextHttpClientTest);

    private Target CiDotnetCoreWorkspaceJsonNewtonsoftTest => _ => _
        .DependsOn(DotnetCoreWorkspaceJsonNewtonsoftWebClientTest);

    private Target CiTypescriptWorkspaceTest => _ => _
        .DependsOn(TypescriptInstall)
        .DependsOn(TypescriptWorkspaceTest);

    private Target CiTypescriptWorkspaceAdaptersJsonTest => _ => _
        .DependsOn(TypescriptInstall)
        .DependsOn(TypescriptWorkspaceAdaptersJsonTest);
}
