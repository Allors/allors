using Nuke.Common;

partial class Build
{
    private Target CiDotnetSystemAdaptersTestMemory => _ => _
        .DependsOn(DotnetSystemAdaptersTestMemory);

    private Target CiDotnetSystemAdaptersTestSqlClient => _ => _
        .DependsOn(DotnetSystemAdaptersTestSqlClient);

    private Target CiDotnetSystemAdaptersTestNpgsql => _ => _
        .DependsOn(DotnetSystemAdaptersTestNpgsql);

    private Target CiDotnetCoreDatabaseTest => _ => _
        .DependsOn(DotnetCoreDatabaseTest);

    private Target CiDotnetCoreWorkspaceLocalTest => _ => _
        .DependsOn(DotnetCoreWorkspaceLocalTest);

    private Target CiDotnetCoreWorkspaceRemoteJsonSystemTextTest => _ => _
        .DependsOn(DotnetCoreWorkspaceRemoteJsonSystemTextTest);

    private Target CiDotnetCoreWorkspaceRemoteJsonRestSharpTest => _ => _
        .DependsOn(DotnetCoreWorkspaceRemoteJsonRestSharpTest);

    private Target CiDotnetBaseDatabaseTest => _ => _
        .DependsOn(DotnetBaseDatabaseTest);

    private Target CiTypescriptWorkspaceTest => _ => _
        .DependsOn(TypescriptInstall)
        .DependsOn(TypescriptWorkspaceTest);

    private Target CiTypescriptWorkspaceAdaptersJsonTest => _ => _
        .DependsOn(TypescriptInstall)
        .DependsOn(TypescriptWorkspaceAdaptersJsonTest);

    private Target CiDemosTest => _ => _
        .DependsOn(DemosDerivationTest);
}
