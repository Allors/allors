namespace Allors.Embedded.Domain
{
    public interface INamed : IEmbeddedObject
    {
        IEmbeddedUnitRole<string> Name { get;  }

        IEmbeddedUnitRole<string> UppercasedName { get; }

        IEmbeddedCompositeAssociation<Organization> OrganizationWhereNamed { get; }
    }
}
