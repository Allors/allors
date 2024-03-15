namespace Allors.Repository.Model;

using System;
using Allors;
using Allors.Repository;
using Allors.Repository.Domain;

public class PropertyModel : RepositoryObjectModel
{
    public PropertyModel(RepositoryModel repositoryModel, Property property) : base(repositoryModel) => this.Property = property;

    public Property Property { get; }

    protected override RepositoryObject RepositoryObject => this.Property;

    public Domain Domain => this.Property.Domain;

    public bool Required => (bool)(((dynamic)this.Property.AttributeByName.Get("Required"))?.Value ?? false);

    public bool Unique => (bool)(((dynamic)this.Property.AttributeByName.Get("Unique"))?.Value ?? false);

    public CompositeModel DefiningType => this.RepositoryModel.Map(this.Property.DefiningType);

    public ObjectTypeModel ObjectType => this.RepositoryModel.Map(this.Property.ObjectType);

    public PropertyModel DefiningProperty => this.RepositoryModel.Map(this.Property.DefiningProperty);

    public Multiplicity Multiplicity => this.Property.Multiplicity;

    public Multiplicity? AssignedMultiplicity =>
        this.ObjectType.IsComposite && this.Multiplicity != Multiplicity.ManyToOne ? this.Multiplicity : null;

    public string RoleName => this.Property.RoleName;

    public string RoleSingularName => this.Property.RoleSingularName;

    public string AssignedRoleSingularName => this.Property.AssignedRoleSingularName;

    public string RolePluralName => this.Property.RolePluralName;

    public string AssignedRolePluralName => this.Property.AssignedRolePluralName;

    public string AssociationName => this.Property.AssociationName;

    public Guid AssociationId => Guid.NewGuid();
}
