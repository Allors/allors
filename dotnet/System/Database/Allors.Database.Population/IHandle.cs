namespace Allors.Database.Population;

using Meta;

public interface IHandle
{
    IRecord Record { get; }

    IRoleType RoleType { get; }

    string Name { get; }

    object Value { get; }
}
