namespace Generate.Model
{
    using Allors.Repository;
    using Allors.Repository.Domain;
    using Allors.Text;
    using System.Collections.Generic;
    using System;
    using System.Linq;

    public abstract class CompositeModel : ObjectTypeModel
    {
        protected CompositeModel(RepositoryModel repositoryModel) : base(repositoryModel) { }

        public abstract Composite Composite { get; }

        public Dictionary<string, Attribute> AttributeByName => this.Composite.AttributeByName;

        public Dictionary<string, Attribute[]> AttributesByName => this.Composite.AttributesByName;

        public XmlDoc XmlDoc => this.Composite.XmlDoc;

        public string PluralName => this.Composite.PluralName;

        public string AssignedPluralName => this.Composite.AssignedPluralName;

        public abstract InterfaceModel[] Interfaces { get; }

        public IList<InterfaceModel> ImplementedInterfaces => this.Composite.ImplementedInterfaces.Select(this.RepositoryModel.Map).ToArray();

        public Dictionary<string, PropertyModel> PropertyByRoleName => this.Composite.PropertyByRoleName.ToDictionary(v => v.Key, v => this.RepositoryModel.Map(v.Value));

        public Dictionary<string, PropertyModel> DefinedReversePropertyByAssociationName => this.Composite.DefinedReversePropertyByAssociationName.ToDictionary(v => v.Key, v => this.RepositoryModel.Map(v.Value));

        public Dictionary<string, PropertyModel> InheritedReversePropertyByAssociationName => this.Composite.InheritedReversePropertyByAssociationName.ToDictionary(v => v.Key, v => this.RepositoryModel.Map(v.Value));

        public Dictionary<string, MethodModel> MethodByName => this.Composite.MethodByName.ToDictionary(v => v.Key, v => this.RepositoryModel.Map(v.Value));

        public PropertyModel[] Properties => this.PropertyByRoleName.Values.ToArray();

        public PropertyModel[] DefinedProperties => this.PropertyByRoleName.Values.Where(v => v.DefiningProperty == null).ToArray();

        public PropertyModel[] InheritedProperties => this.PropertyByRoleName.Values.Where(v => v.DefiningProperty != null).ToArray();

        public PropertyModel[] DefinedReverseProperties => this.DefinedReversePropertyByAssociationName.Values.ToArray();

        public PropertyModel[] InheritedReverseProperties => this.InheritedReversePropertyByAssociationName.Values.ToArray();

        public MethodModel[] Methods => this.MethodByName.Values.ToArray();

        public MethodModel[] DefinedMethods => this.MethodByName.Values.Where(v => v.DefiningMethod == null).ToArray();

        public MethodModel[] InheritedMethods => this.MethodByName.Values.Where(v => v.DefiningMethod != null).ToArray();

        public CompositeModel[] Subtypes => this.Composite.Subtypes.Select(this.RepositoryModel.Map).ToArray();

    }
}
