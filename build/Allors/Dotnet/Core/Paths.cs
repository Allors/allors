using Nuke.Common.IO;

public partial class Paths
{
    public AbsolutePath AllorsDotnetCore => AllorsDotnet / "Core";
    
    public AbsolutePath AllorsDotnetCoreRepository => AllorsDotnetCore / "Repository/Repository.csproj";
    

    public AbsolutePath AllorsDotnetCoreDatabase => AllorsDotnetCore / "Database";
    public AbsolutePath AllorsDotnetCoreDatabaseMetaGenerated => AllorsDotnetCoreDatabase / "Meta.Domain/Generated";
    public AbsolutePath AllorsDotnetCoreDatabaseMetaConfigurationGenerated => AllorsDotnetCoreDatabase / "Meta.Configuration/Generated";
    public AbsolutePath AllorsDotnetCoreDatabaseGenerate => AllorsDotnetCoreDatabase / "Generate/Generate.csproj";
    public AbsolutePath AllorsDotnetCoreDatabaseMerge => AllorsDotnetCoreDatabase / "Merge/Merge.csproj";
    public AbsolutePath AllorsDotnetCoreDatabaseServer => AllorsDotnetCoreDatabase / "Server";
    public AbsolutePath AllorsDotnetCoreDatabaseCommands => AllorsDotnetCoreDatabase / "Commands";
    public AbsolutePath AllorsDotnetCoreDatabaseMetaTests => AllorsDotnetCoreDatabase / "Meta.Tests/Meta.Tests.csproj";
    public AbsolutePath AllorsDotnetCoreDatabaseDomainTests => AllorsDotnetCoreDatabase / "Domain.Tests/Domain.Tests.csproj";
    public AbsolutePath AllorsDotnetCoreDatabaseServerDirectTests => AllorsDotnetCoreDatabase / "Server.Direct.Tests/Server.Direct.Tests.csproj";
    public AbsolutePath AllorsDotnetCoreDatabaseServerJsonTests => AllorsDotnetCoreDatabase / "Server.Json.Tests/Server.Json.Tests.csproj";
    public AbsolutePath AllorsDotnetCoreDatabaseResources => AllorsDotnetCoreDatabase / "Resources";
    public AbsolutePath AllorsDotnetCoreDatabaseResourcesCore => AllorsDotnetCoreDatabaseResources / "Core";
    public AbsolutePath AllorsDotnetCoreDatabaseResourcesCustom => AllorsDotnetCoreDatabaseResources / "Custom";

    
    public AbsolutePath AllorsDotnetCoreWorkspace => AllorsDotnetCore / "Workspace";
    public AbsolutePath AllorsDotnetCoreWorkspaceMetaTests => AllorsDotnetCoreWorkspace / "Meta.Tests";
}
