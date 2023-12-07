namespace Allors.Database.Population;

using System.IO;

public interface IFixtureReader
{
    Fixture Read(Stream stream);
}
