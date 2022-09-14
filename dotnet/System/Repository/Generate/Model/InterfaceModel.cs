namespace Generate.Model
{
    using Allors.Repository;
    using Allors.Repository.Domain;

    public class InterfaceModel : CompositeModel
    {
        public InterfaceModel(RepositoryModel repositoryModel, Interface @interface) : base(repositoryModel) => this.Interface = @interface;

        public Interface Interface { get; }

        protected override RepositoryObject RepositoryObject => this.Interface;

        public override BehavioralType BehavioralType => this.Interface;

        public override StructuralType StructuralType => this.Interface;

        public override Composite Composite => this.Interface;
    }
}
