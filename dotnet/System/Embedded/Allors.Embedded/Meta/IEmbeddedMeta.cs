namespace Allors.Embedded.Meta
{
    using System;
    using System.Collections.Generic;

    public interface IEmbeddedMeta
    {
        IDictionary<Type, EmbeddedObjectType> ObjectTypeByType { get; }

        string Pluralize(string singular);

        EmbeddedUnitRoleType AddUnit<TAssociation, TRole>(string roleName);

        EmbeddedOneToOneRoleType AddOneToOne<TAssociation, TRole>(string roleName);

        EmbeddedManyToOneRoleType AddManyToOne<TAssociation, TRole>(string roleName);

        EmbeddedOneToManyRoleType AddOneToMany<TAssociation, TRole>(string roleName);

        EmbeddedManyToManyRoleType AddManyToMany<TAssociation, TRole>(string roleName);

        EmbeddedObjectType GetOrAddObjectType(Type type);

        void ResetDerivations();
    }
}
