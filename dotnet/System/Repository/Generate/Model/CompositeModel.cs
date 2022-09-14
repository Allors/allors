namespace Generate.Model
{
    using Allors.Repository;
    using Allors.Repository.Domain;

    public abstract class CompositeModel : StructuralTypeModel
    {
        protected CompositeModel(RepositoryModel repositoryModel) : base(repositoryModel) { }

        public abstract Composite Composite { get; }
    }
}
