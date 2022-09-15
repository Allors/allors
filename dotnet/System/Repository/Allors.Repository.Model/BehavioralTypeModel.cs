namespace Generate.Model
{
    using Allors.Repository;

    public abstract class FieldObjectTypeModel : RepositoryObjectModel
    {
        protected FieldObjectTypeModel(RepositoryModel repositoryModel) : base(repositoryModel)
        {
        }

        public abstract FieldObjectType FieldObjectType { get; }
    }
}
