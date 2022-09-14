namespace Generate.Model
{
    using Allors.Repository;
    using Allors.Repository.Domain;

    public class RecordModel : RepositoryObjectModel
    {
        public RecordModel(RepositoryModel repositoryModel, Record record) : base(repositoryModel) => this.Record = record;

        public Record Record { get; }

        protected override RepositoryObject RepositoryObject => this.Record;
    }
}
