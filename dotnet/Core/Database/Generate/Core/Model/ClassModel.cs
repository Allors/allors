namespace Allors.Meta.Generation.Model;

using System.Collections.Generic;
using System.Linq;
using Database.Meta;

public sealed class ClassModel : CompositeModel
{
    public ClassModel(MetaModel metaModel, Class @class)
        : base(metaModel) => this.Class = @class;

    protected override ObjectType ObjectType => this.Class;

    protected override Composite Composite => this.Class;

    public Class Class { get; }

    public override IMetaIdentifiableObject MetaObject => this.Class;

    // IClass
    public IEnumerable<RoleTypeModel> OverriddenRequiredRoleTypes => this.CompositeRoleTypes.Where(v => v.IsAssignedRequired).Select(v => v.RoleType);

    // IClass Extra
    public IReadOnlyDictionary<string, IOrderedEnumerable<RoleTypeModel>> WorkspaceOverriddenRequiredByWorkspaceName => this.WorkspaceNames
        .ToDictionary(v => v,
            v => this.OverriddenRequiredRoleTypes.Where(w => w.RelationType.WorkspaceNames.Contains(v)).OrderBy(w => w.RelationType.Tag));
}
