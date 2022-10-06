namespace Allors.Meta.Generation.Model;

using Allors.Database.Meta;

public class FieldTypeModel : IMetaExtensibleModel
{
    public FieldTypeModel(MetaModel metaModel, IFieldType fieldType)
    {
        this.MetaModel = metaModel;
        this.FieldType = fieldType;
    }

    public MetaModel MetaModel { get; }

    public IFieldType FieldType { get; }

    // IMetaExtensible
    public IMetaExtensible MetaExtensible => this.FieldType;

    public dynamic Extensions => this.MetaExtensible.Attributes;

    // IFieldType
    public RecordModel Record => this.MetaModel.Map(this.FieldType.Record);

    public DataTypeModel DataType => this.MetaModel.Map(this.FieldType.DataType);

    public string Name => this.FieldType.Name;

    public bool IsOne => this.FieldType.IsOne;

    public bool IsMany => this.FieldType.IsMany;
}
