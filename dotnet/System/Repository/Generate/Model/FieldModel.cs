namespace Generate.Model
{
    using Allors.Repository;
    using Allors.Repository.Domain;

    public class FieldModel : RepositoryObjectModel
    {
        public FieldModel(RepositoryModel repositoryModel, Field field) : base(repositoryModel) => this.Field = field;

        public Field Field { get; }

        protected override RepositoryObject RepositoryObject => this.Field;
    }
}
