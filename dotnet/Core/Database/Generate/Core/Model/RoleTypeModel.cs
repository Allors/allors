namespace Allors.Meta.Generation.Model;

using Allors.Database.Meta.Extensions;
using Allors.Database.Meta;

public class RoleTypeModel : RelationEndTypeModel
{
    public RoleTypeModel(MetaModel metaModel, IRoleType roleType)
        : base(metaModel) => this.RoleType = roleType;

    public IRoleType RoleType { get; }

    protected override IRelationEndType RelationEndType => this.RoleType;

    // IRoleType
    public AssociationTypeModel AssociationType => this.MetaModel.Map(this.RoleType.AssociationType);

    public RelationTypeModel RelationType => this.MetaModel.Map(this.RoleType.RelationType);

    public string FullName => this.RoleType.FullName;

    public bool ExistAssignedSingularName => ((RoleType)this.RoleType).ExistAssignedSingularName;

    public string AssignedSingularName => ((RoleType)this.RoleType).AssignedSingularName;

    public bool ExistAssignedPluralName => ((RoleType)this.RoleType).ExistAssignedPluralName;

    public string AssignedPluralName => ((RoleType)this.RoleType).AssignedPluralName;

    public int? Size => this.RoleType.Size;

    public int? Precision => this.RoleType.Precision;

    public int? Scale => this.RoleType.Scale;

    public bool IsRequired => this.RoleType.CompositeRoleType.IsRequired();

    public bool IsUnique => this.RoleType.CompositeRoleType.IsUnique();
}
