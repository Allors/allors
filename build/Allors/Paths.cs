using Nuke.Common.IO;

public partial class Paths
{
    public Paths(AbsolutePath root) => Root = root;

    public AbsolutePath Root { get; }
}
