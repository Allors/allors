namespace Allors.Meta.Generation.Model;

using Database.Meta;

public abstract class ObjectTypeModel : DataTypeModel
{
    protected ObjectTypeModel(MetaModel metaModel)
        : base(metaModel)
    {
    }

    protected abstract IObjectType ObjectType { get; }

    protected override IDataType DataType => this.ObjectType;

    // IObjectType
    public bool IsUnit => this.ObjectType.IsUnit;

    public bool IsComposite => this.ObjectType.IsComposite;

    public bool IsInterface => this.ObjectType.IsInterface;

    public bool IsClass => this.ObjectType.IsClass;

    public string SingularName => this.ObjectType.SingularName;

    public string PluralName => this.ObjectType.PluralName;

    public bool ExistAssignedPluralName => ((ObjectType)this.ObjectType).ExistAssignedPluralName;
}
