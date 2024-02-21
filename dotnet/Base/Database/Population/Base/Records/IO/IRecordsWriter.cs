namespace Allors.Database.Population;

using System.Collections.Generic;
using System.IO;
using Meta;

public interface IRecordsWriter
{
    void Write(Stream stream, IDictionary<Class, Record[]> recordsByClass);
}
