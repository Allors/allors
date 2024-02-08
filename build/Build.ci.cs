using Nuke.Common;

partial class Build
{
    private Target CiDotnetSystem => _ => _
        .DependsOn(Reset)
        .DependsOn(AllorsDotnetSystemSharedTests)
        .DependsOn(AllorsDotnetSystemRepositoryModelTests)
        .DependsOn(AllorsDotnetSystemEmbeddedDomainTests)
        .DependsOn(AllorsDotnetSystemEmbeddedMetaTests);

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
        .DependsOn(AllorsDotnetSystemWorkspaceSignalsTests);

    private Target CiDotnetSystemWorkspaceAdaptersDirect => _ => _
        .DependsOn(Reset)
        .DependsOn(AllorsDotnetSystemWorkspaceAdaptersDirectTests);

    private Target CiDotnetSystemWorkspaceAdaptersJsonNewtonsoft => _ => _
        .DependsOn(Reset)
        .DependsOn(AllorsDotnetSystemWorkspaceAdaptersJsonNewtonsoftTests);

    private Target CiDotnetSystemWorkspaceAdaptersJsonSystemText => _ => _
        .DependsOn(Reset)
        .DependsOn(AllorsDotnetSystemWorkspaceAdaptersJsonSystemTextTests);

    private Target CiDotnetBaseDatabase => _ => _
        .DependsOn(Reset)
        .DependsOn(AllorsDotnetBaseDatabaseMetaTests)
        .DependsOn(AllorsDotnetBaseDatabaseDomainTests)
        .DependsOn(AllorsDotnetBaseDatabaseServerDirectTests)
        .DependsOn(AllorsDotnetBaseDatabaseServerJsonTests);

    private Target CiDotnetBaseWorkspace => _ => _
        .DependsOn(Reset)
        .DependsOn(AllorsDotnetBaseWorkspaceMetaStaticTests)
        .DependsOn(AllorsDotnetBaseWorkspaceWinformsViewModelsTests);

    private Target CiTypescriptWorkspace => _ => _
        .DependsOn(Reset)
        .DependsOn(TypescriptInstall)
        .DependsOn(TypescriptWorkspaceTests);

    private Target CiTypescriptWorkspaceAdaptersJson => _ => _
        .DependsOn(Reset)
        .DependsOn(TypescriptInstall)
        .DependsOn(TypescriptWorkspaceAdaptersJsonTests);
}
