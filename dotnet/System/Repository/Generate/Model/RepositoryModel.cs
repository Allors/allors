namespace Generate.Model
{
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Repository;
    using Allors.Repository.Domain;

    public class RepositoryModel
    {
        private readonly Dictionary<RepositoryObject, RepositoryObjectModel> mapping;

        public RepositoryModel(Repository repository)
        {
            this.Repository = repository;

            this.mapping = new Dictionary<RepositoryObject, RepositoryObjectModel>();

            foreach (var domain in this.Repository.Domains)
            {
                this.mapping.Add(domain, new DomainModel(this, domain));
            }

            foreach (var unit in this.Repository.Units)
            {
                this.mapping.Add(unit, new UnitModel(this, unit));
            }

            foreach (var @interface in this.Repository.Interfaces)
            {
                this.mapping.Add(@interface, new InterfaceModel(this, @interface));
            }

            foreach (var @class in this.Repository.Classes)
            {
                this.mapping.Add(@class, new ClassModel(this, @class));
            }
        }

        public Repository Repository { get; }

        public IEnumerable<RepositoryObjectModel> Objects => this.Repository.Objects.Select(this.Map);

        public IEnumerable<DomainModel> Domains => this.Repository.Domains.Select(this.Map);

        public IEnumerable<UnitModel> Units => this.Repository.Units.Select(this.Map);

        public IEnumerable<CompositeModel> Composites => this.Repository.Composites.Select(this.Map);

        public IEnumerable<InterfaceModel> Interfaces => this.Repository.Interfaces.Select(this.Map);

        public IEnumerable<ClassModel> Classes => this.Repository.Classes.Select(this.Map);

        #region Mappers
        public RepositoryObjectModel Map(RepositoryObject v) => this.mapping[v];

        public BehavioralTypeModel Map(BehavioralType v) => (BehavioralTypeModel)this.mapping[v];

        public StructuralTypeModel Map(StructuralType v) => (StructuralTypeModel)this.mapping[v];

        public CompositeModel Map(Composite v) => (CompositeModel)this.mapping[v];

        public InterfaceModel Map(Interface v) => (InterfaceModel)this.mapping[v];

        public ClassModel Map(Class v) => (ClassModel)this.mapping[v];

        public UnitModel Map(Unit v) => (UnitModel)this.mapping[v];

        public DomainModel Map(Domain v) => (DomainModel)this.mapping[v];

        public PropertyModel Map(Property v) => (PropertyModel)this.mapping[v];

        public MethodModel Map(Method v) => (MethodModel)this.mapping[v];

        public RecordModel Map(Record v) => (RecordModel)this.mapping[v];

        public FieldModel Map(Field v) => (FieldModel)this.mapping[v];
        #endregion
    }
}
