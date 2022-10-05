namespace Allors.Meta.Generation.Model;

using Database.Meta;

public abstract class PropertyTypeModel
    : IMetaExtensibleModel
{
    protected PropertyTypeModel(MetaModel metaModel)
    {
        this.MetaModel = metaModel;
    }

    public MetaModel MetaModel { get; }

    // IMetaExtensible
    public IMetaExtensible MetaExtensible => this.PropertyType;

    public dynamic Extensions => this.MetaExtensible.Attributes;

    // IPropertyType
    public ObjectTypeModel ObjectType => this.MetaModel.Map(this.PropertyType.ObjectType);

    public string Name => this.PropertyType.Name;

    public string SingularName => this.PropertyType.SingularName;

    public string SingularFullName => this.PropertyType.SingularFullName;

    public string PluralName => this.PropertyType.PluralName;

    public string PluralFullName => this.PropertyType.PluralFullName;

    public bool IsOne => this.PropertyType.IsOne;

    public bool IsMany => this.PropertyType.IsMany;

    protected abstract IPropertyType PropertyType { get; }

}
