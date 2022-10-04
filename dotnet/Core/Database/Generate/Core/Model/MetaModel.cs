namespace Allors.Meta.Generation.Model;

using System.Collections.Generic;
using System.Linq;
using Database.Meta;

public class MetaModel
{
    private readonly Dictionary<IMetaExtensible, IMetaExtensibleModel> mapping;

    public MetaModel(MetaPopulation metaPopulation)
    {
        this.MetaPopulation = metaPopulation;

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

        foreach (var record in this.MetaPopulation.Records)
        {
            this.mapping.Add(record, new RecordModel(this, record));
        }

        foreach (var methodType in this.MetaPopulation.MethodTypes)
        {
            this.mapping.Add(methodType, new MethodTypeModel(this, methodType));
        }
    }

    public MetaPopulation MetaPopulation { get; }

    public IEnumerable<DomainModel> Domains => this.MetaPopulation.Domains.Select(this.Map);

    public IEnumerable<UnitModel> Units => this.MetaPopulation.Units.Select(this.Map);

    public IEnumerable<CompositeModel> Composites => this.MetaPopulation.Composites.Select(this.Map);

    public IEnumerable<InterfaceModel> Interfaces => this.MetaPopulation.Interfaces.Select(this.Map);

    public IEnumerable<ClassModel> Classes => this.MetaPopulation.Classes.Select(this.Map);

    public IEnumerable<RelationTypeModel> RelationTypes => this.MetaPopulation.RelationTypes.Select(this.Map);

    public IEnumerable<RecordModel> Records => this.MetaPopulation.Records.Select(this.Map);

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
            .ToDictionary(v => v,
                v => this.RelationTypes.Where(w => w.IsDerived && w.WorkspaceNames.Contains(v)).Select(w => w.Tag).OrderBy(w => w));

    public IReadOnlyDictionary<string, IOrderedEnumerable<string>> WorkspaceRequiredTagsByWorkspaceName =>
        this.WorkspaceNames
            .ToDictionary(v => v,
                v => this.RelationTypes.Where(w => w.RoleType.IsRequired && w.WorkspaceNames.Contains(v)).Select(w => w.Tag)
                    .OrderBy(w => w));

    public IReadOnlyDictionary<string, IOrderedEnumerable<string>> WorkspaceUniqueTagsByWorkspaceName =>
        this.WorkspaceNames
            .ToDictionary(v => v,
                v => this.RelationTypes.Where(w => w.RoleType.IsUnique && w.WorkspaceNames.Contains(v)).Select(w => w.Tag).OrderBy(w => w));

    public IReadOnlyDictionary<string, Dictionary<string, IOrderedEnumerable<string>>> WorkspaceMediaTagsByMediaTypeNameByWorkspaceName =>
        this.WorkspaceNames
            .ToDictionary(v => v, v =>
                this.RelationTypes.Where(w => !string.IsNullOrWhiteSpace(w.MediaType) && w.WorkspaceNames.Contains(v))
                    .GroupBy(w => w.MediaType, w => w.Tag)
                    .ToDictionary(w => w.Key, w => w.OrderBy(x => x)));

    public IReadOnlyDictionary<string, IOrderedEnumerable<CompositeModel>> WorkspaceCompositesByWorkspaceName =>
        this.WorkspaceNames
            .ToDictionary(v => v, v => this.Composites.Where(w => w.WorkspaceNames.Contains(v)).OrderBy(w => w.Tag));

    public IReadOnlyDictionary<string, IOrderedEnumerable<InterfaceModel>> WorkspaceInterfacesByWorkspaceName =>
        this.WorkspaceNames
            .ToDictionary(v => v, v => this.Interfaces.Where(w => w.WorkspaceNames.Contains(v)).OrderBy(w => w.Tag));

    public IReadOnlyDictionary<string, IOrderedEnumerable<ClassModel>> WorkspaceClassesByWorkspaceName =>
        this.WorkspaceNames
            .ToDictionary(v => v, v => this.Classes.Where(w => w.WorkspaceNames.Contains(v)).OrderBy(w => w.Tag));

    public IReadOnlyDictionary<string, IOrderedEnumerable<RelationTypeModel>> WorkspaceRelationTypesByWorkspaceName =>
        this.WorkspaceNames
            .ToDictionary(v => v, v => this.RelationTypes.Where(w => w.WorkspaceNames.Contains(v)).OrderBy(w => w.Tag));

    public IReadOnlyDictionary<string, IOrderedEnumerable<MethodTypeModel>> WorkspaceMethodTypesByWorkspaceName =>
        this.WorkspaceNames
            .ToDictionary(v => v, v => this.MethodTypes.Where(w => w.WorkspaceNames.Contains(v)).OrderBy(w => w.Tag));

    public IReadOnlyDictionary<string, IOrderedEnumerable<string>> WorkspaceMultiplicityTagsByWorkspaceName(Multiplicity multiplicity) =>
        this.WorkspaceNames
            .ToDictionary(v => v,
                v => this.RelationTypes
                    .Where(w => w.RoleType.ObjectType.IsComposite && w.Multiplicity == multiplicity && w.WorkspaceNames.Contains(v))
                    .Select(w => w.Tag).OrderBy(w => w));

    #region Mappers
    public IMetaExtensibleModel Map(IMetaExtensible v) => this.mapping[v];

    public MetaIdentifiableObjectModel Map(IMetaIdentifiableObject v) => (MetaIdentifiableObjectModel)this.mapping[v];

    public DomainModel Map(IDomain v) => (DomainModel)this.mapping[v];

    public ObjectTypeModel Map(IObjectType v) => (ObjectTypeModel)this.mapping[v];

    public UnitModel Map(IUnit v) => (UnitModel)this.mapping[v];

    public CompositeModel Map(IComposite v) => (CompositeModel)this.mapping[v];

    public InterfaceModel Map(IInterface v) => (InterfaceModel)this.mapping[v];

    public ClassModel Map(IClass v) => (ClassModel)this.mapping[v];

    public RelationTypeModel Map(IRelationType v) => (RelationTypeModel)this.mapping[v];

    public AssociationTypeModel Map(IAssociationType v) => (AssociationTypeModel)this.mapping[v];

    public RoleTypeModel Map(IRoleType v) => (RoleTypeModel)this.mapping[v];

    public CompositeRoleTypeModel Map(ICompositeRoleType v) => (CompositeRoleTypeModel)this.mapping[v];

    public RecordModel Map(IRecord v) => (RecordModel)this.mapping[v];

    public MethodTypeModel Map(IMethodType v) => (MethodTypeModel)this.mapping[v];
    #endregion
}
