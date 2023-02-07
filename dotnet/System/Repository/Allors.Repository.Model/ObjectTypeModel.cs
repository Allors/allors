namespace Allors.Repository.Model;

using Allors.Repository.Domain;

public abstract class ObjectTypeModel : RepositoryObjectModel
{
    protected ObjectTypeModel(RepositoryModel repositoryModel) : base(repositoryModel)
    {
    }

    public abstract ObjectType ObjectType { get; }

    public string SingularName => this.ObjectType.SingularName;
    
    public DomainModel Domain => this.RepositoryModel.Map(this.ObjectType.Domain);

    public bool IsInterface => this is InterfaceModel;

    public bool IsClass => this is ClassModel;

    public bool IsComposite => this is CompositeModel;

    public bool IsUnit => this is UnitModel;
}
