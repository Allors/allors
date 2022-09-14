namespace Generate.Model
{
    using Allors.Repository;
    using Allors.Repository.Domain;

    public class MethodModel : RepositoryObjectModel
    {
        public MethodModel(RepositoryModel repositoryModel, Method method) : base(repositoryModel) => this.Method = method;

        public Method Method { get; }

        protected override RepositoryObject RepositoryObject => this.Method;
    }
}
