namespace Allors.Database.Population;

using System.Collections.Generic;
using Meta;

public interface IRecord
{
    IPopulation Population { get; }

    IClass Class { get; }

    IHandle Handle { get; }

    IDictionary<IRoleType, object> ValueByRoleType { get; }
}
