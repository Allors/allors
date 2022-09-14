namespace Generate.Model
{
    using Allors.Repository;

    public abstract class RepositoryObjectModel
    {
        protected RepositoryObjectModel(RepositoryModel repositoryModel) => this.RepositoryModel = repositoryModel;

        public RepositoryModel RepositoryModel { get; }

        protected abstract RepositoryObject RepositoryObject { get; }

        public override string ToString() => this.RepositoryObject.ToString();
    }
}
