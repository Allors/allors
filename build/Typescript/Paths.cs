using Nuke.Common.IO;

public partial class Paths
{
    public AbsolutePath Typescript => Root / "typescript";

    public AbsolutePath TypescriptModulesLibs => Typescript / "libs";
}
