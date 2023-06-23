namespace Allors.Embedded.Meta
{
    public interface IEmbeddedAssociationType
    {
        IEmbeddedObjectType ObjectType { get; }

        IEmbeddedRoleType RoleType { get; }

        string Name { get; }

        string SingularName { get; }

        string PluralName { get; }

        bool IsOne { get; }

        bool IsMany { get; }
    }
}
