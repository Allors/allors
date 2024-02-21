namespace Allors.Meta.Generation.Model;

using Allors.Database.Meta;
using Allors.Database.Meta.Extensions;

public class CompositeRoleTypeModel : IMetaExtensibleModel
{
    public CompositeRoleTypeModel(Model model, CompositeRoleType compositeRoleType)
    {
        this.Model = model;
        this.CompositeRoleType = compositeRoleType;
    }

    public Model Model { get; }

    public IMetaExtensible MetaExtensible => this.CompositeRoleType;

    public dynamic Extensions => this.MetaExtensible.Attributes;

    public CompositeRoleType CompositeRoleType { get; }

    public RoleTypeModel RoleType => this.Model.Map(this.CompositeRoleType.RoleType);

    // ICompositeRoleType
    public bool IsRequired => this.CompositeRoleType.IsRequired();

    public bool IsAssignedRequired => this.CompositeRoleType.Attributes.IsAssignedRequired ?? false;

    public bool IsUnique => this.CompositeRoleType.IsUnique();

    public bool IsAssignedUnique => this.CompositeRoleType.Attributes.IsAssignedUnique ?? false;
}
