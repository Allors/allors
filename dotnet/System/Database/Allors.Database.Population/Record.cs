namespace Allors.Database.Population;

using System.Collections.Generic;
using Meta;

public record Record(
    Class Class,
    Handle Handle,
    IDictionary<RoleType, object> ValueByRoleType);
