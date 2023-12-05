namespace Allors.Database.Population;

using System.Collections.Generic;
using Meta;

public record Fixture(IDictionary<IClass, Record[]> ObjectsByClass);
