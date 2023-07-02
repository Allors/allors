namespace Workspace.ViewModels.WinForms.Services
{
    using Allors.Workspace;
    using Allors.Workspace.Adapters;
    using Workspace.ViewModels.Services;

    public class DatabaseService : IDatabaseService
    {
        public DatabaseService(Connection connection) => this.Connection = connection;

        public Connection Connection { get; }

        public IWorkspace CreateWorkspace() => this.Connection.CreateWorkspace();
    }
}
