namespace Allors.Embedded.Tests.Domain
{
    public interface INamed : IEmbeddedObject
    {
        UnitRole<string> Name { get;  }

        CompositeAssociation<Organization> OrganizationWhereNamed { get; }
    }
}
