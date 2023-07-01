namespace Allors.Meta.Generation.Model;

using Allors.Database.Meta;

public class AssociationTypeModel : RelationEndTypeModel
{
    public AssociationTypeModel(MetaModel metaModel, IAssociationType associationType)
        : base(metaModel) =>
        this.AssociationType = associationType;

    public IAssociationType AssociationType { get; }

    protected override IRelationEndType RelationEndType => this.AssociationType;

    // IAssociationType
    public RelationTypeModel RelationType => this.MetaModel.Map(this.AssociationType.RelationType);

    public RoleTypeModel RoleType => this.MetaModel.Map(this.AssociationType.RoleType);
}
