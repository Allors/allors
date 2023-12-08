namespace Allors.Database.Population;

using System.Collections.Generic;
using Meta;

public record Translation(
    IClass Class,
    string IsoCode,
    IDictionary<string, string> ValueByName);
