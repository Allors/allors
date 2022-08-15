namespace Allors.Workspace.State
{
    using System.Threading.Tasks;

    public interface IPullRequest
    {
        Task<IPullResponse> ObjectVersionById { get; }
    }
}
