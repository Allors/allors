namespace Avalonia.Views;

using Allors.Workspace.Adapters.Json.SystemText;
using Splat;
using Allors.Workspace;

public class WorkspaceFactory : IWorkspaceFactory
{
    public IWorkspace CreateWorkspace()
    {
        var connection = Locator.Current.GetService<Connection>();
        return connection.CreateWorkspace();
    }
}
