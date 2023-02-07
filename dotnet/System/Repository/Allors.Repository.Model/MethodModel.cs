namespace Allors.Repository.Model;

using Allors.Repository;
using Allors.Repository.Domain;

public class MethodModel : RepositoryObjectModel
{
    public MethodModel(RepositoryModel repositoryModel, Method method) : base(repositoryModel) => this.Method = method;

    public Method Method { get; }

    protected override RepositoryObject RepositoryObject => this.Method;

    public DomainModel Domain => this.RepositoryModel.Map(this.Method.Domain);

    public string Name => this.Method.Name;

    public MethodModel DefiningMethod => this.RepositoryModel.Map(this.Method.DefiningMethod);

    public CompositeModel DefiningType => this.RepositoryModel.Map(this.Method.DefiningType);
}
