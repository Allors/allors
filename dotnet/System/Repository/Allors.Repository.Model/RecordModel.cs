namespace Generate.Model;

using System;
using System.Linq;
using Allors.Repository;
using Allors.Repository.Domain;

public class RecordModel : DataTypeModel
{
    public RecordModel(RepositoryModel repositoryModel, Record record) : base(repositoryModel) => this.Record = record;

    public Record Record { get; }

    protected override RepositoryObject RepositoryObject => this.Record;

    public override DataType DataType => this.Record;

    public string Name => this.Record.Name;

    public FieldModel[] Fields => this.Record.Fields.Select(this.RepositoryModel.Map).ToArray();
}
