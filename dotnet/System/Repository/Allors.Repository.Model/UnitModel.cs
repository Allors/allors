namespace Generate.Model
{
    using Allors.Repository;
    using Allors.Repository.Domain;

    public class UnitModel : StructuralTypeModel
    {
        public UnitModel(RepositoryModel repositoryModel, Unit unit) : base(repositoryModel) => this.Unit = unit;

        public Unit Unit { get; }

        protected override RepositoryObject RepositoryObject => this.Unit;

        public override BehavioralType BehavioralType => this.Unit;

        public override StructuralType StructuralType => this.Unit;
    }
}
