namespace Allors.Embedded.Meta
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IEmbeddedObjectType
    {
        IEmbeddedMeta Meta { get; }

        Type Type { get; }

        TypeCode TypeCode { get; }

        ISet<IEmbeddedObjectType> SuperTypes { get; }

        IDictionary<string, IEmbeddedAssociationType> AssociationTypeByName { get; }

        IDictionary<string, IEmbeddedRoleType> RoleTypeByName { get; }

        IEmbeddedRoleType AddUnit(IEmbeddedObjectType roleObjectType, string roleName);

        IEmbeddedRoleType AddOneToOne(IEmbeddedObjectType roleObjectType, string roleName);

        IEmbeddedRoleType AddManyToOne(IEmbeddedObjectType roleObjectType, string roleName);

        IEmbeddedRoleType AddOneToMany(IEmbeddedObjectType roleObjectType, string roleName);

        IEmbeddedRoleType AddManyToMany(IEmbeddedObjectType roleObjectType, string roleName);

        void ResetDerivations();

        void AddAssociationType(IEmbeddedAssociationType associationType);

        void AddRoleType(IEmbeddedRoleType roleType);

        void CheckNames(string singularName, string pluralName);
    }
}
