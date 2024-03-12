namespace Allors.Meta.Generation.Model;

using Allors.Database.Meta;

public class AssociationTypeModel : RelationEndTypeModel
{
    public AssociationTypeModel(Model model, AssociationType associationType)
        : base(model) =>
        this.AssociationType = associationType;

    public AssociationType AssociationType { get; }

    protected override RelationEndType RelationEndType => this.AssociationType;

    // IAssociationType
    public RoleTypeModel RoleType => this.Model.Map(this.AssociationType.RoleType);
}
