namespace Allors.Database.Fixture;

using System.Collections.Generic;
using Meta;

public record Fixture(IDictionary<IClass, Record[]> RecordsByClass);
