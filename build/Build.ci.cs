using Nuke.Common;

partial class Build
{
    private Target CiDotnetSystem => _ => _
        .DependsOn(unknown => unknown
            .DependsOn(DotnetSystemSharedTest)
            .DependsOn(DotnetSystemRepositoryModelTest));

    private Target CiDotnetSystemAdaptersMemory => _ => _
        .DependsOn(DotnetSystemAdaptersTestMemory);

    private Target CiDotnetSystemAdaptersSqlClient => _ => _
        .DependsOn(DotnetSystemAdaptersTestSqlClient);

    private Target CiDotnetSystemAdaptersNpgsql => _ => _
        .DependsOn(DotnetSystemAdaptersTestNpgsql);

    private Target CiDotnetCoreDatabase => _ => _
        .DependsOn(DotnetCoreDatabaseTestMeta)
        .DependsOn(DotnetCoreDatabaseTestDomain)
        .DependsOn(DotnetCoreDatabaseTestServerLocal)
        .DependsOn(DotnetCoreDatabaseTestServerRemote);

    private Target CiDotnetCoreWorkspace => _ => _
        .DependsOn(DotnetCoreWorkspaceMetaStaticTest);

    private Target CiDotnetCoreWorkspaceAdaptersDirect => _ => _
        .DependsOn(DotnetCoreWorkspaceAdaptersDirectTest);

    private Target CiDotnetCoreWorkspaceAdaptersJsonNewtonsoft => _ => _
        .DependsOn(DotnetCoreWorkspaceAdaptersJsonNewtonsoftTest);

    private Target CiDotnetCoreWorkspaceAdaptersJsonSystemText => _ => _
        .DependsOn(DotnetCoreWorkspaceAdaptersJsonSystemTextTest);

    private Target CiTypescriptWorkspace => _ => _
        .DependsOn(TypescriptInstall)
        .DependsOn(TypescriptWorkspaceTest);

    private Target CiTypescriptWorkspaceAdaptersJson => _ => _
        .DependsOn(TypescriptInstall)
        .DependsOn(TypescriptWorkspaceAdaptersJsonTest);
}
