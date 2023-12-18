using Nuke.Common.IO;

public partial class Paths
{
    public AbsolutePath Allors => Root;

    // Artifacts
    public AbsolutePath Artifacts => Root / "artifacts";
    public AbsolutePath ArtifactsCommands => Artifacts / "Commands";
    public AbsolutePath ArtifactsServer => Artifacts / "Server";
}