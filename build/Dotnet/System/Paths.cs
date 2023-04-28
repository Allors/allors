using Nuke.Common.IO;

public partial class Paths
{
    public AbsolutePath DotnetSystem => Dotnet / "System";

    public AbsolutePath DotnetSystemSharedTests => DotnetSystem / "Shared.Tests";

    public AbsolutePath DotnetSystemRepository => DotnetSystem / "Repository";
    public AbsolutePath DotnetSystemRepositoryModelTests => DotnetSystemRepository / "Allors.Repository.Model.Tests";
    public AbsolutePath DotnetSystemRepositoryTemplates => DotnetSystemRepository / "Templates";
    public AbsolutePath DotnetSystemRepositoryTemplatesMetaCs => DotnetSystemRepositoryTemplates / "meta.cs.stg";
    public AbsolutePath DotnetSystemRepositoryTemplatesMetaConfigurationCs => DotnetSystemRepositoryTemplates / "meta.configuration.cs.stg";
    public AbsolutePath DotnetSystemRepositoryGenerate => DotnetSystemRepository / "Generate/Generate.csproj";

    public AbsolutePath DotnetSystemDatabase => DotnetSystem / "Database";
    public AbsolutePath DotnetSystemAdapters => DotnetSystemDatabase / "Adapters";
    public AbsolutePath DotnetSystemAdaptersRepository => DotnetSystemAdapters / "Repository/Repository.csproj";
    public AbsolutePath DotnetSystemAdaptersMetaGenerated => DotnetSystemAdapters / "Meta/Generated";
    public AbsolutePath DotnetSystemAdaptersMetaConfigurationGenerated => DotnetSystemAdapters / "Meta.Configuration/Generated";
    public AbsolutePath DotnetSystemAdaptersGenerate => DotnetSystemAdapters / "Generate/Generate.csproj";
    public AbsolutePath DotnetSystemAdaptersStaticTests => DotnetSystemAdapters / "Tests.Static/Tests.Static.csproj";

    public AbsolutePath DotnetSystemWorkspace => DotnetSystem / "Workspace";
    public AbsolutePath DotnetSystemWorkspaceAdapters => DotnetSystemWorkspace / "Adapters";
    public AbsolutePath DotnetSystemWorkspaceAdaptersDirectTests => DotnetSystemWorkspaceAdapters / "Allors.Workspace.Adapters.Direct.Tests";
    public AbsolutePath DotnetSystemWorkspaceAdaptersJsonNewtonsoftTests => DotnetSystemWorkspaceAdapters / "Allors.Workspace.Adapters.Json.Newtonsoft.Tests";
    public AbsolutePath DotnetSystemWorkspaceAdaptersJsonSystemTextTests => DotnetSystemWorkspaceAdapters / "Allors.Workspace.Adapters.Json.SystemText.Tests";
    public AbsolutePath DotnetSystemWorkspaceTypescript => DotnetSystemWorkspace / "Typescript";



}
