namespace Allors.Database.Meta;

using System.Collections.Generic;

public interface ICompositeRoleType : IMetaExtensible
{
    public IComposite Composite { get; }

    public IRoleType RoleType { get; }
}
