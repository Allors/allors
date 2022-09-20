namespace Generate.Model;

using System.Linq;
using Allors.Repository;
using Allors.Repository.Domain;

public class ClassModel : CompositeModel
{
    public ClassModel(RepositoryModel repositoryModel, Class @class) : base(repositoryModel) => this.Class = @class;

    public Class Class { get; }

    protected override RepositoryObject RepositoryObject => this.Class;

    public override DataType DataType => this.Class;

    public override ObjectType ObjectType => this.Class;

    public override Composite Composite => this.Class;

    public override InterfaceModel[] Interfaces => this.Class.Interfaces.Select(this.RepositoryModel.Map).ToArray();

    public PropertyModel[] InheritedRequiredProperties => this.InheritedProperties.Where(v => v.Required).ToArray();

    public PropertyModel[] InheritedUniqueProperties => this.InheritedProperties.Where(v => v.Unique).ToArray();
}
