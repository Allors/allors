namespace Generate.Model
{
    using Allors.Repository;
    using Allors.Repository.Domain;

    public class ClassModel : CompositeModel
    {
        public ClassModel(RepositoryModel repositoryModel, Class @class) : base(repositoryModel) => this.Class = @class;

        public Class Class { get; }

        protected override RepositoryObject RepositoryObject => this.Class;

        public override BehavioralType BehavioralType => this.Class;

        public override StructuralType StructuralType => this.Class;

        public override Composite Composite => this.Class;
    }
}
