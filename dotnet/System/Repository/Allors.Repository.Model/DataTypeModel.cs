namespace Generate.Model;

using Allors.Repository;

public abstract class DataTypeModel : RepositoryObjectModel
{
    protected DataTypeModel(RepositoryModel repositoryModel) : base(repositoryModel)
    {
    }

    public abstract DataType DataType { get; }

    public bool IsInterface => this is InterfaceModel;

    public bool IsClass => this is ClassModel;

    public bool IsComposite => this is CompositeModel;

    public bool IsUnit => this is UnitModel;

    public bool IsRecord => this is RecordModel;
}
