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
    public AbsolutePath AllorsDotnetSystemRepositoryGenerate => AllorsDotnetSystemRepository / "Generate/Generate.csproj";

    public AbsolutePath AllorsDotnetSystemDatabase => AllorsDotnetSystem / "Database";
    public AbsolutePath AllorsDotnetSystemAdapters => AllorsDotnetSystemDatabase / "Adapters";
    public AbsolutePath AllorsDotnetSystemAdaptersRepository => AllorsDotnetSystemAdapters / "Repository/Repository.csproj";
    public AbsolutePath AllorsDotnetSystemAdaptersMetaGenerated => AllorsDotnetSystemAdapters / "Meta/Generated";
    public AbsolutePath AllorsDotnetSystemAdaptersMetaConfigurationGenerated => AllorsDotnetSystemAdapters / "Meta.Configuration/Generated";
    public AbsolutePath AllorsDotnetSystemAdaptersGenerate => AllorsDotnetSystemAdapters / "Generate/Generate.csproj";
    public AbsolutePath AllorsDotnetSystemAdaptersStaticTests => AllorsDotnetSystemAdapters / "Tests.Static/Tests.Static.csproj";

    public AbsolutePath AllorsDotnetSystemWorkspace => AllorsDotnetSystem / "Workspace";
    public AbsolutePath AllorsDotnetSystemWorkspaceSignalsTests => AllorsDotnetSystemWorkspaceAdapters / "Allors.Workspace.Signals.Tests";
    public AbsolutePath AllorsDotnetSystemWorkspaceMvvmTests => AllorsDotnetSystemWorkspaceAdapters / "Allors.Workspace.Mvvm.Tests";
    public AbsolutePath AllorsDotnetSystemWorkspaceAdapters => AllorsDotnetSystemWorkspace / "Adapters";
    public AbsolutePath AllorsDotnetSystemWorkspaceAdaptersDirectTests => AllorsDotnetSystemWorkspaceAdapters / "Allors.Workspace.Adapters.Direct.Tests";
    public AbsolutePath AllorsDotnetSystemWorkspaceAdaptersJsonNewtonsoftTests => AllorsDotnetSystemWorkspaceAdapters / "Allors.Workspace.Adapters.Json.Newtonsoft.Tests";
    public AbsolutePath AllorsDotnetSystemWorkspaceAdaptersJsonSystemTextTests => AllorsDotnetSystemWorkspaceAdapters / "Allors.Workspace.Adapters.Json.SystemText.Tests";
}
