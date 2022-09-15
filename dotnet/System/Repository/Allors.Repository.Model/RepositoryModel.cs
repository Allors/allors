namespace Generate.Model
{
    using System;
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

            foreach (var @object in this.Repository.Objects)
            {
                if (@object is Domain domain)
                {
                    this.mapping.Add(domain, new DomainModel(this, domain));
                }
                else if (@object is Unit unit)
                {
                    this.mapping.Add(unit, new UnitModel(this, unit));
                }
                else if (@object is Interface @interface)
                {
                    this.mapping.Add(@interface, new InterfaceModel(this, @interface));
                }
                else if (@object is Class @class)
                {
                    this.mapping.Add(@class, new ClassModel(this, @class));
                }
                else if (@object is Property property)
                {
                    this.mapping.Add(property, new PropertyModel(this, property));
                }
                else if (@object is Method method)
                {
                    this.mapping.Add(method, new MethodModel(this, method));
                }
                else if (@object is Record record)
                {
                    this.mapping.Add(record, new RecordModel(this, record));
                }
                else if (@object is Field field)
                {
                    this.mapping.Add(field, new FieldModel(this, field));
                }
                else
                {
                    throw new Exception($"Missing mapping for {@object}");
                }
            }
        }

        public Repository Repository { get; }

        public IEnumerable<RepositoryObjectModel> Objects => this.Repository.Objects.Select(this.Map);

        public IEnumerable<DomainModel> Domains => this.Repository.Objects.OfType<Domain>().Select(this.Map);

        public IEnumerable<UnitModel> Units => this.Repository.Objects.OfType<Unit>().Select(this.Map);

        public IEnumerable<CompositeModel> Composites => this.Repository.Objects.OfType<Composite>().Select(this.Map);

        public IEnumerable<InterfaceModel> Interfaces => this.Repository.Objects.OfType<Interface>().Select(this.Map);

        public IEnumerable<ClassModel> Classes => this.Repository.Objects.OfType<Class>().Select(this.Map);

        public DomainModel[] SortedDomains
        {
            get
            {
                var domains = this.Domains.ToList();
                domains.Sort((x, y) => x.Base == y ? 1 : -1);
                return domains.ToArray();
            }
        }

        #region Mappers
        public RepositoryObjectModel Map(RepositoryObject v) => v != null ? this.mapping[v] : null;

        public FieldObjectTypeModel Map(FieldObjectType v) => v != null ? (FieldObjectTypeModel)this.mapping[v] : null;

        public ObjectTypeModel Map(ObjectType v) => v != null ? (ObjectTypeModel)this.mapping[v] : null;

        public CompositeModel Map(Composite v) => v != null ? (CompositeModel)this.mapping[v] : null;

        public InterfaceModel Map(Interface v) => v != null ? (InterfaceModel)this.mapping[v] : null;

        public ClassModel Map(Class v) => v != null ? (ClassModel)this.mapping[v] : null;

        public UnitModel Map(Unit v) => v != null ? (UnitModel)this.mapping[v] : null;

        public DomainModel Map(Domain v) => v != null ? (DomainModel)this.mapping[v] : null;

        public PropertyModel Map(Property v) => v != null ? (PropertyModel)this.mapping[v] : null;

        public MethodModel Map(Method v) => v != null ? (MethodModel)this.mapping[v] : null;

        public RecordModel Map(Record v) => v != null ? (RecordModel)this.mapping[v] : null;

        public FieldModel Map(Field v) => v != null ? (FieldModel)this.mapping[v] : null;
        #endregion
    }
}
