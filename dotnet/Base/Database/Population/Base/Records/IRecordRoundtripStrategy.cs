namespace Allors.Database.Population;

using System;
using System.Collections.Generic;
using Meta;

public interface IRecordRoundtripStrategy
{
    IEnumerable<IObject> Objects();

    Func<IStrategy, Handle> HandleResolver();

    Func<IStrategy, IRoleType, bool> RoleFilter();
}
