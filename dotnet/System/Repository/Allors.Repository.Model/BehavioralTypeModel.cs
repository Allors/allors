namespace Generate.Model
{
    using Allors.Repository;

    public abstract class BehavioralTypeModel : RepositoryObjectModel
    {
        protected BehavioralTypeModel(RepositoryModel repositoryModel) : base(repositoryModel)
        {
        }

        public abstract BehavioralType BehavioralType { get; }
    }
}
