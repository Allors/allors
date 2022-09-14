namespace Generate.Model
{
    using Allors.Repository.Domain;

    public abstract class StructuralTypeModel : BehavioralTypeModel
    {
        protected StructuralTypeModel(RepositoryModel repositoryModel) : base(repositoryModel)
        {
        }

        public abstract StructuralType StructuralType { get; }
    }
}
