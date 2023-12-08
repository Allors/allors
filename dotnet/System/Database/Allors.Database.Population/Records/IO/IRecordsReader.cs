namespace Allors.Database.Population;

using System.Collections.Generic;
using System.IO;
using Meta;

public interface IRecordsReader
{
    IDictionary<IClass, Record[]> Read(Stream stream);
}
