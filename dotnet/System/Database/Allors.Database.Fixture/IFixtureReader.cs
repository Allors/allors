namespace Allors.Database.Fixture;

using System.IO;

public interface IFixtureReader
{
    Fixture Read(Stream stream);
}
