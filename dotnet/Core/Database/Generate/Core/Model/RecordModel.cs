namespace Allors.Meta.Generation.Model;

using System;
using Database.Meta;

public class RecordModel : FieldObjectTypeModel
{
    public RecordModel(MetaModel metaModel, Record record)
        : base(metaModel) => this.Record = record;

    public Record Record { get; }

    protected override IMetaObject MetaObject => this.Record;

    protected override IFieldObjectType FieldObjectType => this.Record;
}
