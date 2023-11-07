using Nuke.Common.IO;

public partial class Paths
{
    public AbsolutePath AllorsDotnetBase => AllorsDotnet / "Base";

    public AbsolutePath AllorsDotnetBaseRepository => AllorsDotnetBase / "Repository/Repository.csproj";

    public AbsolutePath AllorsDotnetBaseDatabase => AllorsDotnetBase / "Database";
    public AbsolutePath AllorsDotnetBaseDatabaseMetaGenerated => AllorsDotnetBaseDatabase / "Meta.Domain/Generated";
    public AbsolutePath AllorsDotnetBaseDatabaseMetaConfigurationGenerated => AllorsDotnetBaseDatabase / "Meta.Configuration/Generated";
    public AbsolutePath AllorsDotnetBaseDatabaseGenerate => AllorsDotnetBaseDatabase / "Generate/Generate.csproj";
    public AbsolutePath AllorsDotnetBaseDatabaseCommands => AllorsDotnetBaseDatabase / "Commands";
    public AbsolutePath AllorsDotnetBaseDatabaseServer => AllorsDotnetBaseDatabase / "Server";
    public AbsolutePath AllorsDotnetBaseDatabaseDomainTests => AllorsDotnetBaseDatabase / "Domain.Tests/Domain.Tests.csproj";
 }