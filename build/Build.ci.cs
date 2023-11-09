using Nuke.Common;

partial class Build
{
    private Target CiDotnetSystem => _ => _
        .DependsOn(Reset)
        .DependsOn(AllorsDotnetSystemSharedTests)
        .DependsOn(AllorsDotnetSystemRepositoryModelTests);

    private Target CiDotnetSystemDatabaseAdaptersMemory => _ => _
        .DependsOn(Reset)
        .DependsOn(AllorsDotnetSystemDatabaseAdaptersTestMemory);

    private Target CiDotnetSystemDatabaseAdaptersSqlClient => _ => _
        .DependsOn(Reset)
        .DependsOn(AllorsDotnetSystemDatabaseAdaptersSqlClientTests);

    private Target CiDotnetSystemDatabaseAdaptersNpgsql => _ => _
        .DependsOn(Reset)
        .DependsOn(AllorsDotnetSystemDatabaseAdaptersNpgsqlTests);

    private Target CiDotnetSystemWorkspace => _ => _
        .DependsOn(Reset)
        .DependsOn(AllorsDotnetSystemWorkspaceSignalsTests)
        .DependsOn(AllorsDotnetSystemWorkspaceSignalsSourceGeneratorsTests);

    private Target CiDotnetSystemWorkspaceAdaptersDirect => _ => _
        .DependsOn(Reset)
        .DependsOn(AllorsDotnetSystemWorkspaceAdaptersDirectTests);

    private Target CiDotnetSystemWorkspaceAdaptersJsonNewtonsoft => _ => _
        .DependsOn(Reset)
        .DependsOn(AllorsDotnetSystemWorkspaceAdaptersJsonNewtonsoftTests);

    private Target CiDotnetSystemWorkspaceAdaptersJsonSystemText => _ => _
        .DependsOn(Reset)
        .DependsOn(AllorsDotnetSystemWorkspaceAdaptersJsonSystemTextTests);

    private Target CiDotnetCoreDatabase => _ => _
        .DependsOn(Reset)
        .DependsOn(AllorsDotnetCoreDatabaseMetaTests)
        .DependsOn(AllorsDotnetCoreDatabaseDomainTests)
        .DependsOn(AllorsDotnetCoreDatabaseServerDirectTests)
        .DependsOn(AllorsDotnetCoreDatabaseServerJsonTests);

    private Target CiDotnetCoreWorkspace => _ => _
        .DependsOn(Reset)
        .DependsOn(AllorsDotnetCoreWorkspaceMetaStaticTests);

    private Target CiDotnetBaseDatabaseTest => _ => _
        .DependsOn(Reset)
        .DependsOn(AllorsDotnetBaseDatabaseDomainTests);

    // TODO:
    private Target CiDotnetBaseWorkspace => _ => _
        .DependsOn(Reset);
    
    private Target CiTypescriptWorkspace => _ => _
        .DependsOn(Reset)
        .DependsOn(TypescriptInstall)
        .DependsOn(TypescriptWorkspaceTests);

    private Target CiTypescriptWorkspaceAdaptersJson => _ => _
        .DependsOn(Reset)
        .DependsOn(TypescriptInstall)
        .DependsOn(TypescriptWorkspaceAdaptersJsonTests);
}
