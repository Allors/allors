namespace Allors.Database.Population;

using System.Collections.Generic;
using Meta;

public record Record(
    IClass Class,
    Handle Handle,
    IDictionary<IRoleType, object> ValueByRoleType);
