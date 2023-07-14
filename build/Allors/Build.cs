using Nuke.Common;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tooling.ProcessTasks;
using static Nuke.Common.IO.FileSystemTasks;

public partial class Build
{
    private readonly Paths Paths = new Paths(RootDirectory);

    public Target EnsureDirectories => _ => _
        .Executes(() => EnsureExistingDirectory(Paths.ArtifactsTests));

    public static int Main() => Execute<Build>(x => x.Default);

    private Target Reset => _ => _
        .Executes(KillProcesses);

    static void KillProcesses()
    {
        static void TaskKill(string imageName)
        {
            try
            {
                StartProcess("taskkill", $"/IM {imageName} /F /T /FI \"PID ge 0\"").WaitForExit();
            }
            catch
            {
            }
        }

        TaskKill("node.exe");
        TaskKill("chrome.exe");
        TaskKill("chromedriver.exe");
    }
}
