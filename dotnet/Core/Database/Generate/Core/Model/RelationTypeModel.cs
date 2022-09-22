namespace Allors.Meta.Generation.Model;

using System;
using System.Collections.Generic;
using Database.Meta;

public class RelationTypeModel : MetaIdentifiableObjectModel
{
    public RelationTypeModel(MetaModel metaModel, IRelationType relationType)
        : base(metaModel)
    {
        this.RelationType = relationType;
        this.AssociationType = new AssociationTypeModel(metaModel, relationType.AssociationType);
        this.RoleType = new RoleTypeModel(metaModel, relationType.RoleType);
    }

    public IRelationType RelationType { get; }

    public override IMetaIdentifiableObject MetaObject => this.RelationType;

    // IRelationType
    public AssociationTypeModel AssociationType { get; }

    public RoleTypeModel RoleType { get; }

    public Multiplicity Multiplicity => this.RelationType.Multiplicity;

    public bool IsOneToOne => this.RelationType.Multiplicity == Multiplicity.OneToOne;

    public bool IsOneToMany => this.RelationType.Multiplicity == Multiplicity.OneToMany;

    public bool IsManyToOne => this.RelationType.Multiplicity == Multiplicity.ManyToOne;

    public bool IsManyToMany => this.RelationType.Multiplicity == Multiplicity.ManyToMany;

    public bool IsIndexed => this.RelationType.IsIndexed;

    public bool IsDerived => this.RelationType.IsDerived;

    public IEnumerable<string> WorkspaceNames => this.RelationType.WorkspaceNames;

    public string Name => ((RelationType)this.RelationType).Name;

    // IMetaIdentifiableObject
    public Guid Id => this.RelationType.Id;

    public string Tag => this.RelationType.Tag;
}
