using Nuke.Common;

partial class Build
{
    private Target CiDotnetSystem => _ => _
            .DependsOn(DotnetSystemSharedTests)
            .DependsOn(DotnetSystemRepositoryModelTests);

    private Target CiDotnetSystemDatabaseAdaptersMemory => _ => _
        .DependsOn(DotnetSystemDatabaseAdaptersTestMemory);

    private Target CiDotnetSystemDatabaseAdaptersSqlClient => _ => _
        .DependsOn(DotnetSystemDatabaseAdaptersSqlClientTests);

    private Target CiDotnetSystemDatabaseAdaptersNpgsql => _ => _
        .DependsOn(DotnetSystemDatabaseAdaptersNpgsqlTests);

    private Target CiDotnetSystemWorkspaceAdaptersDirect => _ => _
        .DependsOn(DotnetSystemWorkspaceAdaptersDirectTests);

    private Target CiDotnetSystemWorkspaceAdaptersJsonNewtonsoft => _ => _
        .DependsOn(DotnetSystemWorkspaceAdaptersJsonNewtonsoftTests);

    private Target CiDotnetSystemWorkspaceAdaptersJsonSystemText => _ => _
        .DependsOn(DotnetSystemWorkspaceAdaptersJsonSystemTextTests);

    private Target CiDotnetCoreDatabase => _ => _
        .DependsOn(DotnetCoreDatabaseMetaTests)
        .DependsOn(DotnetCoreDatabaseDomainTests)
        .DependsOn(DotnetCoreDatabaseServerDirectTests)
        .DependsOn(DotnetCoreDatabaseServerJsonTests);

    private Target CiDotnetCoreWorkspace => _ => _
        .DependsOn(DotnetCoreWorkspaceMetaStaticTests);

    private Target CiDotnetBaseDatabaseTest => _ => _
        .DependsOn(DotnetBaseDatabaseDomainTests);

    private Target CiTypescriptWorkspace => _ => _
        .DependsOn(TypescriptInstall)
        .DependsOn(TypescriptWorkspaceTests);

    private Target CiTypescriptWorkspaceAdaptersJson => _ => _
        .DependsOn(TypescriptInstall)
        .DependsOn(TypescriptWorkspaceAdaptersJsonTests);
}
