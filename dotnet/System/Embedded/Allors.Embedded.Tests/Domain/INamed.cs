namespace Allors.Embedded.Tests.Domain
{
    public interface INamed
    {
        StringRole Name { get;  }

        Organization OrganizationWhereNamed { get; }
    }
}
