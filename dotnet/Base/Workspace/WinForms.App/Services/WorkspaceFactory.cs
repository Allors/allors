namespace Workspace.WinForms.App.Services
{
    using Allors.Workspace;
    using Allors.Workspace.Adapters;

    public class WorkspaceFactory : IWorkspaceFactory
    {
        public WorkspaceFactory(Connection connection) => this.Connection = connection;

        public Connection Connection { get; }

        public IWorkspace CreateWorkspace() => this.Connection.CreateWorkspace();
    }
}
