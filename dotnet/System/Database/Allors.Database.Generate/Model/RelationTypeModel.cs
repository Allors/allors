namespace Allors.Meta.Generation.Model;

using System.Collections.Generic;
using Allors.Database.Meta.Extensions;
using Allors.Database.Meta;

public class RelationTypeModel : MetaIdentifiableObjectModel
{
    public RelationTypeModel(Model model, RelationType relationType)
        : base(model)
    {
        this.RelationType = relationType;
        this.AssociationType = new AssociationTypeModel(model, relationType.AssociationType);
        this.RoleType = new RoleTypeModel(model, relationType.RoleType);
    }

    public RelationType RelationType { get; }

    public override IMetaIdentifiableObject MetaObject => this.RelationType;

    // IRelationType
    public AssociationTypeModel AssociationType { get; }

    public RoleTypeModel RoleType { get; }

    public string Name => this.RelationType.Name;

    public Multiplicity Multiplicity => this.RelationType.Multiplicity;

    public bool IsOneToOne => this.RelationType.Multiplicity == Multiplicity.OneToOne;

    public bool IsOneToMany => this.RelationType.Multiplicity == Multiplicity.OneToMany;

    public bool IsManyToOne => this.RelationType.Multiplicity == Multiplicity.ManyToOne;

    public bool IsManyToMany => this.RelationType.Multiplicity == Multiplicity.ManyToMany;

    public bool IsDerived => this.RelationType.IsDerived;

    public IEnumerable<string> WorkspaceNames => this.RelationType.WorkspaceNames;

    public string MediaType => this.RelationType.MediaType();
}
