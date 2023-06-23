namespace Allors.Embedded.Tests.Domain
{
    public interface INamed : IEmbeddedObject
    {
        IUnitRole<string> Name { get;  }

        ICompositeAssociation<Organization> OrganizationWhereNamed { get; }
    }
}
