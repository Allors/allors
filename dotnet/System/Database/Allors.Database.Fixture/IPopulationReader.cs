namespace Allors.Database.Population;

using System.IO;

public interface IPopulationReader
{
    Fixture Read(Stream stream);
}
