namespace Allors.Database.Population;

using System.IO;

public interface IFixtureWriter
{
    void Write(Stream stream, Fixture fixture);
}
