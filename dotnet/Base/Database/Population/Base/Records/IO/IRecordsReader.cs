namespace Allors.Database.Population;

using System.Collections.Generic;
using System.IO;
using Meta;

public interface IRecordsReader
{
    IDictionary<Class, Record[]> Read(Stream stream);
}
