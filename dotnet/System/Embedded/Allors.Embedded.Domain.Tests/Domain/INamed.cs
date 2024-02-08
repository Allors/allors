namespace Allors.Embedded.Domain
{
    public interface INamed : IEmbeddedObject
    {
        IUnitRole<string> Name { get;  }

        IUnitRole<string> UppercasedName { get; }

        ICompositeAssociation<Organization> OrganizationWhereNamed { get; }
    }
}
