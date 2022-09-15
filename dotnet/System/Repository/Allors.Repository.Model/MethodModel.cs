namespace Generate.Model
{
    using Allors.Repository;
    using Allors.Repository.Domain;

    public class MethodModel : RepositoryObjectModel
    {
        public MethodModel(RepositoryModel repositoryModel, Method method) : base(repositoryModel) => this.Method = method;

        public Method Method { get; }

        protected override RepositoryObject RepositoryObject => this.Method;

        public DomainModel Domain => this.RepositoryModel.Map(this.Method.Domain);

        public string[] WorkspaceNames => this.Method.WorkspaceNames;

        public string Name => this.Method.Name;

        public string Id => this.Method.Id;

        public XmlDoc XmlDoc => this.Method.XmlDoc;

        public MethodModel DefiningMethod => this.RepositoryModel.Map(this.Method.DefiningMethod);

        public CompositeModel DefiningType => this.RepositoryModel.Map(this.Method.DefiningType);

        public RecordModel Input => this.RepositoryModel.Map(this.Method.Input);

        public RecordModel Output => this.RepositoryModel.Map(this.Method.Output);
    }
}
