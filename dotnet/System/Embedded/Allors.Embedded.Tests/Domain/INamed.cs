namespace Allors.Embedded.Tests.Domain
{
    public interface INamed 
    {
        string Name { get; set; }

        Organization OrganizationWhereNamed { get; }
    }
}
