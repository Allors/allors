using Nuke.Common.IO;

public partial class Paths
{
    public AbsolutePath AllorsDotnetSystem => AllorsDotnet / "System";

    public AbsolutePath AllorsDotnetSystemSharedTests => AllorsDotnetSystem / "Shared.Tests";

    public AbsolutePath AllorsDotnetSystemRepository => AllorsDotnetSystem / "Repository";
    public AbsolutePath AllorsDotnetSystemRepositoryModelTests => AllorsDotnetSystemRepository / "Allors.Repository.Model.Tests";
    public AbsolutePath AllorsDotnetSystemRepositoryTemplates => AllorsDotnetSystemRepository / "Templates";
    public AbsolutePath AllorsDotnetSystemRepositoryTemplatesMetaCs => AllorsDotnetSystemRepositoryTemplates / "meta.cs.stg";
    public AbsolutePath AllorsDotnetSystemRepositoryTemplatesMetaConfigurationCs => AllorsDotnetSystemRepositoryTemplates / "meta.configuration.cs.stg";
    public AbsolutePath AllorsDotnetSystemRepositoryTemplatesMetaExtensionsCs => AllorsDotnetSystemRepositoryTemplates / "meta.extensions.cs.stg";
    public AbsolutePath AllorsDotnetSystemRepositoryGenerate => AllorsDotnetSystemRepository / "Generate/Generate.csproj";

    public AbsolutePath AllorsDotnetSystemEmbedded => AllorsDotnetSystem / "Embedded";
    public AbsolutePath AllorsDotnetSystemEmbeddedDomainTests => AllorsDotnetSystemEmbedded / "Allors.Embedded.Domain.Tests/Allors.Embedded.Domain.Tests.csproj";
    public AbsolutePath AllorsDotnetSystemEmbeddedMetaTests => AllorsDotnetSystemEmbedded / "Allors.Embedded.Meta.Tests/Allors.Embedded.Meta.Tests.csproj";
    
    public AbsolutePath AllorsDotnetSystemDatabase => AllorsDotnetSystem / "Database";
    public AbsolutePath AllorsDotnetSystemDatabaseAdapters => AllorsDotnetSystemDatabase / "Adapters";
    public AbsolutePath AllorsDotnetSystemDatabaseAdaptersRepository => AllorsDotnetSystemDatabaseAdapters / "Repository/Repository.csproj";
    public AbsolutePath AllorsDotnetSystemDatabaseAdaptersMetaGenerated => AllorsDotnetSystemDatabaseAdapters / "Meta/Generated";
    public AbsolutePath AllorsDotnetSystemDatabaseAdaptersMetaConfigurationGenerated => AllorsDotnetSystemDatabaseAdapters / "Meta.Configuration/Generated";
    public AbsolutePath AllorsDotnetSystemDatabaseAdaptersGenerate => AllorsDotnetSystemDatabaseAdapters / "Generate/Generate.csproj";
    public AbsolutePath AllorsDotnetSystemDatabaseAdaptersTests => AllorsDotnetSystemDatabaseAdapters / "Tests/Tests.csproj";

    public AbsolutePath AllorsDotnetSystemWorkspace => AllorsDotnetSystem / "Workspace";
    public AbsolutePath AllorsDotnetSystemWorkspaceSignalsTests => AllorsDotnetSystemWorkspace / "Allors.Workspace.Signals.Tests";
    public AbsolutePath AllorsDotnetSystemWorkspaceSignalsSourceGeneratorsTests => AllorsDotnetSystemWorkspace / "Allors.Workspace.Signals.SourceGenerators.Tests";
    public AbsolutePath AllorsDotnetSystemWorkspaceAdapters => AllorsDotnetSystemWorkspace / "Adapters";
    public AbsolutePath AllorsDotnetSystemWorkspaceAdaptersDirectTests => AllorsDotnetSystemWorkspaceAdapters / "Allors.Workspace.Adapters.Direct.Tests";
    public AbsolutePath AllorsDotnetSystemWorkspaceAdaptersJsonNewtonsoftTests => AllorsDotnetSystemWorkspaceAdapters / "Allors.Workspace.Adapters.Json.Newtonsoft.Tests";
    public AbsolutePath AllorsDotnetSystemWorkspaceAdaptersJsonSystemTextTests => AllorsDotnetSystemWorkspaceAdapters / "Allors.Workspace.Adapters.Json.SystemText.Tests";
}
