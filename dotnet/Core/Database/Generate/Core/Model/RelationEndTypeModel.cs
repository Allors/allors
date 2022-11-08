namespace Allors.Meta.Generation.Model;

using Allors.Database.Meta;

public abstract class RelationEndTypeModel
    : IMetaExtensibleModel
{
    protected RelationEndTypeModel(MetaModel metaModel)
    {
        this.MetaModel = metaModel;
    }

    public MetaModel MetaModel { get; }

    // IMetaExtensible
    public IMetaExtensible MetaExtensible => this.RelationEndType;

    public dynamic Extensions => this.MetaExtensible.Attributes;

    // IRelationEndType
    public ObjectTypeModel ObjectType => this.MetaModel.Map(this.RelationEndType.ObjectType);

    public string Name => this.RelationEndType.Name;

    public string SingularName => this.RelationEndType.SingularName;

    public string SingularFullName => this.RelationEndType.SingularFullName;

    public string PluralName => this.RelationEndType.PluralName;

    public string PluralFullName => this.RelationEndType.PluralFullName;

    public bool IsOne => this.RelationEndType.IsOne;

    public bool IsMany => this.RelationEndType.IsMany;

    protected abstract IRelationEndType RelationEndType { get; }

}
