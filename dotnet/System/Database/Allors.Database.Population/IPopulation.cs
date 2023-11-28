namespace Allors.Database.Population;

using System.Collections.Generic;
using Meta;

public interface IPopulation
{
    IDictionary<IClass, IObject> ObjectsByClass { get; }
}
