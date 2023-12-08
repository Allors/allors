namespace Allors.Database.Population;

using System.Collections.Generic;
using System.IO;
using Meta;

public interface IFixtureWriter
{
    void Write(Stream stream, IDictionary<IClass, Record[]> recordsByClass);
}
