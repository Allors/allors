namespace Allors.Meta.Generation.Model;

using System.Collections.Generic;
using System.Linq;
using Allors.Database.Meta;

public class RecordModel : DataTypeModel
{
    public RecordModel(MetaModel metaModel, Record record)
        : base(metaModel) => this.Record = record;

    public Record Record { get; }

    public override IMetaIdentifiableObject MetaObject => this.Record;

    protected override IDataType DataType => this.Record;

    public IEnumerable<FieldTypeModel> FieldTypes => this.Record.FieldTypes.Select(this.MetaModel.Map);
}
