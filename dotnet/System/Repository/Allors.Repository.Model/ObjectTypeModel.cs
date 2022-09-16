namespace Generate.Model;

using Allors.Repository;
using Allors.Repository.Domain;

public abstract class ObjectTypeModel : FieldObjectTypeModel
{
    protected ObjectTypeModel(RepositoryModel repositoryModel) : base(repositoryModel)
    {
    }

    public abstract ObjectType ObjectType { get; }

    public string Id => (string)((dynamic)this.ObjectType.AttributeByName.Get("Id"))?.Value;

    public string SingularName => this.ObjectType.SingularName;

    public DomainModel Domain => this.RepositoryModel.Map(this.ObjectType.Domain);
}
