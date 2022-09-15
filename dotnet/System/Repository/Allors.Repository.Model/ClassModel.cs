namespace Generate.Model
{
    using Allors.Repository;
    using Allors.Repository.Domain;
    using System.Linq;

    public class ClassModel : CompositeModel
    {
        public ClassModel(RepositoryModel repositoryModel, Class @class) : base(repositoryModel) => this.Class = @class;

        public Class Class { get; }

        protected override RepositoryObject RepositoryObject => this.Class;

        public override FieldObjectType FieldObjectType => this.Class;

        public override ObjectType ObjectType => this.Class;

        public override Composite Composite => this.Class;

        public override InterfaceModel[] Interfaces => this.Class.Interfaces.Select(this.RepositoryModel.Map).ToArray();

        public string[] WorkspaceNames => this.Class.WorkspaceNames;

        public PropertyModel[] InheritedRequiredProperties => this.Class.InheritedProperties.Where(v => v.Required)
            .Select(this.RepositoryModel.Map)
            .ToArray();

        public PropertyModel[] InheritedUniqueProperties => this.Class.InheritedProperties.Where(v => v.Unique)
            .Select(this.RepositoryModel.Map)
            .ToArray();

    }
}
