namespace Allors.Meta.Generation.Model;

using System;
using Database.Meta;

public abstract class DataTypeModel : MetaIdentifiableObjectModel
{
    protected DataTypeModel(MetaModel metaModel)
        : base(metaModel)
    {
    }

    protected abstract IDataType DataType { get; }

    // IMetaIdentifiableObject
    public Guid Id => this.DataType.Id;

    public string Tag => this.DataType.Tag;

    // IDataType
    public string Name => this.DataType.Name;

    public Type ClrType => this.DataType.BoundType;
}
