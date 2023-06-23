namespace Allors.Embedded.Tests.Domain
{
    public interface INamed : IEmbeddedObject
    {
        UnitRole<string> Name { get;  }

        Organization OrganizationWhereNamed { get; }
    }
}
