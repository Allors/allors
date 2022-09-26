namespace Generate.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using Allors.Repository;
using Allors.Repository.Domain;

public class InterfaceModel : CompositeModel
{
    public InterfaceModel(RepositoryModel repositoryModel, Interface @interface) : base(repositoryModel) => this.Interface = @interface;

    public Interface Interface { get; }

    protected override RepositoryObject RepositoryObject => this.Interface;

    public override DataType DataType => this.Interface;

    public override ObjectType ObjectType => this.Interface;

    public override Composite Composite => this.Interface;

    public override InterfaceModel[] Interfaces => this.Interface.Interfaces.Select(this.RepositoryModel.Map).ToArray();

    public Dictionary<string, PropertyModel> InheritedPropertyByRoleName =>
        this.Interface.InheritedPropertyByRoleName.ToDictionary(v => v.Key, v => this.RepositoryModel.Map(v.Value));

    public PropertyModel[] InheritedProperties => this.InheritedPropertyByRoleName.Values.ToArray();

  
}
