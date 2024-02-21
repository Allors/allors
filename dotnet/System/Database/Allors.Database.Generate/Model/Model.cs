namespace Allors.Meta.Generation.Model;

using System.Collections.Generic;
using System.Linq;
using Database.Meta;
using Database.Population;

public partial class Model
{
    private readonly Dictionary<IMetaExtensible, IMetaExtensibleModel> mapping;
    private readonly Dictionary<Record, RecordModel> recordMapping;
    private readonly Dictionary<Handle, HandleModel> handleMapping;

    public Model(MetaPopulation metaPopulation, IDictionary<Class, Record[]> recordsByClass)
    {
        this.MetaPopulation = metaPopulation;
        this.RecordsByClass = recordsByClass;

        this.mapping = new Dictionary<IMetaExtensible, IMetaExtensibleModel>();

        foreach (var domain in this.MetaPopulation.Domains)
        {
            this.mapping.Add(domain, new DomainModel(this, domain));
        }

        foreach (var unit in this.MetaPopulation.Units)
        {
            this.mapping.Add(unit, new UnitModel(this, unit));
        }

        foreach (var @interface in this.MetaPopulation.Interfaces)
        {
            this.mapping.Add(@interface, new InterfaceModel(this, @interface));
        }

        foreach (var @class in this.MetaPopulation.Classes)
        {
            this.mapping.Add(@class, new ClassModel(this, @class));
            foreach (var compositeRoleType in @class.CompositeRoleTypeByRoleType.Values)
            {
                this.mapping.Add(compositeRoleType, new CompositeRoleTypeModel(this, compositeRoleType));
            }
        }

        foreach (var relationType in this.MetaPopulation.RelationTypes)
        {
            this.mapping.Add(relationType, new RelationTypeModel(this, relationType));
            this.mapping.Add(relationType.AssociationType, new AssociationTypeModel(this, relationType.AssociationType));
            this.mapping.Add(relationType.RoleType, new RoleTypeModel(this, relationType.RoleType));
        }

        foreach (var methodType in this.MetaPopulation.MethodTypes)
        {
            this.mapping.Add(methodType, new MethodTypeModel(this, methodType));
        }

        this.recordMapping = this.RecordsByClass.Values
            .SelectMany(v => v)
            .ToDictionary(v => v, v => new RecordModel(this, v));

        this.handleMapping = this.RecordsByClass.Values
            .SelectMany(v => v)
            .Where(v => v.Handle != null)
            .Select(v => v.Handle)
            .ToDictionary(v => v, v => new HandleModel(this, v));
    }

    public MetaPopulation MetaPopulation { get; }

    public IDictionary<Class, Record[]> RecordsByClass { get; }

    public IEnumerable<DomainModel> Domains => this.MetaPopulation.Domains.Select(this.Map);

    public IEnumerable<UnitModel> Units => this.MetaPopulation.Units.Select(this.Map);

    public IEnumerable<CompositeModel> Composites => this.MetaPopulation.Composites.Select(this.Map);

    public IEnumerable<InterfaceModel> Interfaces => this.MetaPopulation.Interfaces.Select(this.Map);

    public IEnumerable<ClassModel> Classes => this.MetaPopulation.Classes.Select(this.Map);

    public IEnumerable<RelationTypeModel> RelationTypes => this.MetaPopulation.RelationTypes.Select(this.Map);

    public IEnumerable<MethodTypeModel> MethodTypes => this.MetaPopulation.MethodTypes.Select(this.Map);

    public IEnumerable<string> WorkspaceNames => this.MetaPopulation.WorkspaceNames;

    public IReadOnlyDictionary<string, IOrderedEnumerable<string>> WorkspaceOneToOneTagsByWorkspaceName =>
        this.WorkspaceMultiplicityTagsByWorkspaceName(Multiplicity.OneToOne);

    public IReadOnlyDictionary<string, IOrderedEnumerable<string>> WorkspaceOneToManyTagsByWorkspaceName =>
        this.WorkspaceMultiplicityTagsByWorkspaceName(Multiplicity.OneToMany);

    public IReadOnlyDictionary<string, IOrderedEnumerable<string>> WorkspaceManyToManyTagsByWorkspaceName =>
        this.WorkspaceMultiplicityTagsByWorkspaceName(Multiplicity.ManyToMany);

    public IReadOnlyDictionary<string, IOrderedEnumerable<string>> WorkspaceDerivedTagsByWorkspaceName =>
        this.WorkspaceNames
            .ToDictionary(
                v => v,
                v => this.RelationTypes.Where(w => w.IsDerived && w.WorkspaceNames.Contains(v)).Select(w => w.Tag).OrderBy(w => w));

    public IReadOnlyDictionary<string, IOrderedEnumerable<string>> WorkspaceRequiredTagsByWorkspaceName =>
        this.WorkspaceNames
            .ToDictionary(
                v => v,
                v => this.RelationTypes.Where(w => w.RoleType.IsRequired && w.WorkspaceNames.Contains(v)).Select(w => w.Tag)
                    .OrderBy(w => w));

    public IReadOnlyDictionary<string, IOrderedEnumerable<string>> WorkspaceUniqueTagsByWorkspaceName =>
        this.WorkspaceNames
            .ToDictionary(
                v => v,
                v => this.RelationTypes.Where(w => w.RoleType.IsUnique && w.WorkspaceNames.Contains(v)).Select(w => w.Tag).OrderBy(w => w));

    public IReadOnlyDictionary<string, Dictionary<string, IOrderedEnumerable<string>>> WorkspaceMediaTagsByMediaTypeNameByWorkspaceName =>
        this.WorkspaceNames
            .ToDictionary(v => v, v =>
                this.RelationTypes.Where(w => !string.IsNullOrWhiteSpace(w.MediaType) && w.WorkspaceNames.Contains(v))
                    .GroupBy(w => w.MediaType, w => w.Tag)
                    .ToDictionary(w => w.Key, w => w.OrderBy(x => x)));

    public IReadOnlyDictionary<string, IEnumerable<CompositeModel>> WorkspaceCompositesByWorkspaceName =>
        this.WorkspaceNames
            .ToDictionary(v => v, v => this.Composites.Where(w => w.WorkspaceNames.Contains(v)));

    public IReadOnlyDictionary<string, IEnumerable<InterfaceModel>> WorkspaceInterfacesByWorkspaceName =>
        this.WorkspaceNames
            .ToDictionary(v => v, v => this.Interfaces.Where(w => w.WorkspaceNames.Contains(v)));

    public IReadOnlyDictionary<string, IEnumerable<ClassModel>> WorkspaceClassesByWorkspaceName =>
        this.WorkspaceNames
            .ToDictionary(v => v, v => this.Classes.Where(w => w.WorkspaceNames.Contains(v)));

    public IReadOnlyDictionary<string, IOrderedEnumerable<RelationTypeModel>> WorkspaceRelationTypesByWorkspaceName =>
        this.WorkspaceNames
            .ToDictionary(v => v, v => this.RelationTypes.Where(w => w.WorkspaceNames.Contains(v)).OrderBy(w => w.Tag));

    public IReadOnlyDictionary<string, IOrderedEnumerable<RelationTypeModel>> WorkspaceCompositeRelationTypesByWorkspaceName =>
        this.WorkspaceNames
            .ToDictionary(v => v, v => this.RelationTypes.Where(w => w.WorkspaceNames.Contains(v) && w.RoleType.ObjectType.IsComposite).OrderBy(w => w.Tag));

    public IReadOnlyDictionary<string, IOrderedEnumerable<RelationTypeModel>> WorkspaceUnitRelationTypesByWorkspaceName =>
        this.WorkspaceNames
            .ToDictionary(v => v, v => this.RelationTypes.Where(w => w.WorkspaceNames.Contains(v) && w.RoleType.ObjectType.IsUnit).OrderBy(w => w.Tag));

    public IReadOnlyDictionary<string, IOrderedEnumerable<MethodTypeModel>> WorkspaceMethodTypesByWorkspaceName =>
        this.WorkspaceNames
            .ToDictionary(v => v, v => this.MethodTypes.Where(w => w.WorkspaceNames.Contains(v)).OrderBy(w => w.Tag));

    public IReadOnlyDictionary<string, IOrderedEnumerable<string>> WorkspaceMultiplicityTagsByWorkspaceName(Multiplicity multiplicity) =>
        this.WorkspaceNames
            .ToDictionary(
                v => v,
                v => this.RelationTypes
                    .Where(w => w.RoleType.ObjectType.IsComposite && w.Multiplicity == multiplicity && w.WorkspaceNames.Contains(v))
                    .Select(w => w.Tag).OrderBy(w => w));

    #region Mappers
    public IMetaExtensibleModel Map(IMetaExtensible v) => v != null ? this.mapping[v] : null;

    public MetaIdentifiableObjectModel Map(IMetaIdentifiableObject v) => v != null ? (MetaIdentifiableObjectModel)this.mapping[v] : null;

    public DomainModel Map(Domain v) => v != null ? (DomainModel)this.mapping[v] : null;

    public ObjectTypeModel Map(IObjectType v) => v != null ? (ObjectTypeModel)this.mapping[v] : null;

    public UnitModel Map(Unit v) => v != null ? (UnitModel)this.mapping[v] : null;

    public CompositeModel Map(IComposite v) => v != null ? (CompositeModel)this.mapping[v] : null;

    public InterfaceModel Map(Interface v) => v != null ? (InterfaceModel)this.mapping[v] : null;

    public ClassModel Map(Class v) => v != null ? (ClassModel)this.mapping[v] : null;

    public RelationTypeModel Map(RelationType v) => v != null ? (RelationTypeModel)this.mapping[v] : null;

    public AssociationTypeModel Map(AssociationType v) => v != null ? (AssociationTypeModel)this.mapping[v] : null;

    public RoleTypeModel Map(RoleType v) => v != null ? (RoleTypeModel)this.mapping[v] : null;

    public CompositeRoleTypeModel Map(CompositeRoleType v) => v != null ? (CompositeRoleTypeModel)this.mapping[v] : null;

    public MethodTypeModel Map(MethodType v) => v != null ? (MethodTypeModel)this.mapping[v] : null;

    public RecordModel Map(Record v) => v != null ? this.recordMapping[v] : null;

    public HandleModel Map(Handle v) => v != null ? this.handleMapping[v] : null;
    #endregion
}
