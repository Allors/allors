namespace Allors.Meta.Generation.Model;

using Allors.Database.Meta;

public class CompositeRoleTypeModel : IMetaExtensibleModel
{
    public CompositeRoleTypeModel(Model model, CompositeRoleType compositeRoleType)
    {
        this.Model = model;
        this.CompositeRoleType = compositeRoleType;
    }

    public Model Model { get; }

    public CompositeRoleType CompositeRoleType { get; }

    public RoleTypeModel RoleType => this.Model.Map(this.CompositeRoleType.RoleType);

    // ICompositeRoleType
    public bool IsRequired => this.CompositeRoleType.IsRequired;

    public bool IsAssignedRequired => this.CompositeRoleType.AssignedIsRequired ?? false;

    public bool IsUnique => this.CompositeRoleType.IsUnique;

    public bool IsAssignedUnique => this.CompositeRoleType.AssignedIsUnique ?? false;
}
