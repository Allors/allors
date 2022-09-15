namespace Generate.Model
{
    using Allors;
    using Allors.Repository;
    using Allors.Repository.Domain;
    using System.Collections.Generic;
    using System;

    public class PropertyModel : RepositoryObjectModel
    {
        public PropertyModel(RepositoryModel repositoryModel, Property property) : base(repositoryModel) => this.Property = property;

        public Property Property { get; }

        protected override RepositoryObject RepositoryObject => this.Property;

        public Dictionary<string, Attribute> AttributeByName => this.Property.AttributeByName;

        public Dictionary<string, Attribute[]> AttributesByName => this.Property.AttributesByName;

        public Domain Domain => this.Property.Domain;

        public string[] WorkspaceNames => this.Property.WorkspaceNames;

        public string Id => this.Property.Id;

        public bool Required => this.Property.Required;

        public bool Unique => this.Property.Unique;

        public XmlDoc XmlDoc => this.Property.XmlDoc;

        public Composite DefiningType => this.Property.DefiningType;

        public ObjectType ObjectType => this.Property.ObjectType;

        public Property DefiningProperty => this.Property.DefiningProperty;

        public Multiplicity Multiplicity => this.Property.Multiplicity;

        public bool IsRoleOne => !(this.Property.Multiplicity is Multiplicity.OneToMany or Multiplicity.ManyToMany);

        public bool IsRoleMany => this.Property.Multiplicity is Multiplicity.OneToMany or Multiplicity.ManyToMany;

        public bool IsAssociationOne => !(this.Property.Multiplicity is Multiplicity.ManyToOne or Multiplicity.ManyToMany);

        public bool IsAssociationMany => this.Property.Multiplicity is Multiplicity.ManyToOne or Multiplicity.ManyToMany;

        public string RoleName => this.Property.RoleName;

        public string RoleSingularName => this.Property.RoleSingularName;

        public string AssignedRoleSingularName => this.Property.AssignedRoleSingularName;

        public string RolePluralName => this.Property.RolePluralName;

        public string AssignedRolePluralName => this.Property.AssignedRolePluralName;

        public string AssociationName => this.Property.AssociationName;
    }
}
