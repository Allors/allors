namespace Allors.Repository.Model;

using Allors.Repository;
using Allors.Repository.Domain;

public class UnitModel : ObjectTypeModel
{
    public UnitModel(RepositoryModel repositoryModel, Unit unit)
        : base(repositoryModel) => this.Unit = unit;

    public Unit Unit { get; }

    protected override RepositoryObject RepositoryObject => this.Unit;

    public override DataType DataType => this.Unit;

    public override ObjectType ObjectType => this.Unit;
}
