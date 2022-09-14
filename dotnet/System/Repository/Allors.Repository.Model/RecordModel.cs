namespace Generate.Model
{
    using Allors.Repository;
    using Allors.Repository.Domain;
    using System.Collections.Generic;
    using System.Linq;

    public class RecordModel : RepositoryObjectModel
    {
        public RecordModel(RepositoryModel repositoryModel, Record record) : base(repositoryModel) => this.Record = record;

        public Record Record { get; }

        protected override RepositoryObject RepositoryObject => this.Record;

        public string Name => this.Record.Name;

        public XmlDoc XmlDoc => this.Record.XmlDoc;

        public Dictionary<string, FieldModel> FieldByName => this.Record.FieldByName.ToDictionary(v => v.Key, v => this.RepositoryModel.Map(v.Value));

        public FieldModel[] Fields => this.FieldByName.Values.ToArray();
    }
}
