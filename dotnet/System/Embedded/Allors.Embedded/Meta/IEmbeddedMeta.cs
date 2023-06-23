namespace Allors.Embedded.Meta
{
    using System;
    using System.Collections.Generic;

    public interface IEmbeddedMeta
    {
        IDictionary<Type, IEmbeddedObjectType> ObjectTypeByType { get; }

        string Pluralize(string singular);

        IEmbeddedRoleType AddUnit<TAssociation, TRole>(string roleName);

        IEmbeddedRoleType AddOneToOne<TAssociation, TRole>(string roleName);

        IEmbeddedRoleType AddManyToOne<TAssociation, TRole>(string roleName);

        IEmbeddedRoleType AddOneToMany<TAssociation, TRole>(string roleName);

        IEmbeddedRoleType AddManyToMany<TAssociation, TRole>(string roleName);

        IEmbeddedObjectType GetOrAddObjectType(Type type);

        void ResetDerivations();
    }
}
