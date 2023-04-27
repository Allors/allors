using Nuke.Common.IO;

public partial class Paths
{
    public AbsolutePath DotnetCore => Dotnet / "Core";
    public AbsolutePath DotnetCoreRepository => DotnetCore / "Repository/Repository.csproj";

    public AbsolutePath DotnetCoreDatabase => DotnetCore / "Database";
    public AbsolutePath DotnetCoreDatabaseMetaGenerated => DotnetCoreDatabase / "Meta/Generated";
    public AbsolutePath DotnetCoreDatabaseMetaConfigurationGenerated => DotnetCoreDatabase / "Meta.Configuration/Generated";
    public AbsolutePath DotnetCoreDatabaseGenerate => DotnetCoreDatabase / "Generate/Generate.csproj";
    public AbsolutePath DotnetCoreDatabaseMerge => DotnetCoreDatabase / "Merge/Merge.csproj";
    public AbsolutePath DotnetCoreDatabaseServer => DotnetCoreDatabase / "Server";
    public AbsolutePath DotnetCoreDatabaseCommands => DotnetCoreDatabase / "Commands";
    public AbsolutePath DotnetCoreDatabaseMetaTests => DotnetCoreDatabase / "Meta.Tests/Meta.Tests.csproj";
    public AbsolutePath DotnetCoreDatabaseDomainTests => DotnetCoreDatabase / "Domain.Tests/Domain.Tests.csproj";

    public AbsolutePath DotnetCoreDatabaseServerLocalTests => DotnetCoreDatabase / "Server.Local.Tests/Server.Local.Tests.csproj";

    public AbsolutePath DotnetCoreDatabaseServerRemoteTests => DotnetCoreDatabase / "Server.Remote.Tests/Server.Remote.Tests.csproj";

    public AbsolutePath DotnetCoreDatabaseResources => DotnetCoreDatabase / "Resources";
    public AbsolutePath DotnetCoreDatabaseResourcesCore => DotnetCoreDatabaseResources / "Core";
    public AbsolutePath DotnetCoreDatabaseResourcesCustom => DotnetCoreDatabaseResources / "Custom";

    public AbsolutePath DotnetCoreWorkspace => DotnetCore / "Workspace";
    public AbsolutePath DotnetCoreWorkspaceAdaptersDirectTests => DotnetCoreWorkspace / "Tests.Direct";
    public AbsolutePath DotnetCoreWorkspaceAdaptersJsonNewtonsoftTests => DotnetCoreWorkspace / "Tests.Json.Newtonsoft";
    public AbsolutePath DotnetCoreWorkspaceAdaptersJsonSystemTextTests => DotnetCoreWorkspace / "Tests.Json.SystemText";
}
