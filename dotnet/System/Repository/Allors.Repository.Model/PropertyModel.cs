namespace Generate.Model
{
    using Allors;
    using Allors.Repository;
    using Allors.Repository.Domain;
    using Allors.Text;
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

        public ObjectType Type => this.Property.Type;

        public Property DefiningProperty => this.Property.DefiningProperty;

        public Multiplicity Multiplicity => this.Property.Multiplicity;

        public bool IsRoleOne => this.Property.IsRoleOne;

        public bool IsRoleMany => this.Property.IsRoleMany;

        public bool IsAssociationOne => this.Property.IsAssociationOne;

        public bool IsAssociationMany => this.Property.IsAssociationMany;

        public string RoleName => this.Property.RoleName;

        public string RoleSingularName => this.Property.RoleSingularName;

        public string AssignedRoleSingularName => this.Property.AssignedRoleSingularName;

        public string RolePluralName => this.Property.RolePluralName;

        public string AssignedRolePluralName => this.Property.AssignedRolePluralName;

        public string AssociationName => this.Property.AssociationName;
    }
}
