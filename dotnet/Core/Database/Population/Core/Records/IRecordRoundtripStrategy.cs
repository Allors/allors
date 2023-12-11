namespace Allors.Database.Roundtrip;

using System;
using System.Collections.Generic;
using Population;

public interface IRecordRoundtripStrategy
{
    IEnumerable<IObject> Objects();

    Func<IStrategy, Handle> HandleResolver();
}
