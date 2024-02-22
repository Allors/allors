namespace Allors.Meta.Generation.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using Allors.Database.Meta;
using Database.Population;

public sealed class ClassModel : CompositeModel
{
    public ClassModel(Model model, Class @class)
        : base(model) => this.Class = @class;

    protected override IObjectType ObjectType => this.Class;

    protected override Composite Composite => this.Class;

    public Class Class { get; }

    public override IMetaIdentifiableObject MetaObject => this.Class;

    // IClass
    public IEnumerable<RoleTypeModel> OverriddenRequiredRoleTypes => this.CompositeRoleTypes.Where(v => v.IsAssignedRequired).Select(v => v.RoleType);

    // IClass Extra
    public IReadOnlyDictionary<string, IEnumerable<RoleTypeModel>> WorkspaceOverriddenRequiredByWorkspaceName => this.WorkspaceNames
        .ToDictionary(
            v => v,
            v => this.OverriddenRequiredRoleTypes.Where(w => w.RelationType.WorkspaceNames.Contains(v)));

    // Population
    public IEnumerable<RecordModel> Objects
    {
        get
        {
            this.Model.RecordsByClass.TryGetValue(this.Class, out var objects);
            return objects?.Select(this.Model.Map);
        }
    }
}
