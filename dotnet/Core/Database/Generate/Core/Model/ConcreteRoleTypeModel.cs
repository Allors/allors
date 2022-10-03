namespace Allors.Meta.Generation.Model;

using Allors.Database.Meta;
using Allors.Database.Meta.Extensions;
using Allors.Meta.Generation.Model;

public class ConcreteRoleTypeModel : IMetaExtensibleModel
{
    public ConcreteRoleTypeModel(MetaModel metaModel, IConcreteRoleType concreteRoleType)
    {
        this.MetaModel = metaModel;
        this.ConcreteRoleType = concreteRoleType;
    }

    public MetaModel MetaModel { get; }

    public IMetaExtensible MetaExtensible => this.ConcreteRoleType;

    public dynamic Extensions => this.MetaExtensible.Extensions;

    public IConcreteRoleType ConcreteRoleType { get; }

    public RoleTypeModel RoleType => this.MetaModel.Map(this.ConcreteRoleType.RoleType);

    // IConcreteRoleType
    public bool IsRequired => this.ConcreteRoleType.Required();

    public bool IsRequiredOverridden => this.ConcreteRoleType.RequiredOverridden();

    public bool IsUnique => this.ConcreteRoleType.Unique();

    public bool IsUniqueOverridden => this.ConcreteRoleType.UniqueOverriden();
}
