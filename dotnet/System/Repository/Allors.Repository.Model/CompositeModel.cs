namespace Allors.Repository.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using Allors.Repository.Domain;

public abstract class CompositeModel : ObjectTypeModel
{
    private IEnumerable<InterfaceModel> supertypes;

    protected CompositeModel(RepositoryModel repositoryModel)
        : base(repositoryModel) { }

    public abstract Composite Composite { get; }

    public string PluralName => this.Composite.PluralName;

    public string AssignedPluralName => this.Composite.AssignedPluralName;

    public abstract InterfaceModel[] Interfaces { get; }

    public IList<InterfaceModel> ImplementedInterfaces => this.Composite.ImplementedInterfaces.Select(this.RepositoryModel.Map).ToArray();

    public PropertyModel[] Properties => this.Composite.Properties.Select(this.RepositoryModel.Map).ToArray();

    public PropertyModel[] DefinedProperties => this.Properties.Where(v => v.DefiningProperty == null).ToArray();

    public PropertyModel[] InheritedProperties => this.Properties.Where(v => v.DefiningProperty != null).ToArray();

    public PropertyModel[] DefinedReverseProperties => this.Composite.DefinedReverseProperties.Select(this.RepositoryModel.Map).ToArray();

    public PropertyModel[] InheritedReverseProperties =>
        this.Composite.InheritedReverseProperties.Select(this.RepositoryModel.Map).ToArray();

    public MethodModel[] Methods => this.Composite.Methods.Select(this.RepositoryModel.Map).ToArray();

    public MethodModel[] DefinedMethods => this.Methods.Where(v => v.DefiningMethod == null).ToArray();

    public MethodModel[] InheritedMethods => this.Methods.Where(v => v.DefiningMethod != null).ToArray();

    public CompositeModel[] Subtypes => this.Composite.Subtypes.Select(this.RepositoryModel.Map).ToArray();

    public IEnumerable<InterfaceModel> Supertypes
    {
        get
        {
            return this.supertypes ?? this.Interfaces.Union(this.Interfaces.SelectMany(v => v.Supertypes)).ToArray();
        }
    }
}
