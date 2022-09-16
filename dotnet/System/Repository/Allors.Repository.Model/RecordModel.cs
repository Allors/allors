namespace Generate.Model;

using System.Linq;
using Allors.Repository;
using Allors.Repository.Domain;

public class RecordModel : FieldObjectTypeModel
{
    public RecordModel(RepositoryModel repositoryModel, Record record) : base(repositoryModel) => this.Record = record;

    public Record Record { get; }

    protected override RepositoryObject RepositoryObject => this.Record;

    public override FieldObjectType FieldObjectType => this.Record;

    public string Name => this.Record.Name;

    public XmlDoc XmlDoc => this.Record.XmlDoc;

    public FieldModel[] Fields => this.Record.Fields.Select(this.RepositoryModel.Map).ToArray();
}
