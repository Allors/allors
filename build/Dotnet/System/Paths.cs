using Nuke.Common.IO;

public partial class Paths
{
    public AbsolutePath DotnetSystem => Dotnet / "System";

    public AbsolutePath DotnetSystemSharedTests => DotnetSystem / "Shared.Tests";

    public AbsolutePath DotnetSystemRepositoryModelTests => DotnetSystemRepository / "Allors.Repository.Model.Tests";

    public AbsolutePath DotnetSystemRepository => DotnetSystem / "Repository";
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

    public AbsolutePath DotnetSystemWorkspaceTypescript => DotnetSystem / "Workspace/Typescript";
}
