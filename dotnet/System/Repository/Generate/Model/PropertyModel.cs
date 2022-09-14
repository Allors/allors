namespace Generate.Model
{
    using Allors.Repository;
    using Allors.Repository.Domain;

    public class PropertyModel : RepositoryObjectModel
    {
        public PropertyModel(RepositoryModel repositoryModel, Property property) : base(repositoryModel) => this.Property = property;

        public Property Property { get; }

        protected override RepositoryObject RepositoryObject => this.Property;
    }
}
