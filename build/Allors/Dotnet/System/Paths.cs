using Nuke.Common.IO;

public partial class Paths
{
    public AbsolutePath AllorsDotnetSystem => AllorsDotnet / "System";

    public AbsolutePath AllorsDotnetSystemSharedTests => AllorsDotnetSystem / "Shared.Tests";

    public AbsolutePath AllorsDotnetSystemRepository => AllorsDotnetSystem / "Repository";
    public AbsolutePath AllorsDotnetSystemRepositoryModelTests => AllorsDotnetSystemRepository / "Allors.Repository.Model.Tests";
    public AbsolutePath AllorsDotnetSystemRepositoryTemplates => AllorsDotnetSystemRepository / "Templates";
    public AbsolutePath AllorsDotnetSystemRepositoryTemplatesMetaConfigurationCs => AllorsDotnetSystemRepositoryTemplates / "meta.configuration.cs.stg";
    public AbsolutePath AllorsDotnetSystemRepositoryTemplatesMetaExtensionsCs => AllorsDotnetSystemRepositoryTemplates / "meta.extensions.cs.stg";
    public AbsolutePath AllorsDotnetSystemRepositoryGenerate => AllorsDotnetSystemRepository / "Generate/Generate.csproj";

    public AbsolutePath AllorsDotnetSystemEmbedded => AllorsDotnetSystem / "Embedded";
    public AbsolutePath AllorsDotnetSystemEmbeddedMetaTests => AllorsDotnetSystemEmbedded / "Allors.Embedded.Meta.Tests/Allors.Embedded.Meta.Tests.csproj";
    public AbsolutePath AllorsDotnetSystemEmbeddedDomainMemoryTests => AllorsDotnetSystemEmbedded / "Allors.Embedded.Domain.Memory.Tests/Allors.Embedded.Domain.Memory.Tests.csproj";

    public AbsolutePath AllorsDotnetSystemDatabase => AllorsDotnetSystem / "Database";
    public AbsolutePath AllorsDotnetSystemDatabaseAdapters => AllorsDotnetSystemDatabase / "Adapters";
    public AbsolutePath AllorsDotnetSystemDatabaseTestsAdapters => AllorsDotnetSystemDatabaseAdapters / "Tests";
    public AbsolutePath AllorsDotnetSystemDatabaseTestsAdaptersRepository => AllorsDotnetSystemDatabaseTestsAdapters / "Repository/Repository.csproj";
    public AbsolutePath AllorsDotnetSystemDatabaseTestsAdaptersAllorsAdaptersGenerated => AllorsDotnetSystemDatabaseTestsAdapters / "Allors.Adapters/Generated";
    public AbsolutePath AllorsDotnetSystemDatabaseTestsAdaptersGenerate => AllorsDotnetSystemDatabaseTestsAdapters / "Generate/Generate.csproj";
    public AbsolutePath AllorsDotnetSystemDatabaseTestsAdaptersTests => AllorsDotnetSystemDatabaseTestsAdapters / "Tests/Tests.csproj";

    public AbsolutePath AllorsDotnetSystemWorkspace => AllorsDotnetSystem / "Workspace";
    public AbsolutePath AllorsDotnetSystemWorkspaceSignalsTests => AllorsDotnetSystemWorkspace / "Allors.Workspace.Signals.Tests";
    public AbsolutePath AllorsDotnetSystemWorkspaceSignalsSourceGeneratorsTests => AllorsDotnetSystemWorkspace / "Allors.Workspace.Signals.SourceGenerators.Tests";
    public AbsolutePath AllorsDotnetSystemWorkspaceAdapters => AllorsDotnetSystemWorkspace / "Adapters";
    public AbsolutePath AllorsDotnetSystemWorkspaceAdaptersDirectTests => AllorsDotnetSystemWorkspaceAdapters / "Allors.Workspace.Adapters.Direct.Tests";
    public AbsolutePath AllorsDotnetSystemWorkspaceAdaptersJsonNewtonsoftTests => AllorsDotnetSystemWorkspaceAdapters / "Allors.Workspace.Adapters.Json.Newtonsoft.Tests";
    public AbsolutePath AllorsDotnetSystemWorkspaceAdaptersJsonSystemTextTests => AllorsDotnetSystemWorkspaceAdapters / "Allors.Workspace.Adapters.Json.SystemText.Tests";
}
