namespace Allors.Embedded.Meta
{
    using System;

    public interface IEmbeddedAssociationType
    {
        EmbeddedObjectType ObjectType { get; }

        IEmbeddedRoleType RoleType { get; }

        string Name { get; }

        string SingularName { get; }

        string PluralName { get; }

        bool IsOne { get; }

        bool IsMany { get; }
    }
}
