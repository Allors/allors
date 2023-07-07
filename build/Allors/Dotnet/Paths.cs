using Nuke.Common.IO;

public partial class Paths
{
    public AbsolutePath AllorsDotnet => Allors / "dotnet";

    // Core
    public AbsolutePath ArtifactsCoreCommands => Artifacts / "Core/Commands";
    public AbsolutePath ArtifactsCoreServer => Artifacts / "Core/Server";

    // Base
    public AbsolutePath ArtifactsBaseCommands => Artifacts / "Base/Commands";
    public AbsolutePath ArtifactsBaseServer => Artifacts / "Base/Server";
}
