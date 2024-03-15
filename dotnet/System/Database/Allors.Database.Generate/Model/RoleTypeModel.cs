namespace Allors.Meta.Generation.Model;

using System;
using System.Collections.Generic;
using Allors.Database.Meta;

public class RoleTypeModel : RelationEndTypeModel, IMetaIdentifiableObjectModel
{
    public RoleTypeModel(Model model, RoleType roleType)
        : base(model) => this.RoleType = roleType;

    public RoleType RoleType { get; }

    protected override RelationEndType RelationEndType => this.RoleType;

    public IMetaIdentifiableObject MetaObject => this.RoleType;
    
    // IRoleType
    public AssociationTypeModel AssociationType => this.Model.Map(this.RoleType.AssociationType);

    public string FullName => this.RoleType.FullName;

    public bool ExistAssignedSingularName => this.AssignedSingularName != null;

    public string AssignedSingularName => (this.RoleType).AssignedSingularName;

    public bool ExistAssignedPluralName => this.AssignedPluralName != null;

    public string AssignedPluralName => (this.RoleType).AssignedPluralName;

    public int? Size => this.RoleType.Size;

    public int? Precision => this.RoleType.Precision;

    public int? Scale => this.RoleType.Scale;

    public bool IsRequired => this.RoleType.CompositeRoleType.IsRequired;

    public bool IsUnique => this.RoleType.CompositeRoleType.IsUnique;

    public Multiplicity Multiplicity => this.RoleType.Multiplicity;

    public bool IsOneToOne => this.RoleType.Multiplicity == Multiplicity.OneToOne;

    public bool IsOneToMany => this.RoleType.Multiplicity == Multiplicity.OneToMany;

    public bool IsManyToOne => this.RoleType.Multiplicity == Multiplicity.ManyToOne;

    public bool IsManyToMany => this.RoleType.Multiplicity == Multiplicity.ManyToMany;

    public bool IsDerived => this.RoleType.IsDerived;
    
    public string MediaType => this.RoleType.MediaType;
}
