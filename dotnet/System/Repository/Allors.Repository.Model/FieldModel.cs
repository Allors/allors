namespace Generate.Model;

using Allors.Repository;
using Allors.Repository.Domain;

public class FieldModel : RepositoryObjectModel
{
    public FieldModel(RepositoryModel repositoryModel, Field field) : base(repositoryModel) => this.Field = field;

    public Field Field { get; }

    protected override RepositoryObject RepositoryObject => this.Field;

    public string Name => this.Field.Name;

    public RecordModel Record => this.RepositoryModel.Map(this.Field.Record);

    public XmlDoc XmlDoc => this.Field.XmlDoc;

    public FieldObjectTypeModel Type => this.RepositoryModel.Map(this.Field.Type);

    public bool IsOne => !this.Field.IsMany;

    public bool IsMany => this.Field.IsMany;
}
