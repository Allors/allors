namespace Allors.Database.Population;

using System.Collections.Generic;
using Meta;

public record Translations(
    IClass Class,
    string IsoCode,
    IDictionary<string, string> ValueByKey);


public record Translation(
    IClass Class,
    string IsoCode,
    string Key,
    string Value);
