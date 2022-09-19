namespace Allors.Meta.Generation.Model;

using System;
using Database.Meta;

public abstract class FieldObjectTypeModel : MetaObjectModel, IMetaIdentifiableObjectModel
{
    protected FieldObjectTypeModel(MetaModel metaModel)
        : base(metaModel)
    {
    }

    protected abstract IFieldObjectType FieldObjectType { get; }

    // IMetaIdentifiableObject
    public Guid Id => this.FieldObjectType.Id;

    public string Tag => this.FieldObjectType.Tag;

    // IFieldObjectType
    public string Name => this.FieldObjectType.Name;

    public Type ClrType => this.FieldObjectType.ClrType;
}
