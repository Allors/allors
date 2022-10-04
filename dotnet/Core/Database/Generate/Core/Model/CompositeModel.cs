namespace Allors.Meta.Generation.Model;

using System.Collections.Generic;
using System.Linq;
using Database.Meta;

public abstract class CompositeModel : ObjectTypeModel
{
    protected CompositeModel(MetaModel metaModel)
        : base(metaModel)
    {
    }

    protected abstract Composite Composite { get; }

    // IComposite
    public IEnumerable<InterfaceModel> Supertypes => this.Composite.Supertypes.Select(this.MetaModel.Map);

    public IEnumerable<CompositeModel> Subtypes => this.Composite.Subtypes.Select(this.MetaModel.Map);

    public IEnumerable<ClassModel> Classes => this.Composite.Classes.Select(this.MetaModel.Map);

    public bool ExistExclusiveClass => this.Composite.ExclusiveClass != null;

    public IEnumerable<CompositeRoleTypeModel> CompositeRoleTypes => this.Composite.CompositeRoleTypeByRoleType.Values.Select(this.MetaModel.Map);

    public IEnumerable<AssociationTypeModel> InheritedAssociationTypes => this.AssociationTypes.Except(this.ExclusiveAssociationTypes);

    public IEnumerable<RoleTypeModel> InheritedRoleTypes => this.RoleTypes.Except(this.ExclusiveRoleTypes);

    public IEnumerable<MethodTypeModel> MethodTypes => this.Composite.MethodTypes.Select(this.MetaModel.Map);

    public IEnumerable<MethodTypeModel> InheritedMethodTypes => this.MethodTypes.Except(this.ExclusiveMethodTypes);

    public IEnumerable<MethodTypeModel> ExclusiveMethodTypes => this.MethodTypes.Where(v => this.Equals(v.ObjectType));

    // IComposite Extra
    public bool ExistDirectSupertypes => this.DirectSupertypes.Any();

    public bool ExistSupertypes => this.Supertypes.Any();

    public bool ExistAssociationTypes => this.AssociationTypes.Any();

    public bool ExistRoleTypes => this.AssociationTypes.Any();

    public bool ExistMethodTypes => this.MethodTypes.Any();

    public IEnumerable<InterfaceModel> DirectSupertypes => this.Composite.DirectSupertypes.Select(this.MetaModel.Map);

    public IEnumerable<CompositeModel> RelatedComposites =>
        this
            .Supertypes
            .Union(this.RoleTypes.Where(m => m.ObjectType.IsComposite).Select(v => v.ObjectType))
            .Union(this.AssociationTypes.Select(v => v.ObjectType)).Distinct()
            .Except(new[] { this })
            .Cast<CompositeModel>();

    public IEnumerable<AssociationTypeModel> AssociationTypes => this.Composite.AssociationTypes.Select(this.MetaModel.Map);

    public IEnumerable<AssociationTypeModel> ExclusiveAssociationTypes => this.AssociationTypes.Where(v => this.Equals(v.RoleType.ObjectType));

    public IEnumerable<RoleTypeModel> RoleTypes => this.Composite.RoleTypes.Select(this.MetaModel.Map);

    public IEnumerable<RoleTypeModel> ExclusiveRoleTypes => this.RoleTypes.Where(v => this.Equals(v.AssociationType.ObjectType));

    public bool ExistClass => this.Composite.Classes.Count > 0;

    public ClassModel ExclusiveClass => this.MetaModel.Map(this.Composite.ExclusiveClass);
    
    public IEnumerable<RoleTypeModel> SortedExclusiveRoleTypes => this.ExclusiveRoleTypes.OrderBy(v => v.Name);

    public IEnumerable<RoleTypeModel> ExclusiveCompositeRoleTypes =>
        this.ExclusiveRoleTypes.Where(roleType => roleType.ObjectType.IsComposite);

    public IReadOnlyDictionary<string, IOrderedEnumerable<AssociationTypeModel>> WorkspaceAssociationTypesByWorkspaceName =>
        this.WorkspaceNames
            .ToDictionary(v => v,
                v => this.AssociationTypes.Where(w => w.RelationType.WorkspaceNames.Contains(v)).OrderBy(w => w.RelationType.Tag));

    public IReadOnlyDictionary<string, IOrderedEnumerable<AssociationTypeModel>> WorkspaceExclusiveAssociationTypesByWorkspaceName =>
        this.WorkspaceNames
            .ToDictionary(v => v,
                v => this.ExclusiveAssociationTypes.Where(w => w.RelationType.WorkspaceNames.Contains(v)).OrderBy(w => w.RelationType.Tag));

    public IReadOnlyDictionary<string, IOrderedEnumerable<AssociationTypeModel>> WorkspaceInheritedAssociationTypesByWorkspaceName =>
        this.WorkspaceNames
            .ToDictionary(v => v,
                v => this.InheritedAssociationTypes.Where(w => w.RelationType.WorkspaceNames.Contains(v)).OrderBy(w => w.RelationType.Tag));

    public IReadOnlyDictionary<string, IOrderedEnumerable<RoleTypeModel>> WorkspaceRoleTypesByWorkspaceName =>
        this.WorkspaceNames
            .ToDictionary(v => v,
                v => this.RoleTypes.Where(w => w.RelationType.WorkspaceNames.Contains(v)).OrderBy(w => w.RelationType.Tag));

    public IReadOnlyDictionary<string, IOrderedEnumerable<RoleTypeModel>> WorkspaceCompositeRoleTypesByWorkspaceName =>
        this.WorkspaceNames
            .ToDictionary(v => v,
                v => this.RoleTypes.Where(w => w.ObjectType.IsComposite && w.RelationType.WorkspaceNames.Contains(v))
                    .OrderBy(w => w.RelationType.Tag));

    public IReadOnlyDictionary<string, IOrderedEnumerable<RoleTypeModel>> WorkspaceInheritedRoleTypesByWorkspaceName =>
        this.WorkspaceNames
            .ToDictionary(v => v,
                v => this.InheritedRoleTypes.Where(w => w.RelationType.WorkspaceNames.Contains(v)).OrderBy(w => w.RelationType.Tag));

    public IReadOnlyDictionary<string, IOrderedEnumerable<RoleTypeModel>> WorkspaceExclusiveRoleTypesByWorkspaceName =>
        this.WorkspaceNames
            .ToDictionary(v => v,
                v => this.ExclusiveRoleTypes.Where(w => w.RelationType.WorkspaceNames.Contains(v)).OrderBy(w => w.RelationType.Tag));

    public IReadOnlyDictionary<string, IOrderedEnumerable<RoleTypeModel>> WorkspaceExclusiveCompositeRoleTypesByWorkspaceName =>
        this.WorkspaceNames
            .ToDictionary(v => v,
                v => this.ExclusiveRoleTypes.Where(w => w.ObjectType.IsComposite && w.RelationType.WorkspaceNames.Contains(v))
                    .OrderBy(w => w.RelationType.Tag));

    public IReadOnlyDictionary<string, IOrderedEnumerable<MethodTypeModel>> WorkspaceExclusiveMethodTypesByWorkspaceName =>
        this.WorkspaceNames
            .ToDictionary(v => v, v => this.ExclusiveMethodTypes.Where(w => w.WorkspaceNames.Contains(v)).OrderBy(w => w.Tag));

    public IReadOnlyDictionary<string, IOrderedEnumerable<MethodTypeModel>> WorkspaceInheritedMethodTypesByWorkspaceName =>
        this.WorkspaceNames
            .ToDictionary(v => v, v => this.InheritedMethodTypes.Where(w => w.WorkspaceNames.Contains(v)).OrderBy(w => w.Tag));

    public IReadOnlyDictionary<string, IOrderedEnumerable<MethodTypeModel>> WorkspaceMethodTypesByWorkspaceName =>
        this.WorkspaceNames
            .ToDictionary(v => v, v => this.MethodTypes.Where(w => w.WorkspaceNames.Contains(v)).OrderBy(w => w.Tag));

    public IReadOnlyDictionary<string, IOrderedEnumerable<InterfaceModel>> WorkspaceDirectSupertypesByWorkspaceName => this.WorkspaceNames
        .ToDictionary(v => v, v => this.DirectSupertypes.Where(w => w.WorkspaceNames.Contains(v)).OrderBy(w => w.Tag));

    public IReadOnlyDictionary<string, IOrderedEnumerable<InterfaceModel>> WorkspaceSupertypesByWorkspaceName =>
        this.WorkspaceNames
            .ToDictionary(v => v, v => this.Supertypes.Where(w => w.WorkspaceNames.Contains(v)).OrderBy(w => w.Tag));

    public IReadOnlyDictionary<string, IOrderedEnumerable<CompositeModel>> WorkspaceSubtypesByWorkspaceName =>
        this.WorkspaceNames
            .ToDictionary(v => v, v => this.Subtypes.Where(w => w.WorkspaceNames.Contains(v)).OrderBy(w => w.Tag));

    public IReadOnlyDictionary<string, IOrderedEnumerable<CompositeModel>> WorkspaceRelatedCompositesByWorkspaceName => this.WorkspaceNames
        .ToDictionary(v => v, v => this.RelatedComposites.Where(w => w.WorkspaceNames.Contains(v)).OrderBy(w => w.Tag));
}
