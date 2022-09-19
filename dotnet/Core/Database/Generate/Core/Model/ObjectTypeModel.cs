namespace Allors.Meta.Generation.Model;

using System;
using Database.Meta;

public abstract class ObjectTypeModel : FieldObjectTypeModel
{
    protected ObjectTypeModel(MetaModel metaModel)
        : base(metaModel)
    {
    }

    protected abstract IObjectType ObjectType { get; }

    protected override IFieldObjectType FieldObjectType => this.ObjectType;

    // IObjectType
    public bool IsUnit => this.ObjectType.IsUnit;

    public bool IsComposite => this.ObjectType.IsComposite;

    public bool IsInterface => this.ObjectType.IsInterface;

    public bool IsClass => this.ObjectType.IsClass;

    public string SingularName => this.ObjectType.SingularName;

    public string PluralName => this.ObjectType.PluralName;

    public bool ExistAssignedPluralName => ((ObjectType)this.ObjectType).ExistAssignedPluralName;
}
