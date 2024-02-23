using Nuke.Common.IO;

public partial class Paths
{
    public AbsolutePath AllorsDotnetBase => AllorsDotnet / "Base";

    public AbsolutePath AllorsDotnetBaseRepository => AllorsDotnetBase / "Repository/Repository.csproj";

    public AbsolutePath AllorsDotnetBaseDatabase => AllorsDotnetBase / "Database";
    public AbsolutePath AllorsDotnetBaseDatabaseMetaGenerated => AllorsDotnetBaseDatabase / "Meta/Generated";
    public AbsolutePath AllorsDotnetBaseDatabaseMetaTests => AllorsDotnetBaseDatabase / "Meta.Tests/Meta.Tests.csproj";
    public AbsolutePath AllorsDotnetBaseDatabaseGenerate => AllorsDotnetBaseDatabase / "Generate/Generate.csproj";
    public AbsolutePath AllorsDotnetBaseDatabaseCommands => AllorsDotnetBaseDatabase / "Commands";
    public AbsolutePath AllorsDotnetBaseDatabaseServer => AllorsDotnetBaseDatabase / "Server";
    public AbsolutePath AllorsDotnetBaseDatabaseServerDirectTests => AllorsDotnetBaseDatabase / "Server.Direct.Tests/Server.Direct.Tests.csproj";
    public AbsolutePath AllorsDotnetBaseDatabaseServerJsonTests => AllorsDotnetBaseDatabase / "Server.Json.Tests/Server.Json.Tests.csproj";
    public AbsolutePath AllorsDotnetBaseDatabaseDomainTests => AllorsDotnetBaseDatabase / "Domain.Tests/Domain.Tests.csproj";

    public AbsolutePath AllorsDotnetBaseWorkspace => AllorsDotnetBase / "Workspace";
    public AbsolutePath AllorsDotnetBaseWorkspaceMetaTests => AllorsDotnetBaseWorkspace / "Meta.Tests";
    public AbsolutePath AllorsDotnetBaseWorkspaceWinformsViewModelsTests => AllorsDotnetBaseWorkspace / "WinForms.ViewModels.Tests";
}