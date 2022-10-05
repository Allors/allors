namespace Allors.Repository.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using Allors.Repository;

public abstract class RepositoryObjectModel : IComparable<RepositoryObjectModel>
{
    protected RepositoryObjectModel(RepositoryModel repositoryModel) => this.RepositoryModel = repositoryModel;

    public RepositoryModel RepositoryModel { get; }

    protected abstract RepositoryObject RepositoryObject { get; }

    public Dictionary<string, Attribute> AttributeByName => this.RepositoryObject.AttributeByName;

    public Dictionary<string, Attribute[]> AttributesByName => this.RepositoryObject.AttributesByName;

    public string Id => (string)((dynamic)this.AttributeByName.Get("Id"))?.Value;

    public int CompareTo(RepositoryObjectModel other)
    {
        return string.CompareOrdinal(this.Id, other.Id);
    }

    public override string ToString() => this.RepositoryObject.ToString();

    public Attribute[] ExtensionAttributes => this.AttributeByName.Values.Where(v => v.GetType().GetInterfaces().Any(v => "IExtensionAttribute" == v.Name)).ToArray();

    public Attribute[] RelationTypeExtensionAttributes => this.ExtensionAttributes.Where(v => ((dynamic)v).ForRelationType).ToArray();

    public Attribute[] AssociationTypeExtensionAttributes => this.ExtensionAttributes.Where(v => ((dynamic)v).ForAssociationType).ToArray();

    public Attribute[] RoleTypeExtensionAttributes => this.ExtensionAttributes.Where(v => ((dynamic)v).ForRoleType).ToArray();

    public Attribute[] CompositeRoleTypeExtensionAttributes => this.ExtensionAttributes.Where(v => ((dynamic)v).ForCompositeRoleType).ToArray();

    public Attribute[] MethodTypeExtensionAttributes => this.ExtensionAttributes.Where(v => ((dynamic)v).ForMethodType).ToArray();

    public Attribute[] CompositeMethodTypeExtensionAttributes => this.ExtensionAttributes.Where(v => ((dynamic)v).ForCompositeMethodType).ToArray();
}
