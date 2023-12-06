namespace Allors.Database.Fixture;

using System.IO;

public interface IFixtureWriter
{
    void Write(Stream stream, Fixture fixture);
}
