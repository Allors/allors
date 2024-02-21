namespace Allors.Meta.Generation.Model;

using Allors.Database.Meta.Extensions;
using Allors.Database.Meta;

public class RoleTypeModel : RelationEndTypeModel
{
    public RoleTypeModel(Model model, RoleType roleType)
        : base(model) => this.RoleType = roleType;

    public RoleType RoleType { get; }

    protected override RelationEndType RelationEndType => this.RoleType;

    // IRoleType
    public AssociationTypeModel AssociationType => this.Model.Map(this.RoleType.AssociationType);

    public RelationTypeModel RelationType => this.Model.Map(this.RoleType.RelationType);

    public string FullName => this.RoleType.FullName;

    public bool ExistAssignedSingularName => this.AssignedSingularName != null;

    public string AssignedSingularName => (this.RoleType).AssignedSingularName;

    public bool ExistAssignedPluralName => this.AssignedPluralName != null;

    public string AssignedPluralName => (this.RoleType).AssignedPluralName;

    public int? Size => this.RoleType.Size;

    public int? Precision => this.RoleType.Precision;

    public int? Scale => this.RoleType.Scale;

    public bool IsRequired => this.RoleType.CompositeRoleType.IsRequired();

    public bool IsUnique => this.RoleType.CompositeRoleType.IsUnique();
}
