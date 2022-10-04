namespace Allors.Meta.Generation.Model;

using Allors.Database.Meta;
using Allors.Database.Meta.Extensions;
using Allors.Meta.Generation.Model;

public class CompositeRoleTypeModel : IMetaExtensibleModel
{
    public CompositeRoleTypeModel(MetaModel metaModel, ICompositeRoleType compositeRoleType)
    {
        this.MetaModel = metaModel;
        this.CompositeRoleType = compositeRoleType;
    }

    public MetaModel MetaModel { get; }

    public IMetaExtensible MetaExtensible => this.CompositeRoleType;

    public dynamic Extensions => this.MetaExtensible.Extensions;

    public ICompositeRoleType CompositeRoleType { get; }

    public RoleTypeModel RoleType => this.MetaModel.Map(this.CompositeRoleType.RoleType);

    // ICompositeRoleType
    public bool IsRequired => this.CompositeRoleType.Required();

    public bool IsRequiredOverridden => this.CompositeRoleType.RequiredOverridden();

    public bool IsUnique => this.CompositeRoleType.Unique();

    public bool IsUniqueOverridden => this.CompositeRoleType.UniqueOverriden();
}
