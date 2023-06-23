namespace Allors.Embedded.Meta
{
    public interface IEmbeddedRoleType
    {
        EmbeddedObjectType ObjectType { get; }

        IEmbeddedAssociationType AssociationType { get; }

        string Name { get; }

        string SingularName { get; }

        string PluralName { get; }

        bool IsOne { get; }

        bool IsMany { get; }

        bool IsUnit { get; }

        object Normalize(object value);
    }
}