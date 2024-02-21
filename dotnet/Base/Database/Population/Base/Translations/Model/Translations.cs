namespace Allors.Database.Population;

using System.Collections.Generic;
using Meta;

public record Translations(
    Class Class,
    string IsoCode,
    IDictionary<string, string> ValueByKey);


public record Translation(
    Class Class,
    string IsoCode,
    string Key,
    string Value);
